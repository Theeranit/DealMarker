<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FXSpotEntryInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.FXSpotEntryInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <form id="form1" runat="server">
        <div id="message-dialog" style="display:none;">
            <label id="message" />
        </div>
        <div class="table-container">
            <table>
                <tr>
                    <td>
                        <label>Trade Date:</label>
                    </td>
                    <td>
                        <input type="text" name="tradedate" id="tradedate" readonly="readonly" class="round input-form-textbox"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Spot Date:</label>
                    </td>
                    <td>
                        <input type="text" name="spotdate" id="spotdate" class="round input-form-textbox"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Counterparty :</label>
                    </td>
                    <td>
                        <select id="counterparty" name="counterparty"></select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Portfolio :</label>
                    </td>
                    <td>
                        <select id="portfolio" name="portfolio" class="round input-from-dropdownlist"></select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Currency Pair :</label>
                    </td>
                    <td>
                      <select id="currencypair" name="currencypair" class="round input-from-dropdownlist"></select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Buy/Sell :</label>
                    </td>
                    <td>
                        <select id="buysell" name="buysell" class="round input-from-dropdownlist">
                            <option value="B">Buy</option>
                            <option value="S">Sell</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Contract CCY:</label>
                    </td>
                    <td>
                        <select id="ccy" name="ccy" class="round input-from-dropdownlist">
           
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Spot Rate :</label>
                    </td>
                    <td>
                        <input type="text" id="spotrate" name="spotrate" class="round input-form-number"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Contract Amount :</label>
                    </td>
                    <td>
                        <input type="text" name="contractamt" id="contractamt" class="round input-form-number"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Counter Amount :</label>
                    </td>
                    <td>
                        <input type="text" name="counteramt" id="counteramt" class="round input-form-number"/>
                    </td>
                </tr>
                 <tr id="remarkrow">
                    <td>
                        <label>Comment :</label>
                    </td>
                    <td>
                        <input type="text" name="remark" id="remark" class="round input-form-textbox"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Good Fund Deal :</label>
                    </td>
                    <td>
                        <input type="checkbox" id='settleFlag' name='settleFlag' />                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="button" id="limitcheck" value="Limit Check" onclick="return limitcheck_onclick()" class="round blue button-submit"/>
                    </td>
                    <td>                        
                        <% if (Writable)
                           { %><input type="button" id="submit" value="submit" onclick="return submit_onclick()" class="round blue button-submit"/><% } %>
                    </td>
                </tr>
            </table>
        </div>       
         <script type="text/javascript">
             var productid = null;
             var trnRecord = null;
             $(document).ready(function () {
                 productid = '<%= ProductId %>';
                 $('#title').text('FX Spot Deal Entry');
                 $('#spotdate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= GetSpotDateString() %>');
                 $('#tradedate').val('<%= CurrentProcessDate %>');
                 $('#contractamt').change(function (e) {
                     CalCounterAmt();
                 });
                 $('#contractamt').autoNumeric('init');
                 // window.parent.document.getElementById("Iframe").src = 'Deal/FXForwardEntryInfo.aspx?';
                 $('#ccy').change(function () {
                     $('#counteramt').val("");
                     $('#contractamt').val("");
                 });
                 $('#spotrate').change(function () {
                     CalCounterAmt();
                 })
                .autoNumeric('init', {mDec : 8});
                 $('#counteramt').change(function () {
                     CalContractAmt();
                 })
                 .autoNumeric('init');
                 $('#counteramt').keyup(function (event) {
                     if (event.which == 75 || event.which == 107) {
                         $('#counteramt').autoNumeric('set', $('#counteramt').autoNumeric('get') * 1000);
                     }
                     else if (event.which == 77 || event.which == 109) {
                         $('#counteramt').autoNumeric('set', $('#counteramt').autoNumeric('get') * 1000000);

                     }
                     return false;
                 });
                 $('#contractamt').keyup(function (event) {
                     if (event.which == 75 || event.which == 107) {
                         $('#contractamt').autoNumeric('set', $('#contractamt').autoNumeric('get') * 1000);
                     }
                     else if (event.which == 77 || event.which == 109) {
                         $('#contractamt').autoNumeric('set', $('#contractamt').autoNumeric('get') * 1000000);

                     }
                     return false;
                 });
                 if (productid != 'null') {
                     $.ajax({
                         type: "POST",
                         url: window.location.pathname + "/" + "GetEditByID", //'<%= Page.ResolveClientUrl("FIEntryInfo.aspx/' + strFunction + '" ) %>',
                         data: '{ id : ' + productid + ' }',
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (msg) {
                             if (msg.d.Result == 'OK') {
                                 $('#tradedate').val(msg.d.record.TradeDate);
                                 $('#spotdate').val(msg.d.record.MaturityDate);
                                 $('#buysell').val(msg.d.record.BuySell);
                                 $('#spotrate').autoNumeric('set', msg.d.record.Rate1);
                                 $('#contractamt').autoNumeric('set', msg.d.record.Notional1);
                                 $('#counteramt').autoNumeric('set', msg.d.record.Notional2);
                                 $('#remark').val(msg.d.record.Remark);
                                 $('#settleFlag').prop('checked', !msg.d.record.flag_settle);
                                 SetDropdown(msg.d.record.Counterparty, msg.d.record.Portfolio, msg.d.record.Instrument);
                                 SetContractCCY(msg.d.CCY);
                                 $('#ccy').val(msg.d.record.CCY1);
                             }
                             else if (msg.d.Result == "ERROR") {
                                 $("#message").text(msg.d.Message);

                                 $("#message-dialog").dialog({
                                     title: 'ERROR',
                                     height: 200,
                                     width: 350,
                                     modal: true
                                 });
                             }
                         },
                         error: function () {

                         }
                     });
                     $("#remarkrow").show();
                 }
                 else {
                     SetDropdown(null, null, null);
                     $("#remarkrow").hide();
                 }

                 $('#currencypair').combobox({
                     change: function (evt, ui) {
                         $('#ccy option').remove();
                         if ($(this).combobox('value') != null) {
                             GetInstrument();
                         }
                     }
                 });

                 $('#counterparty').combobox({
                     change: function (evt, ui) {
                         $('#counterparty').combobox('value', $('#counterparty').combobox('value'));

                     }
                 });
             });

             function submit_onclick() {
                 if (ValidateForm() == true) {
                     $('input[id="submit"]').prop('disabled', true);
                     GenerateObjects(true);
                 }
             }

             function GetInstrument() {
                 var predate = '{ id : "' + $('#currencypair option:selected').val() + '" }';
             $.ajax({
                 type: "POST",
                 url: '<%= Page.ResolveClientUrl("FXSpotEntryInfo.aspx/GetInstrument") %>',
                 data: predate,
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     if (msg.d.Result == 'OK') {
                         SetContractCCY(msg.d.CCY);
                         CalCounterAmt();
                     }
                     else if (msg.d.Result == "ERROR") {
                         $("#message").text(msg.d.Message);

                         $("#message-dialog").dialog({
                             title: 'ERROR',
                             height: 200,
                             width: 350,
                             modal: true
                         });
                     }
                 },
                 error: function () {

                 }
             });
             }

             function SetContractCCY(objPCCF) {
                 $('#ccy').append($('<option>', { value: objPCCF.CURRENCYID1 }).text(objPCCF.CURRENCY1).attr('type','1'));
                 $('#ccy').append($('<option>', { value: objPCCF.CURRENCYID2 }).text(objPCCF.CURRENCY2).attr('type','2'));
                 $('#ccy').data('flag',objPCCF.FLAG_MULTIPLY);

             }

             function CalCounterAmt() {
                 var ccyType = $('#ccy option:selected').attr('type');
                 var spotrate = $('#spotrate').autoNumeric('get');
                 var counteramt = $('#counteramt');
                 if ($('#contractamt').autoNumeric('get') == '') {
                     $('#contractamt').autoNumeric('set', 0.00);
                 }
                 var contractamtval = $('#contractamt').autoNumeric('get');
                 var flag_mul = $('#ccy').data('flag');
                 if (ccyType == '1' && flag_mul) {
                     counteramt.autoNumeric('set',contractamtval * spotrate);
                 }
                 else if (ccyType == '2' && flag_mul) {
                     counteramt.autoNumeric('set',contractamtval / spotrate);
                 }
                 else if (ccyType == '1' && !flag_mul) {
                     counteramt.autoNumeric('set',contractamtval / spotrate);
                 }
                 else if (ccyType == '2' && !flag_mul) {
                     counteramt.autoNumeric('set',contractamtval * spotrate);
                 }
             }

             function CalContractAmt() {
                 var ccyType = $('#ccy option:selected').attr('type');
                 var spotrate = $('#spotrate').autoNumeric('get');
                 var contractamt = $('#contractamt');
                 var counteramtval = $('#counteramt').autoNumeric('get');
                 var flag_mul = $('#ccy').data('flag');
                 if (ccyType == '1' && flag_mul) {
                     contractamt.autoNumeric('set',counteramtval / spotrate);
                 }
                 else if (ccyType == '2' && flag_mul) {
                     contractamt.autoNumeric('set', counteramtval * spotrate);
                 }
                 else if (ccyType == '1' && !flag_mul) {
                     contractamt.autoNumeric('set', counteramtval * spotrate);
                 }
                 else if (ccyType == '2' && !flag_mul) {
                     contractamt.autoNumeric('set',counteramtval / spotrate);
                 }
             }

             function GenerateObjects(blnIsSubmit) {
                 var pData = "{ "
                        + " strTradeDate : '" + $('#tradedate').val() + "'"
                        + ", strSpotDate : '" + $('#spotdate').val() + "'"
                        + ", strCtpy : '" + $('#counterparty').combobox('value') + "'"
                        + ", strPortfolio : '" + $('#portfolio').val() + "'"
                        + ", strCurrencyPair : '" + $('#currencypair').combobox('value') + "'"
                        + ", strBS : '" + $('#buysell').val() + "'"
                        + ", strContractCcy : '" + $('#ccy').val() + "'"
                        + ", strCounterCcy : '" + $('#ccy option:not(:selected)').val() + "'"
                        + ", strSpotRate : '" + $('#spotrate').autoNumeric('get') + "'"
                        + ", strContractAmt : '" + $('#contractamt').autoNumeric('get') + "'"
                        + ", strCounterAmt : '" + $('#counteramt').autoNumeric('get') + "'"
                        + ", strRemark : '" + $('#remark').val() + "'"
                        + ", strProductId : " + productid
                        + ", settleFlag : " + $('#settleFlag').prop('checked')
                        + ", blnIsSubmit : " + blnIsSubmit                 
                        + " }";

                 $.ajax({
                     type: "POST",
                     url: '<%= Page.ResolveClientUrl("FXSpotEntryInfo.aspx/GenerateTrnObject") %>',
                     data: pData,
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (msg) {
                         if (msg.d.Result == 'OK') {
                             trnRecord = msg.d.record;
                             if (msg.d.pcesce.Result == 'OK') {
                                 if (blnIsSubmit) {
                                     submit_deal('', '');
                                 }
                                 else {
                                     open_limit_dialog(false, msg.d.pcesce.PCERecords, msg.d.pcesce.SCERecords, msg.d.pcesce.CountryRecords);
                                 }
                             }
                             else if (msg.d.pcesce.Result == "ERROR") {
                                 $("#message").text(msg.d.pcesce.Message);

                                 $("#message-dialog").dialog({
                                     title: 'ERROR',
                                     height: 200,
                                     width: 350,
                                     modal: true
                                 });
                                 $('input[id="submit"]').prop('disabled', false);
                             }
                             else if (msg.d.pcesce.Result == "NEEDAPPROVE") {
                                 $('input[id="submit"]').prop('disabled', false);
                                 open_limit_dialog(true, msg.d.pcesce.PCERecords, msg.d.pcesce.SCERecords, msg.d.pcesce.CountryRecords);
                             }
                             else if (msg.d.pcesce.Result == "NOTALLOW") {
                                 $('input[id="submit"]').prop('disabled', false);
                                 open_limit_dialog(false, msg.d.pcesce.PCERecords, msg.d.pcesce.SCERecords, msg.d.pcesce.CountryRecords);
                             }
                         }
                         else if (msg.d.Result == 'ERROR') {
                             $("#message").text(msg.d.Message);

                             $("#message-dialog").dialog({
                                 title: 'ERROR',
                                 height: 200,
                                 width: 350,
                                 modal: true
                             });

                             $('input[id="submit"]').prop('disabled', false);
                         };
                     },
                     error: function () {
                         $('input[id="submit"]').prop('disabled', false);
                     }
                 });

             }
            /* function CheckLimits(blnIsSubmit) {
                 $.ajax({
                     type: "POST",
                     url: '<%= Page.ResolveClientUrl("FXSpotEntryInfo.aspx/CheckFXSpotLimits") %>',
                     data: "{blnIsSubmit : " + blnIsSubmit + ", record : '" + trnRecord + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (msg) {
                         if (msg.d.Result == 'OK') {
                             if (blnIsSubmit) {
                                 submit_deal('', '');
                             }
                             else {
                                 open_limit_dialog(false, msg.d.PCERecords, msg.d.SCERecords);
                             }
                         }
                         else if (msg.d.Result == "ERROR") {
                             $("#message").text(msg.d.Message);

                             $("#message-dialog").dialog({
                                 title: 'ERROR',
                                 height: 200,
                                 width: 350,
                                 modal: true
                             });
                         }
                         else if (msg.d.Result == "NEEDAPPROVE") {
                             open_limit_dialog(true, msg.d.PCERecords, msg.d.SCERecords);
                         }
                     },
                     error: function () {

                     }
                 });
             }*/

             function limitcheck_onclick() {
                 if (ValidateForm()==true) {
                     GenerateObjects(false);
                 }
             }

             function submit_deal(strOverApprover, strOverComment) {

                 var pData = "{ strOverApprover : '" + strOverApprover + "'"
                        + ", strOverComment : '" + strOverComment + "'"
                        + ", record : '" + trnRecord + "'"
                        + ", strProductId : " + productid
                        + " }";

                 $.ajax({
                     type: "POST",
                     url: '<%= Page.ResolveClientUrl("FXSpotEntryInfo.aspx/SubmitDeal") %>',
                     data: pData,
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (msg) {
                         if (msg.d.Result == "OK") {
                             $("#message").text("/DMK" + msg.d.Message);
                             $("#message-dialog").dialog({
                                 title: "Deal " + msg.d.Message + " is booked.",
                                 height: 200,
                                 width: 350,
                                 modal: true,
                                 close: function (event, ui) { window.parent.document.getElementById("Iframe").src = window.location.pathname; }
                             });

                         }
                         else {
                             $("#message").text(msg.d.Message);
                             $("#message-dialog").dialog({
                                 title: msg.d.Result,
                                 height: 200,
                                 width: 350,
                                 modal: true
                             });
                         }
                         $('input[id="submit"]').prop('disabled', false);
                     },
                     error: function () {
                         $('input[id="submit"]').prop('disabled', false);
                     }
                 });

             }

             function open_limit_dialog(blnNeedApprove, pces, sces, country) {
                 var intHeight;

                 if (blnNeedApprove) {
                     $("#over-limit").show();
                     intHeight = 475;
                 }
                 else {
                     $("#over-limit").hide();
                     intHeight = 450;
                 }

                 $('#limit-preset').jtable('load', { record: pces });
                 $('#limit-preset').show();

                 if ($('#settleFlag').prop('checked') == false) {
                     $('#limit-set').jtable('load', { record: sces });
                     $('#limit-set').show();
                 }
                 else {
                     $('#limit-set').hide();
                 }

                 $('#limit-country').jtable('load', { record: country });
                 $('#limit-country').show();

                 $("#limit-dialog").dialog({
                     title: 'Limit Contribution',
                     height: intHeight,
                     width: 1200,
                     modal: true
                 });
             }

             function ValidateForm() {
                 var mess = '';
                 mess = $("#tradedate").val() == '' ? 'Please input trade date.' : mess;
                 mess = $("#spotdate").val() == '' && mess == '' ? 'Please input spot date.' : mess;
                 mess = isValidDate($("#spotdate").val()) == false && mess == '' ? 'Invalid spot date.' : mess;
                 mess = StrToDate($("#tradedate").val()) > StrToDate($("#spotdate").val()) && mess == '' ? 'Spot date cannot be before trade date.' : mess;
                 mess = ($('#counterparty').combobox('value') == null || $('#counterparty').combobox('value') == '') && mess == '' ? 'Please input counterparty.' : mess;
                 mess = $("#portfolio").val() == '-1' && mess == '' ? 'Please select portfolio.' : mess;
                 mess = ($('#currencypair').combobox('value') == null || $('#currencypair').combobox('value') == '') && mess == '' ? 'Please input currency pair.' : mess;
                 mess = ($("#ccy").val() == null || $("#ccy").val() == '') && mess == '' ? 'Please select contract ccy:.' : mess;
                 mess = $("#spotrate").autoNumeric('get') == '' && mess == '' ? 'Please input spot rate.' : mess;
                 mess = $("#contractamt").autoNumeric('get') == '' && mess == '' ? 'Please input contract amount.' : mess;
                 mess = $("#contractamt").autoNumeric('get') <= 0 && mess == '' ? 'Invalid contract amount.' : mess;
                 mess = $("#counteramt").autoNumeric('get') == '' && mess == '' ? 'Please input counter amount.' : mess;
                 mess = $("#counteramt").autoNumeric('get') <= 0 && mess == '' ? 'Invalid counter amount.' : mess;
                 mess = $("#remark").val() == '' && mess == '' && productid != 'null' ? 'Please input comment.' : mess;
                 if (mess != '') {
                     $("#message").text(mess);

                     $("#message-dialog").dialog({
                         title: 'ERROR',
                         height: 200,
                         width: 350,
                         modal: true
                     });

                     return false;
                 }
                 else {
                     return true;
                 }

            }

           function isValidDate(date) {
               var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
               return date_regex.test(date);             
            }

            function StrToDate(date) {               
                var p = date.split("/");
                return new Date(p[2], p[1]-1, p[0]);
            }

            function SetDropdown(counterpartyVal, portfolioVal, currencypairVal) {
                set_options('counterparty', null, 'GetCounterpartyOptions', function () {
                        $('#counterparty').combobox('value', counterpartyVal);
                });                 
                set_options('portfolio', 'Please select', 'GetPortfolioOptions', portfolioVal != null ? function () {
                        $('#portfolio').val(portfolioVal);
                } : null);
                set_options('currencypair', null, 'GetFXSpotInstrumentOptions',function () {
                    $('#currencypair').combobox('value', currencypairVal);
                });
            }
    </script>
    </form>
</asp:Content>
