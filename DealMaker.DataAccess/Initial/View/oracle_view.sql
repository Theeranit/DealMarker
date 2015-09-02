DROP VIEW OPICS.V_DMK_CASHFLOW;

/* Formatted on 10/15/2013 10:00:03 AM (QP5 v5.163.1008.3004) */
CREATE OR REPLACE FORCE VIEW OPICS.V_DMK_CASHFLOW
(
   EXT_DEAL_NO,
   LEG,
   SEQ,
   RATE,
   FLOW_DATE,
   FLOW_AMOUNT,
   FLOW_AMOUNT_THB,
   DEALDATE,
   MATDATE
)
AS
   SELECT TRIM (data.DEALNO) AS EXT_DEAL_NO,
          CASE WHEN data.LEG = 1 THEN 1 ELSE 0 END AS LEG,
          data.SEQ AS SEQ,
          data.INTRATE AS RATE,
          data.FLOWDATE AS FLOW_DATE,
          data.FLOWAMOUNT AS FLOW_AMOUNT,
          data.FLOWAMOUNT_THB AS FLOW_AMOUNT_THB,
          data.DEALDATE,
          data.MATDATE
     FROM (  SELECT t.DEALNO AS DEALNO,
                    CAST (t.SEQ AS NUMBER (3)) AS LEG,
                    CAST (t.SCHDSEQ AS NUMBER (3)) AS SEQ,
                    CASE WHEN t.FIXFLOATIND = 'X' THEN 'FIXED' ELSE 'FLOAT' END
                       AS FIXFLOATIND,
                    CAST (
                       (CASE
                           WHEN t.INTRATE_8 = 0 AND t.IMPLINTRATE_8 = 0
                           THEN
                              flleg.FLOW_RATE
                           WHEN t.INTRATE_8 = 0
                           THEN
                              t.IMPLINTRATE_8
                           ELSE
                              t.INTRATE_8
                        END)
                       + t.SPREAD_8 AS NUMBER (18, 6))
                       AS INTRATE,
                    NVL (t.IPAYDATE, t.INTENDDTE) AS FLOWDATE,
                    CAST (
                       CASE
                          WHEN     t.SCHDTYPE = 'V'
                               AND t.SEQ = '001'
                               AND l1.INITEXCHAMT <> 0
                          THEN
                             0
                          WHEN     t.SCHDTYPE = 'V'
                               AND t.SEQ = '002'
                               AND l2.INITEXCHAMT <> 0
                          THEN
                             0
                          WHEN t.SCHDTYPE IN ('M', 'P', 'R') AND t.INTAMT <> 0
                          THEN
                             t.INTAMT
                          WHEN     t.SCHDTYPE IN ('M', 'P', 'R')
                               AND t.INTAMT = 0
                               AND t.IMPLINTAMT = 0
                          THEN
                             flleg.FLOW_AMOUNT
                          WHEN t.SCHDTYPE IN ('M', 'P', 'R') AND t.INTAMT = 0
                          THEN
                             t.IMPLINTAMT
                       END AS NUMBER (18, 2))
                       AS FLOWAMOUNT,
                    CAST (
                       CASE
                          WHEN     t.SCHDTYPE = 'V'
                               AND t.SEQ = '001'
                               AND l1.INITEXCHAMT <> 0
                          THEN
                             0
                          WHEN     t.SCHDTYPE = 'V'
                               AND t.SEQ = '002'
                               AND l2.INITEXCHAMT <> 0
                          THEN
                             0
                          WHEN t.SCHDTYPE IN ('M', 'P', 'R') AND t.INTAMT <> 0
                          THEN
                             t.INTAMT
                          WHEN     t.SCHDTYPE IN ('M', 'P', 'R')
                               AND t.INTAMT = 0
                               AND t.IMPLINTAMT = 0
                          THEN
                             flleg.FLOW_AMOUNT
                          WHEN t.SCHDTYPE IN ('M', 'P', 'R') AND t.INTAMT = 0
                          THEN
                             t.IMPLINTAMT
                       END AS NUMBER (18, 2))
                       AS FLOWAMOUNT_THB,
                    H.DEALDATE,
                    H.MATDATE
               FROM OPICS.SWDS t
                    INNER JOIN (SELECT l.DEALNO,
                                       l.FIXFLOATIND,
                                       l.INITEXCHAMT,
                                       l.FINEXCHAMT,
                                       l.NOTCCYAMT,
                                       l.NXTRATEREV
                                  FROM OPICS.SWDT l
                                 WHERE l.SEQ = '001') l1
                       ON t.DEALNO = l1.DEALNO
                    INNER JOIN (SELECT l.DEALNO,
                                       l.FIXFLOATIND,
                                       l.INITEXCHAMT,
                                       l.FINEXCHAMT,
                                       l.NOTCCYAMT,
                                       l.NXTRATEREV
                                  FROM OPICS.SWDT l
                                 WHERE l.SEQ = '002') l2
                       ON t.DEALNO = l2.DEALNO
                    LEFT JOIN OPICS.SWDH h
                       ON h.DEALNO = t.DEALNO
                    INNER JOIN OPICS.BRPS b
                       ON TRIM (h.BR) = TRIM (b.BR)
                    LEFT JOIN (SELECT DEALNO,
                                      INTAMT AS FLOW_AMOUNT,
                                      INTRATE_8 AS FLOW_RATE,
                                      ROW_NUMBER ()
                                      OVER (PARTITION BY DEALNO
                                            ORDER BY DEALNO, SEQ, SCHDSEQ)
                                         AS ROWNUMBER
                                 FROM OPICS.SWDS
                                WHERE FIXFLOATIND = 'L'
                                      AND SCHDTYPE NOT IN ('V', 'M')) flleg
                       ON     flleg.DEALNO = t.DEALNO
                          AND flleg.ROWNUMBER = 1
                          AND t.FIXFLOATIND = 'L'
              WHERE     h.REVDATE IS NULL
                    AND h.PRODTYPE = 'IR'
                    AND h.VOPER IS NOT NULL
                    AND TO_CHAR (h.MATDATE, 'YYYYMMDD') >
                           TO_CHAR (b.BRANPRCDATE - 7, 'YYYYMMDD')
           ORDER BY t.DEALNO,
                    CAST (t.SEQ AS NUMBER (3)),
                    CAST (t.SCHDSEQ AS NUMBER (3))) data
    WHERE data.FLOWAMOUNT <> '0.00'
   UNION
   SELECT TRIM (DEAL.DEALNO) AS EXT_DEAL_NO,
          1 AS LEG,
          1 AS SEQ,
          CAST (DEAL.CCYRATE_8 AS NUMBER (18, 6)) AS RATE,
          DEAL.VDATE AS FLOW_DATE,
          CAST (DEAL.CCYAMT AS NUMBER (18, 2)) AS FLOW_AMOUNT,
          CASE
             WHEN DEAL.CCY = 'THB' THEN CAST (DEAL.CCYAMT AS NUMBER (18, 2))
             ELSE CAST (DEAL.CCYAMT * re.SPOTRATE_8 AS NUMBER (18, 2))
          END
             AS FLOW_AMOUNT_THB,
          DEAL.DEALDATE,
          DEAL.VDATE AS MATDATE
     FROM OPICS.FXDH deal
          INNER JOIN OPICS.BRPS br
             ON DEAL.BR = BR.BR
          INNER JOIN OPICS.REVP re
             ON deal.CCY = re.CCY
    WHERE DEAL.REVDATE IS NULL AND DEAL.VOPER IS NOT NULL
          AND TO_CHAR (DEAL.VDATE, 'yyyymmdd') >
                 TO_CHAR (BR.BRANPRCDATE - 7, 'yyyymmdd')
   UNION
   SELECT TRIM (DEAL.DEALNO) AS EXT_DEAL_NO,
          0 AS LEG,
          2 AS SEQ,
          NULL AS RATE,
          DEAL.VDATE AS FLOW_DATE,
          CAST (DEAL.CTRAMT AS NUMBER (18, 2)) AS FLOW_AMOUNT,
          CASE
             WHEN DEAL.CTRCCY = 'THB'
             THEN
                CAST (DEAL.CTRAMT AS NUMBER (18, 2))
             ELSE
                CAST (DEAL.CTRAMT * re.SPOTRATE_8 AS NUMBER (18, 2))
          END
             AS FLOW_AMOUNT_THB,
          DEAL.DEALDATE,
          DEAL.VDATE AS MATDATE
     FROM OPICS.FXDH deal
          INNER JOIN OPICS.BRPS br
             ON DEAL.BR = BR.BR
          INNER JOIN OPICS.REVP re
             ON deal.CCY = re.CCY
    WHERE DEAL.REVDATE IS NULL AND DEAL.VOPER IS NOT NULL
          AND TO_CHAR (DEAL.VDATE, 'yyyymmdd') >
                 TO_CHAR (BR.BRANPRCDATE - 7, 'yyyymmdd');


GRANT SELECT ON OPICS.V_DMK_CASHFLOW TO DMKR;
