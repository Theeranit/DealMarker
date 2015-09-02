CREATE OR REPLACE PROCEDURE OPICINF.KKB_TRO_DMK1_1_NEXTBIZDAY(
               p_date  in char
                --ref_rpt_cur      OUT SYS_REFCURSOR)
)
IS
/******************************************************************************
   NAME:       KKB_TRO_DMK_NEXTBIZDAY
   PURPOSE:    

   REVISIONS:
   Ver        Date        Author           Description
   ---------  ----------  ---------------  ------------------------------------
   1.0        7/12/2013   Developer       1. Created this procedure.
   1.1        8/30/2013   Theeranit.W       Add new parameter for call this procedure and return current, next business and previous date.
   1.2        10/04/2013   Theeranit.W       Fixed for SSIS Package by insert into temporary table which name is KKB_TB_PROCESS_DATE
******************************************************************************/
        P_ERRNO NUMBER;
        P_BASECCY VARCHAR2(32767);
        P_DEALCCY VARCHAR2(32767);
        P_HOLTYPE VARCHAR2(32767);
        P_SETDAYS VARCHAR2(32767);
        P_BRPROCDATE DATE;
        P_SETDATE DATE;
        P_CURRDATE DATE;
        
BEGIN
        
        --P_ERRNO := NULL;
        P_BASECCY := 'THB';
        P_DEALCCY := 'THB';
        P_HOLTYPE := '1';
        P_SETDAYS := '1';
        --P_BRPROCDATE := TO_DATE('01/25/2013','mm/dd/yyyy');
        --Default
        SELECT BRANPRCDATE INTO P_CURRDATE FROM OPICS.BRPS WHERE BR='01' AND ROWNUM=1 ORDER BY BRANPRCDATE DESC;
        
        --DBMS_OUTPUT.PUT_LINE('P_DATE=' ||p_date || ' NVL ' || NVL(p_date,''));
        
        IF p_date is null THEN
            P_BRPROCDATE := P_CURRDATE;
        ELSE
            P_BRPROCDATE := TO_DATE(p_date, 'mm/dd/yyyy');
        END IF;
            
        --P_BRPROCDATE := CASE WHEN p_date is null THEN TO_DATE(p_date, 'mm/dd/yyyy') ELSE P_CURRDATE  END;
        --P_BRPROCDATE := TO_DATE(p_date, 'mm/dd/yyyy');
        P_SETDATE := NULL;
        OPICS.SP_GET_NEXTBUSDAY ( P_ERRNO, P_BASECCY, P_DEALCCY, P_HOLTYPE, P_SETDAYS, P_BRPROCDATE, P_SETDATE );
       
       --DBMS_OUTPUT.PUT_LINE('P_CURDATE=' || TO_CHAR(P_BRPROCDATE, 'mm/dd/yyyy') ||  ' and P_SETDATE=' || TO_CHAR(P_SETDATE, 'mm/dd/yyyy')||  ' and P_PREVDATE=' || TO_CHAR(P_BRPROCDATE-1, 'mm/dd/yyyy'));
       
       --OPEN ref_rpt_cur FOR
       DELETE FROM OPICINF.KKB_TB_PROCESS_DATE;
       
       INSERT INTO OPICINF.KKB_TB_PROCESS_DATE (PROC_DATE, NEXT_PROC_DATE, PREV_PROC_DATE, MODIFYDATE, MODIFYBY)
       SELECT P_BRPROCDATE AS PROC_DATE,  P_SETDATE AS NEXT_PROC_DATE, P_BRPROCDATE-1 AS PREV_PROC_DATE, SYSDATE, 'SYSTEM'  
       FROM DUAL;
  
END;
/

CREATE OR REPLACE PROCEDURE OPICINF.KKB_TRO_DMK1_1_OS(
            p_date  in char,
            ref_rpt_cur      OUT SYS_REFCURSOR)

IS
/******************************************************************************
   NAME:       KKB_TRO_DMK_OS
   PURPOSE:    

   REVISIONS:
   Ver        Date        Author           Description
   ---------  ----------  ---------------  ------------------------------------
   1.0        7/12/2013   Developer           1. Created this procedure.
   2.0       7/26/2013    Developer           1. Check FO verify flag for both FI and SWAP products
   2.1       8/21/2013    Theeranit.W       Add process date as parameter
   3.0       9/13/2013    Ekachai V.           1. Add FX Product

******************************************************************************/
BEGIN
        OPEN ref_rpt_cur FOR
        
        SELECT TRIM(h.dealno)       AS EXT_DEAL_NO,        
               TRIM(h.cno)            AS CPTY,
               'SWAP'                    AS PRODUCT,
               h.dealdate              AS TRADE_DATE,
               h.startdate             AS START_DATE,
               h.matdate               AS MATURITY_DATE,
               NULL                    AS    FLAG_NEARFAR,
               NULL                    AS BUY_SELL,
               (CASE WHEN h.prodtype = 'IR' THEN 'IRS' ELSE 'CCS' END) AS INSTRUMENT,
               CAST( ABS(l1.notccyamt) AS NUMBER(18,2))  AS NOTIONAL1, 
               TRIM(l1.notccy)                           AS CCY1,                   
               TRIM(l1.payrecind)                        AS PAY_REC1,
               (CASE WHEN l1.fixfloatind = 'X' THEN 'FIXED' ELSE 'FLOAT' END)   AS FIXED_FLOAT1, 
               TRIM(l1.intpaycycle)                      AS FREQ1,
               (CASE WHEN l1.fixfloatind = 'X' THEN  NULL  ELSE
                  CAST(l1.intrate_8 + l1.spread_8 AS NUMBER(18,6))  END)     AS FIRST_FIXING1,
               (CASE WHEN l1.fixfloatind = 'X' THEN  CAST(l1.intrate_8 + l1.spread_8 AS NUMBER(18,6)) ELSE
                 CAST(0 AS NUMBER(18,6))  END)       AS RATE1,
               NULL                        AS SWAP_POINT1,
               CAST( ABS(l2.notccyamt) AS NUMBER(18,2))  AS NOTIONAL2,
               TRIM(l2.notccy)                           AS CCY2,
               TRIM(l2.payrecind)                        AS PAY_REC2,
               (CASE WHEN l2.fixfloatind = 'X' THEN 'FIXED' ELSE 'FLOAT' END)   AS FIXED_FLOAT2, 
               TRIM(l2.intpaycycle)                  AS FREQ2, 
               (CASE WHEN l2.fixfloatind = 'X' THEN  NULL  ELSE
                  CAST (l2.intrate_8 + l2.spread_8 AS NUMBER(18,6))  END)     AS FIRST_FIXING2,    
               (CASE WHEN l2.fixfloatind = 'X' THEN  CAST(l2.intrate_8 + l2.spread_8 AS NUMBER(18,6)) ELSE
                 CAST(0 AS NUMBER(18,6))  END)      AS RATE2,
               NULL                        AS SWAP_POINT2,
               TRIM(h.trad)                         AS INSERT_BY_EXT,
               h.BRPRCINDTE                         AS INSERT_DATE,
               h.port                               AS EXT_PORTFOLIO,
               CASE WHEN h.plmethod = 'H' THEN 'BANKING' ELSE 'TRADING' END AS PORTFOLIO,
               CASE 
                    WHEN INSTR(h.DEALTEXT,'/DMK') = 0 THEN ''
                    ELSE SUBSTR(h.DEALTEXT, INSTR(h.DEALTEXT,'/DMK') + 4, 11)
               END AS INT_DEAL_NO,
               C.CMNE AS SNAME,
               NULL                                 AS SPOT_DATE
        FROM opics.swdh h, 
               (SELECT l.* FROM opics.swdt l WHERE l.seq = '001' ) l1,
               (SELECT l.* FROM opics.swdt l WHERE l.seq = '002' ) l2,
               opics.cust c,
               opics.brps b
        WHERE ( (h.dealno = l1.dealno) AND (h.dealno = l2.dealno) )
                AND h.revdate IS NULL AND h.prodtype = 'IR'
                AND H.VOPER IS NOT NULL
                AND TRIM(h.br) = TRIM(b.br) 
                AND h.cno = c.cno
                AND TO_CHAR(h.dealdate,'yyyymmdd') <= CASE WHEN p_date IS NOT NULL THEN TO_CHAR(TO_DATE(p_date, 'mm/dd/yyyy'), 'yyyymmdd') ELSE TO_CHAR(b.branprcdate,'yyyymmdd')  END  
                AND TO_CHAR(h.matdate,'yyyymmdd') > CASE WHEN p_date IS NOT NULL THEN TO_CHAR(TO_DATE(p_date, 'mm/dd/yyyy'), 'yyyymmdd') ELSE TO_CHAR(b.branprcdate,'yyyymmdd')  END
        UNION        
        SELECT TRIM(FI.DEALNO)                AS EXT_DEAL_NO,
               TRIM(FI.CNO)                    AS CPTY,
               'BOND'                            AS PRODUCT,
               FI.DEALDATE                     AS TRADE_DATE,
               NULL                            AS START_DATE,
               FI.SETTDATE                     AS MATURITY_DATE,
               NULL                            AS    FLAG_NEARFAR,
               CASE
                 WHEN INSTR (FI.DEALTEXT, 'REDEMPTION') > 0 THEN 'MATURED'
                 ELSE CASE WHEN FI.PS = 'P' THEN 'B' ELSE 'S' END
                END                            AS BUY_SELL,
                TRIM(FI.SECID)               AS INSTRUMENT,
                CAST(ABS(FI.PROCEEDAMT)  AS NUMBER (18,2))   AS NOTIONAL1,
                TRIM(FI.CCY)                 AS CCY1,
                NULL                        AS PAY_REC1,
                NULL                        AS FIXED_FLOAT1,
                NULL                        AS FREQ1,
                NULL                        AS FIRST_FIXING1,
                NULL                        AS RATE1,
                NULL                        AS SWAP_POINT1,
                NULL                           AS NOTIONAL2,
                NULL                         AS CCY2,
                NULL                        AS PAY_REC2,
                NULL                        AS FIXED_FLOAT2,
                NULL                        AS FREQ2,
                NULL                        AS FIRST_FIXING2,
                NULL                        AS RATE2,
                NULL                        AS SWAP_POINT2,
                TRIM(FI.TRAD)                AS INSERT_BY_EXT,
                FI.BRPRCINDTE                 AS INSERT_DATE,
                FI.PORT                     AS EXT_PORTFOLIO,
                CASE
                     WHEN FI.INVTYPE = 'T' THEN 'TRADING'
                    ELSE 'BANKING'
                END                            AS PORTFOLIO,
                CASE 
                    WHEN INSTR(FI.DEALTEXT,'/DMK') = 0 THEN ''
                    ELSE SUBSTR(FI.DEALTEXT, INSTR(FI.DEALTEXT,'/DMK') + 4, 11)
                END AS INT_DEAL_NO,
                C.CMNE AS SNAME,
                NULL                                        AS SPOT_DATE
        FROM OPICS.SPSH FI
        JOIN OPICS.CUST C ON C.CNO = FI.CNO
        LEFT JOIN OPICS.SECM SE
               ON FI.SECID = SE.SECID
        LEFT JOIN OPICS.BRPS BR
               ON FI.BR = BR.BR
        WHERE FI.REVDATE IS NULL AND FI.PRODTYPE LIKE 'FI%' 
          AND TO_CHAR (FI.DEALDATE, 'yyyymmdd') <= CASE WHEN p_date IS NOT NULL THEN TO_CHAR(TO_DATE(p_date, 'mm/dd/yyyy'), 'yyyymmdd')  ELSE TO_CHAR (BR.BRANPRCDATE, 'yyyymmdd')  END 
          AND TO_CHAR (FI.SETTDATE, 'yyyymmdd') > CASE WHEN p_date IS NOT NULL THEN TO_CHAR(TO_DATE(p_date, 'mm/dd/yyyy'), 'yyyymmdd')  ELSE TO_CHAR (BR.BRANPRCDATE, 'yyyymmdd')  END 
          AND FI.VOPER IS NOT NULL
        UNION
        SELECT     TRIM(DEAL.DEALNO)                AS    EXT_DEAL_NO,
                TRIM(DEAL.CUST)                    AS    CPTY,
                CASE
                    WHEN DEAL.FARNEARIND IS NOT NULL THEN 'FX SWAP'
                    WHEN DEAL.SPOTFWDIND = 'F' THEN 'FX FORWARD'
                    WHEN DEAL.SPOTFWDIND = 'S' THEN 'FX SPOT'
                END                                AS    PRODUCT,
                DEAL.DEALDATE                    AS    TRADE_DATE,
                CASE
                    WHEN DEAL.SPOTFWDIND = 'S' THEN NULL
                    ELSE    DEAL.SPOTDATE                        
                END                                     AS START_DATE,
                DEAL.VDATE                        AS    MATURITY_DATE,
                DEAL.FARNEARIND                    AS    FLAG_NEARFAR,
                CASE
                    WHEN DEAL.PS = 'P' THEN 'B'
                    ELSE 'S'
                END                                AS    BUY_SELL,
                DEAL.CCY || '/' || DEAL.CTRCCY                   AS INSTRUMENT,
                CAST(DEAL.CCYAMT  AS NUMBER (18,2))   AS NOTIONAL1,
                DEAL.CCY                        AS    CCY1,
                CASE
                    WHEN DEAL.PS = 'P' THEN 'R'
                    ELSE 'P'
                END                                AS    PAY_REC1,
                NULL                            AS    FIXED_FLOAT1,
                NULL                            AS    FREQ1,
                NULL                            AS    FIRST_FIXING1,
                CAST(DEAL.CCYRATE_8 AS NUMBER (18,6))    AS    RATE1,
                CAST(DEAL.CCYPD_8 AS NUMBER(18,6))        AS    SWAP_POINT1,
                CAST(DEAL.CTRAMT AS NUMBER(18,2))        AS    NOTIONAL2,
                DEAL.CTRCCY                        AS    CCY2,
                CASE
                    WHEN DEAL.PS = 'P' THEN 'P'
                    ELSE 'R'
                END                                AS    PAY_REC2,
                NULL                            AS    FIXED_FLOAT2,
                NULL                            AS    FREQ2,
                NULL                            AS    FIRST_FIXING2,
                NULL                            AS    RATE2,
                NULL                            AS    SWAP_POINT2,
                TRIM(DEAL.TRAD)                    AS    INSERT_BY_EXT,
                DEAL.BRPRCINDTE                 AS    INSERT_DATE,
                DEAL.PORT                        AS    EXT_PORTFOLIO,
                CASE
                    WHEN DEAL.COST = 800 THEN 'TRADING'
                    ELSE 'BANKING'
                END                            AS PORTFOLIO,
                CASE
                    WHEN NEARDEAL.NEAR_DMK_NO IS NOT NULL THEN NEARDEAL.NEAR_DMK_NO
                    WHEN INSTR(DEAL.DEALTEXT,'/DMK') = 0 THEN ''
                    ELSE SUBSTR(DEAL.DEALTEXT, INSTR(DEAL.DEALTEXT,'/DMK') + 4, 11)
                END                                AS    INT_DEAL_NO,
                C.CMNE AS SNAME,
                DEAL.SPOTDATE          AS SPOT_DATE
        FROM OPICS.FXDH deal
            INNER JOIN OPICS.BRPS br
                ON  DEAL.BR = BR.BR
            INNER JOIN OPICS.CUST C ON C.CNO = deal.CUST
            LEFT JOIN (SELECT SWAPDEAL
                                , CASE 
                                    WHEN INSTR(DEALTEXT,'/DMK') = 0 THEN ''
                                    ELSE SUBSTR(DEALTEXT, INSTR(DEALTEXT,'/DMK') + 4, 11)
                                END AS NEAR_DMK_NO
                       FROM OPICS.FXDH
                       WHERE FARNEARIND = 'N') NEARDEAL
                 ON DEAL.DEALNO = NEARDEAL.SWAPDEAL
        WHERE DEAL.REVDATE IS NULL AND DEAL.VOPER IS NOT NULL
                    AND TO_CHAR(DEAL.DEALDATE, 'YYYYMMDD') <= CASE WHEN p_date IS NOT NULL THEN TO_CHAR(TO_DATE(p_date, 'mm/dd/yyyy'), 'yyyymmdd')  ELSE TO_CHAR (BR.BRANPRCDATE, 'yyyymmdd')  END
                    AND TO_CHAR (DEAL.VDATE, 'yyyymmdd') > CASE WHEN p_date IS NOT NULL THEN TO_CHAR(TO_DATE(p_date, 'mm/dd/yyyy'), 'yyyymmdd')  ELSE TO_CHAR (BR.BRANPRCDATE, 'yyyymmdd')  END
        ORDER BY EXT_DEAL_NO;
          
END;
/
