<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="ReconcileInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.ReconcileInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Process Date:
                    <input type="text" name="processdate" id="processdate" class="validate[required] round input-form-textbox"/></label>
                    <input type="button" id="LoadRecordsButton" value="Refresh Deals" class="round blue button-submit" /> &nbsp; 
                    <label id='unavailablemess' style="color:Red"></label>
                    <%--<input type="button" id="LoadOpicDealRecordsButton" value="Refresh OPICS Deals" class="round blue button-submit" />--%>
                    
                </form>
            </div>
        </div>
    </div>
    <div class="table-container">
        <table id="tbReconcile" border="0">
            <tbody>
                <tr>
                    <td id="tdDealToday" width="65%" valign="top">
                        <div id="div1" style="overflow-x:scroll;">
                        <table>
                        <tr>
                            <td>
                                <div id="TableDealTodayContainer">
                                </div>
                                <%--<br />
                                <div id="TableDealOpicsContainer" >
                                </div>--%>
                            </td>
                        </tr>                           
                        </table>
                        </div><br />
                        <div id="div2" style="overflow-x:scroll">
                        <table>
                            <tr>
                                <td>
                                    <div id="TableDealOpicsContainer" >
                                    </div>
                                </td>
                            </tr>                           
                        </table>
                        </div>
                    </td>
                    <td width="3%" align="center" valign="middle">
                        <input type="button" id="MatchDealButton" value=">>" class="round blue button-submit" title="Match deal" />
                        <p></p>
                        <input type="button" id="RemoveDealButton" value="<<" class="round blue button-submit" title="Cancel or remove deal" />
                    </td>
                    <td width="32%"  valign="top" >
                        <div id="TableDealMatchedContainer">
                        </div>
                        <ul>
                            <li>Step 1. Auto matched first</li>
                            <li>Step 2. Select your deal between Deal Maker deals(as Deal as of Today area) and OPICS deals(Opics Deal area) as one to one and then Click <b>Match</b> button.</li>
                            <li>Step 3. Optional: If cancel deal, please select deal from match area and then Click <b>Remove</b> button.</li>
                        </ul>
                    </td>
                </tr>
               
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="3">
                        <!-- visible Value Area -->
                        <input type="hidden" id="hdDMKID" name="hdDMKID" />
                        <input type="hidden" id="hdDMKNo" name="hdDMKNo" />
                        <input type="hidden" id="hdOPICNo" name="hdOPICNo" />
                        <input type="hidden" id="hdDMKMatchID" name="hdDMKMatchID" />
                    </td>
                </tr>
            </tfoot>
        </table>
        
    </div>  
    <div id="message-dialog" style="display:none;">
        <label id="message" />
    </div>
   <script type="text/javascript">

       $(document).ready(function () {
           //$("#processdate").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());
           $("#processdate").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", "<%=CurrentProcessDate %>");
           $("#processdate").attr('disabled', true);
           $("#div1,#div2").css('width', $("div.content-module").width() * .65);
           //validate input
           //$("#Form1").validationEngine();

           //Prepare jtable plugin
           $('#TableDealTodayContainer').jtable({
               title: 'Deal as of today',
               paging: true,
               pageSize: 10,
               sorting: true,
               selecting: true,
               selectingCheckboxes: true,
               defaultSorting: 'Product ASC',
               actions: {
                   listAction: '<%= Page.ResolveClientUrl("ReconcileInfo.aspx/GetDealToday") %>'
               },
               fields: {
                   ID: {
                       key: true,
                       create: false,
                       edit: false,
                       list: false
                   },
                   Product: {
                       title: 'Product'
                   },
                   DMK_NO: {
                       title: 'DMK No.'
                   },
                   OPICS_NO: {
                       title: 'OPICS NO'
                   },
                   Instrument: {
                       title: 'Instrument'
                   },
                   Counterparty: {
                       title: 'CTPY'
                   },
                   TradeDate: {
                       title: 'Trade Date',
                       type: 'date',
                       displayFormat: 'dd-M-yy'
                   },
                   EffectiveDate: {
                       title: 'Start Date',
                       type: 'date',
                       displayFormat: 'dd-M-yy',
                       visibility: 'visible'
                   },
                   MaturityDate: {
                       title: 'Maturity Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                   },
                   Portfolio: {
                       title: 'Portfolio'
                   }, /*
                   EntryDate: {
                       title: 'Entry Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                        , visibility: 'visible'
                   },*/
                   BuySell: {
                       title: 'B/S'
                        , visibility: 'visible'
                   },
                   Notional1: {
                       title: 'Notional1'
                        , visibility: 'visible'
                        , type: 'number'
                        , displayFormat: '2'
                   },
                   CCY1: {
                       title: 'Ccy1'
                        , visibility: 'visible'
                   },
                   PayRec1: {
                       title: 'PayRec1'
                        , visibility: 'visible'
                   },
                   FixedFloat1: {
                       title: 'FixedFloat1'
                        , visibility: 'visible'
                   },
                   Freq1: {
                       title: 'Freq1'
                        , visibility: 'visible'
                   },
                   Fixing1: {
                       title: 'Fixing1'
                        , visibility: 'visible'
                   },
                   Rate1: {
                       title: 'Rate1'
                        , visibility: 'visible'
                   },
                   SwapPoint1: {
                       title: 'SwapPoint1'
                        , visibility: 'visible'
                   },
                   Notional2: {
                       title: 'Notional2'
                        , visibility: 'visible'
                        , type: 'number'
                        , displayFormat: '2'
                   },
                   CCY2: {
                       title: 'Ccy2'
                        , visibility: 'visible'
                   },
                   PayRec2: {
                       title: 'PayRec2'
                        , visibility: 'visible'
                   },
                   FixedFloat2: {
                       title: 'FixedFloat2'
                        , visibility: 'visible'
                   },
                   Freq2: {
                       title: 'Freq2'
                        , visibility: 'visible'
                   },
                   Fixing2: {
                       title: 'Fixing2'
                        , visibility: 'visible'
                   },
                   Rate2: {
                       title: 'Rate2'
                        , visibility: 'visible'
                   },
                   SwapPoint2: {
                       title: 'SwapPoint2'
                        , visibility: 'visible'
                   }, /*
                   Status: {
                       title: 'Status'
                        , visibility: 'visible'
                   },
                   BotContribute: {
                       title: 'BOT'
                         , visibility: 'visible'
                        , type: 'number'
                   },
                   KKContribute: {
                       title: 'KK'
                         , visibility: 'visible'
                        , type: 'number'
                   },*/
                   Remark: {
                       title: 'Remark'
                         , visibility: 'hidden'
                   }
               },
               selectionChanged: function () {
                   //Get all selected rows
                   var $selectedRows = $('#TableDealTodayContainer').jtable('selectedRows');
                   if ($selectedRows.length > 0) {
                       //Show selected rows
                       $selectedRows.each(function () {
                           var record = $(this).data('record');
                           $('#hdDMKID').val(record.ID);
                           $('#hdDMKNo').val(record.DMK_NO);
                       });
                   } else {
                       $('#hdDMKID').val('');
                       $('#hdDMKNo').val('');
                       //No rows selected
                       //alert('No row selected! Select rows to see here...');
                   }
               }
           });

           $('#TableDealOpicsContainer').jtable({
               title: 'Opics Deal',
               paging: true,
               pageSize: 10,
               sorting: true,
               selecting: true,
               selectingCheckboxes: true,
               multiselect: true,
               defaultSorting: 'PRODUCT ASC',
               actions: {
                   listAction: '<%= Page.ResolveClientUrl("ReconcileInfo.aspx/GetDealOpics") %>'
               },
               fields: {
                   ID: {
                       create: false,
                       edit: false,
                       list: false
                   },
                   PRODUCT: {
                       title: 'Product'
                   },
                   INT_DEAL_NO: {
                       title: 'DMK No.'
                   },
                   EXT_DEAL_NO: {
                       key: true,
                       title: 'OPICS No.'
                   },
                   INSTRUMENT: {
                       title: 'Instrument'
                   },
                   SNAME: {
                       title: 'CTPY'
                   },
                   TRADE_DATE: {
                       title: 'Trade Date',
                       type: 'date',
                       displayFormat: 'dd-M-yy'
                   },
                   START_DATE: {
                       title: 'Start Date',
                       type: 'date',
                       displayFormat: 'dd-M-yy',
                       visibility: 'visible'
                   },
                   MATURITY_DATE: {
                       title: 'Maturity Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                   },
                   PORTFOLIO: {
                       title: 'Portfolio'
                   },
                   BUY_SELL: {
                       title: 'B/S'
                        , visibility: 'visible'
                   }, /*
                   INSERT_DATE: {
                       title: 'Entry date',
                       type: 'date',
                       displayFormat: 'dd-M-yy',
                       visibility: 'visible'
                   },
                   EXT_PORTFOLIO: {
                       title: 'External Portfolio',
                       visibility: 'visible'
                   },*/
                   NOTIONAL1: {
                       title: 'Notional1'
                        , visibility: 'visible'
                        , type: 'number'
                        , displayFormat: '2'
                   },
                   CCY1: {
                       title: 'CCY1'
                        , visibility: 'visible'
                   },
                   PAY_REC1: {
                       title: 'PayRec1'
                        , visibility: 'visible'
                   },
                   FIXED_FLOAT1: {
                       title: 'FixedFloat1'
                        , visibility: 'visible'
                   },
                   FREQ1: {
                       title: 'Freq1'
                        , visibility: 'visible'
                   },
                   FIRST_FIXING1: {
                       title: 'First Fixing1'
                        , visibility: 'visible'
                   },
                   RATE1: {
                       title: 'Rate1'
                        , visibility: 'visible'
                   },
                   SWAP_POINT1: {
                       title: 'SwapPoint1'
                        , visibility: 'visible'
                   },
                   NOTIONAL2: {
                       title: 'Notional2'
                        , visibility: 'visible'
                        , type: 'number'
                        , displayFormat: '2'
                   },
                   CCY2: {
                       title: 'CCY2'
                        , visibility: 'visible'
                   },
                   PAY_REC2: {
                       title: 'PayRec2'
                        , visibility: 'visible'
                   },
                   FIXED_FLOAT2: {
                       title: 'FixedFloat2'
                        , visibility: 'visible'
                   },
                   FREQ2: {
                       title: 'Freq2'
                        , visibility: 'visible'
                   },
                   FIRST_FIXING2: {
                       title: 'First Fixing2'
                        , visibility: 'visible'
                   },
                   RATE2: {
                       title: 'Rate2'
                        , visibility: 'visible'
                   },
                   SWAP_POINT2: {
                       title: 'SwapPoint2'
                        , visibility: 'visible'
                   }/*,
                   INSERT_BY_EXT: {
                       title: 'Insert by External'
                        , visibility: 'visible'
                   },
                   CPTY: {
                       title: 'CPTY'
                        , visibility: 'visible'
                   }*/
               },
               selectionChanged: function (event, data) {
                   //Get all selected rows
                   var $selectedRows = $('#TableDealOpicsContainer').jtable('selectedRows');
                   $('#hdOPICNo').val('');
                   if ($selectedRows.length > 0) {
                       //Show selected rows
                       $selectedRows.each(function () {
                           var record = $(this).data('record');
                           if ($('#hdOPICNo').val() != '') $('#hdOPICNo').val($('#hdOPICNo').val() + ",");
                           $('#hdOPICNo').val($('#hdOPICNo').val() + record.EXT_DEAL_NO);
                       });
                   } else {
                       $('#hdOPICNo').val('');
                       //No rows selected
                       //alert('No row selected! Select rows to see here...');
                   }
               }
           });


           $('#TableDealMatchedContainer').jtable({
               title: 'Deal Matched',
               paging: true,
               pageSize: 20,
               sorting: true,
               selecting: true,
               selectingCheckboxes: true,
               defaultSorting: 'Product ASC',
               actions: {
                   listAction: '<%= Page.ResolveClientUrl("ReconcileInfo.aspx/GetDealMatchToday") %>'
               },
               fields: {
                   ID: {
                       key: true,
                       create: false,
                       edit: false,
                       list: false
                   },
                   Product: {
                       title: 'Product',
                       width: '10%'
                   },
                   DMK_NO: {
                       title: 'DMK No.',
                       width: '10%'
                   },
                   OPICS_NO: {
                       title: 'OPICS No.',
                       width: '10%',
                       display: function (data) {
                           return data.record.OPICS_NO.replace(/,/g, "<br>");
                       }
                   },
                   Counterparty: {
                       title: 'CTPY',
                       width: '10%'
                   }
               },
               selectionChanged: function () {
                   //Get all selected rows
                   var $selectedRows = $('#TableDealMatchedContainer').jtable('selectedRows');
                   if ($selectedRows.length > 0) {
                       //Show selected rows
                       $selectedRows.each(function () {
                           var record = $(this).data('record');
                           $('#hdDMKMatchID').val(record.ID);
                       });
                   } else {
                       $('#hdDMKMatchID').val('');
                       //No rows selected
                       //alert('No row selected! Select rows to see here...');
                   }
               }
           });

           //Re-load records when user click 'load records' button.
           $('#LoadRecordsButton').click(function (e) {
               e.preventDefault();
               if (ValidateForm()) {

                   $.ajax({
                       type: "POST",
                       url: '<%= Page.ResolveClientUrl("ReconcileInfo.aspx/CheckFlagReconcile") %>',
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       success: function (msg) {
                           $('#unavailablemess').text('');
                           if (msg.d.Result == 'UNAVAILBLE') {
                               $('#unavailablemess').text(msg.d.Message);
                           }
                           else if (msg.d.Result == 'ERROR') {
                               $("#message").text(msg.d.Message);
                               $("#message-dialog").dialog({
                                   modal: true, title: 'Information',
                                   height: 200,
                                   width: 350,
                                   modal: true
                               });
                           }
                           else {
                               $('#TableDealTodayContainer').jtable('load', {
                                   processdate: $('#processdate').val()
                               }, function () {
                                   $('#TableDealTodayContainer').jtable('deselectRows');
                                   $('#TableDealOpicsContainer').jtable('load', {
                                       processdate: $('#processdate').val()
                                   }, function () {
                                       $('#TableDealOpicsContainer').jtable('deselectRows');
                                       $('#TableDealMatchedContainer').jtable('load', {
                                           processdate: $('#processdate').val()
                                       }, function () {
                                           $('#TableDealMatchedContainer').jtable('deselectRows');
                                       });
                                   });
                               });
                           }

                       },
                       error: function () { }
                   });

               }
               Clear();
           });

           $('#LoadOpicDealRecordsButton').click(function (e) {
               e.preventDefault();
               if (ValidateForm()) {
                   $('#TableDealOpicsContainer').jtable('load', {
                       processdate: $('#processdate').val()
                   });
               }
           });

           $('#MatchDealButton').click(function (e) {
               e.preventDefault();

               //               $('#confimDialog').appendTo('body')
               //                    .html('<div>Are you confirm match deal?</div>')
               //                    .dialog({
               //                        modal: true, title: 'Confirm', zIndex: 10000, autoOpen: true,
               //                        width: 'auto', resizable: false,
               //                        buttons: {
               //                            Yes: function () {
               //                                Match();
               //                                $(this).dialog("close");
               //                            },
               //                            No: function () {
               //                                $(this).dialog("close");
               //                            }
               //                        }
               //                    });

               if (ValidateForm()) Match();
           });

           $('#RemoveDealButton').click(function (e) {
               e.preventDefault();

               if (ValidateForm()) Cancel();
           });

           /*
           var colsDealToday = new Array('Entry Date', 'Product', 'DMK No.', 'OPICS_NO'
           , 'Instrument', 'B/S', 'CTPY'
           , 'Trade Date', 'Portfolio', 'Start Date', 'Maturity Date'
           , 'Notional1', 'PayRec1', 'FixedFloat1'
           , 'Fixing1', 'Rate1', 'SwapPoint1', 'Notional2', 'PayRec2', 'FixedFloat2'
           , 'Fixing2', 'Rate2', 'SwapPoint2', 'Status', 'BOT', 'KK'); // header for columns

           $('#TableDealTodayContainer table.jtable > tbody').on('mouseover', 'tr.jtable-data-row', function (e) {
           var trEle = $(this);
           var tdEle = trEle.children('td:not(:has("img,button,input"))');
           var titleStr = '';
           $.each(tdEle, function (index, ele) {
           titleStr += colsDealToday[index];
           titleStr += ' = ';
           titleStr += $(ele).text();
           titleStr += (index < tdEle.length - 1 ? '<br>' : '');
           });
           trEle.children('td').eq(3).attr('alt', titleStr);
           });

           var colsOPICSDeal = new Array('Entry Date', 'Product', 'DMK No.', 'OPICS_NO'
           , 'Instrument', 'B/S', 'CTPY'
           , 'Trade Date', 'Portfolio', 'External Portfolio'
           , 'Start Date', 'Maturity Date', 'Notional1', 'CCY1', 'PayRec1', 'FixedFloat1', 'Frequency1', 'First Fixing1'
           , 'Rate1', 'SwapPoint1', 'Notional2', 'CCY2', 'PayRec2', 'FixedFloat2', 'Frequency2', 'First Fixing2'
           , 'Rate2', 'SwapPoint2', 'Insert by External', 'CPTY'); // header for columns
           $('#TableDealOpicsContainer table.jtable > tbody').on('mouseover', 'tr.jtable-data-row', function (e) {
           var trEle = $(this);
           var tdEle = trEle.children('td:not(:has("img,button,input"))');
           var titleStr = '';
           $.each(tdEle, function (index, ele) {
           titleStr += colsOPICSDeal[index];
           titleStr += ' = ';
           titleStr += $(ele).text();
           titleStr += (index < tdEle.length - 1 ? '<br>' : '');
           });
           trEle.children('td').eq(3).attr('alt', titleStr);
           });
           */
           $('#TableDealTodayContainer table.jtable > tbody').on('mouseover', 'tr.jtable-data-row', function (e) {
               var trEle = $(this); trEle.children('td').eq(2).attr('alt', trEle.children('td:last').text());
           });

       }).tooltip({
           items: "td",
           content: function () {
               var element = $(this);
               return element.attr("alt");
           },
           position: { my: "right-10 bottom",
               at: "center top", collision: "flipfit"
           },
           track: true
       }).ajaxStart(function () {
          $('#LoadRecordsButton').prop('disabled',true);
       }).ajaxStop(function () {
           $("#LoadRecordsButton").prop('disabled', false);
       });

       function Match() {
           if ($('#hdDMKID').val() == "") {
               alert('Please select DMK deal before match.');
               return false;
           }

           if ($('#hdOPICNo').val() == "") {
               alert('Please select OPICS deal before match.');
               return false;
           }
           
           //do something
           var pData = "{ processdate : '" + $('#processdate').val() + "'"
                        + ", dmkid : '" + $('#hdDMKID').val() + "'"
                        + ", opicsno : '" + $('#hdOPICNo').val() + "'"
                        + " }";

           $.ajax({
               type: "POST",
               url: '<%= Page.ResolveClientUrl("ReconcileInfo.aspx/MatchingDeal") %>',
               data: pData,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (msg) {
                   //alert(msg.d.Message);
                   $("#message").text(msg.d.Message);

                   $("#message-dialog").dialog({
                       modal: true, title: 'Information',
                       height: 200,
                       width: 350,
                       modal: true,
                       buttons: {
                           OK: function () {
                               $('#LoadRecordsButton').click();
                               $(this).dialog("close");
                                Clear(); 
                               
                           }
                       }
                   });

                   
               },
               error: function () { }
           });
       }

       function Cancel() {
           if ($('#hdDMKMatchID').val() == "") {
               alert('Please select match deal before cancel.');
               return false;
           }
           //do something
           var pData = "{ processdate : '" + $('#processdate').val() + "'"
                        + ", dmkid : '" + $('#hdDMKMatchID').val() + "'"
                        + " }";

           $.ajax({
               type: "POST",
               url: '<%= Page.ResolveClientUrl("ReconcileInfo.aspx/CancelDeal") %>',
               data: pData,
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (msg) {
                   $("#message").text(msg.d.Message);

                   $("#message-dialog").dialog({
                       modal: true, title: 'Information',
                       height: 200,
                       width: 350,
                       modal: true,
                       buttons: {
                           OK: function () {
                               $('#LoadRecordsButton').click();
                               $(this).dialog("close");
                                Clear(); 
                           }
                       }
                   });

               },
               error: function () { }
           });
       }

       function ValidateForm() {
//           if ($('#Form1').validationEngine('validate')) {
//               if ($('#processdate').val() == "") {
//                   return false;
//               }
               return true;
//           }
       }

       function Clear() {
           $('#hdDMKMatchID').val('');
           $('#hdDMKID').val('');
           $('#hdOPICNo').val('');
           $('#hdDMKNo').val('');    
       }
  </script>
</asp:Content>
