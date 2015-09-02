<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="SwapEntryInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.SwapEntryInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%= Page.ResolveClientUrl("~/Scripts/autoNumeric.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <form id="form1" runat="server">
        <div id="message-dialog" style="display:none;">
            <label id="message" />
        </div>
        <div class="table-container">
            <table>
                <tr>
                    <td colspan="2">
                        <label>Trade Date:</label>
                    </td>
                    <td colspan="2">
                        <input type="text" name="tradedate" id="tradedate" class="round input-form-textbox"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <label>Instrument :</label>
                    </td>
                    <td colspan="2">
                        <select id="instrument" class="round input-from-dropdownlist"></select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <label>Counterparty :</label>
                    </td>
                    <td colspan="2">
                       <select id="counterparty" name="counterparty"></select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <label>Portfolio :</label>
                    </td>
                    <td colspan="2">
                        <select id="portfolio" class="round input-from-dropdownlist"></select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <label>Value Date :</label>
                    </td>
                    <td colspan="2">
                        <input type="text" name="settledate" id="effdate" class="round input-form-textbox"/>
                    </td>
                </tr>
<%--                <tr>
                    <td colspan="2">
                        <label>Initial Payment Date :</label>
                    </td>
                    <td colspan="2">
                        <input type="text" name="spotdate" id="spotdate" class="round input-form-textbox"/>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2">
                        <label>Maturity Date :</label>
                    </td>
                    <td colspan="2">
                        <input type="text" name="settledate" id="matdate" class="round input-form-textbox"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset>
                        <legend>Payer Leg</legend> 
                             <table>                             
                                <tr>
                                    <td>
                                        <label>Notional :</label>
                                    </td>
                                    <td>
                                        <input type="text" name="notional1" id="notional1" class="round input-form-number"/>
                                        <div id="paynotional" style="display:none;">
                                            Settlement limit is checked on value date.
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <label>Currency :</label>
                                    </td>
                                    <td>
                                        <select id="ccy1" name="ccy1" class="round input-from-dropdownlist">           
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Fixed/Float :</label>
                                    </td>
                                    <td>
                                        <select id="fixedfloat1" class="round input-from-dropdownlist">
                                            <option value="1">Fixed</option>
                                            <option value="0">Float</option>
                                        </select>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                        <label>First Fixing :</label>
                                    </td>
                                    <td>
                                        <input type="text" name="firstfixing1" id="firstfixing1" disabled="disabled" class="round input-form-number"/>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                        <label>Rate/Spread :</label>
                                    </td>
                                    <td>
                                        <input type="text" name="rate1" id="rate1" class="round input-form-number"/>
                                    </td>                                 
                                 </tr>
                                 <tr>
                                     <td>
                                            <label>Frequency :</label>
                                        </td>
                                        <td>
                                            <select id="freq1" class="round input-from-dropdownlist"></select>
                                        </td>
                                 </tr>
                             </table>
                        </fieldset>
                    </td>
                    <td colspan="2">
                        <fieldset>
                        <legend>Receiver Leg</legend> 
                             <table>
                                 <tr>
                                    <td>
                                        <label>Notional :</label>
                                    </td>
                                    <td>
                                        <input type="text" name="notional2" id="notional2" class="round input-form-number"/>
                                        <div id="recnotional" style="display:none;">
                                            Settlement limit is checked on maturity date.
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <label>Currency :</label>
                                    </td>
                                    <td>
                                        <select id="ccy2" name="ccy2" class="round input-from-dropdownlist">           
                                        </select>
                                    </td>
                                </tr>
                                 <tr>    
                                    <td>
                                        <label>Fixed/Float :</label>
                                    </td>
                                    <td>
                                        <select id="fixedfloat2" class="round input-from-dropdownlist">
                                            <option value="1">Fixed</option>
                                            <option value="0">Float</option>
                                        </select>
                                    </td>
                                </tr>
                                 <tr>                  
                                    <td>
                                        <label>First Fixing :</label>
                                    </td>
                                    <td>
                                        <input type="text" name="firstfixing2" id="firstfixing2" disabled="disabled" class="round input-form-number"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Rate/Spread :</label>
                                    </td>
                                    <td>
                                        <input type="text" name="rate2" id="rate2" class="round input-form-number"/>
                                    </td>
                                </tr>
                                 <tr>                   
                                    <td>
                                        <label>Frequency :</label>
                                    </td>
                                    <td>
                                        <select id="freq2" class="round input-from-dropdownlist"></select>
                                    </td>
                                </tr>
                             </table>
                        </fieldset>
                    </td>
                </tr>  
                 <tr id="remarkrow">
                    <td  colspan="2">
                        <label>Comment :</label>
                    </td>
                    <td  colspan="2">
                        <input type="text" name="remark" id="remark" class="round input-form-textbox"/>
                    </td>
                </tr>              
                <tr>
                    <td colspan="2">
                        <input type="button" id="limitcheck" value="Limit Check" onclick="return limitcheck_onclick()" class="round blue button-submit" />
                    </td>
                    <td colspan="2">
                        <% if (Writable) { %><input type="button" id="submit" value="submit" onclick="return submit_onclick()" class="round blue button-submit" /> <% } %>
                    </td>
                </tr>
            </table>
        </div>
        
        <script type="text/javascript">
            var productid = null;
            var trnRecord = null;
            $(document).ready(function () {
                productid = '<%= ProductId %>';
                $('#title').text('Interest Rate Derivatives Deal Entry');

                $('#tradedate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= CurrentProcessDate %>');
                $('#effdate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= GetSpotDateString() %>');
                $('#matdate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= GetSpotDateString() %>');
                $('#notional1,#notional2').autoNumeric('init');
                $('#rate1').autoNumeric('init', { vMin: '-999999999999999.99999999', vMax: '999999999999999.99999999' });
                $('#rate2').autoNumeric('init', { vMin: '-999999999999999.99999999', vMax: '999999999999999.99999999' });
                $('#firstfixing1').autoNumeric('init', { vMin: '-999999999999999.99999999', vMax: '999999999999999.99999999' });
                $('#firstfixing2').autoNumeric('init', { vMin: '-999999999999999.99999999', vMax: '999999999999999.99999999' });

                $('#instrument').change(function () {
                    if ($(this).find("option:selected").text() == 'IRS') {
                        $('#notional2,#ccy2').prop('disabled', true);
                        $('#notional1').autoNumeric('set', '100000000');
                        $("#ccy1 option:contains('THB')").prop('selected', true);
                        $('#fixedfloat1').val("1");
                        $('#fixedfloat2').val("0");
                        $("#firstfixing2").prop('disabled', false);
                        $("#freq1 option:contains('Semi-Annually')").prop('selected', true);
                        $("#freq2 option:contains('Semi-Annually')").prop('selected', true);
                        $('#notional1').autoNumeric('get') > 0 ? $('#notional2').autoNumeric('set', $('#notional1').autoNumeric('get')) : null;
                        $('#ccy2').val($('#ccy1').val());
                        $('#paynotional').hide();
                        $('#recnotional').hide();
                    }
                    else {
                        $('#notional1,#notional2').val('');
                        $('#rate1,#rate2').val('');
                        $('#ccy1,#ccy2').val("");
                        $('#fixedfloat1,#fixedfloat2').val("1");
                        $("#firstfixing1,#firstfixing2").prop('disabled', true);
                        $('#freq1,#freq2').val("-1");
                        $('#notional2,#ccy2').prop('disabled', false);
                        $('#paynotional').show();
                        $('#recnotional').show();
                    }
                });

                $('#notional1,#notional2').keyup(function (event) {
                    if (event.which == 75 || event.which == 107) {
                        $(this).autoNumeric('set', $(this).autoNumeric('get') * 1000);
                    }
                    else if (event.which == 77 || event.which == 109) {
                        $(this).autoNumeric('set', $(this).autoNumeric('get') * 1000000);

                    }
                    return false;
                });
                $('#notional1').change(function () {
                    if ($('#instrument option:selected').text() == 'IRS') {
                        $('#notional2').autoNumeric('set', $('#notional1').autoNumeric('get'))
                    }
                });
                $('#ccy1').change(function () {
                    if ($('#instrument option:selected').text() == 'IRS') {
                        $('#ccy2').val($('#ccy1').val())
                    }
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
                                $('#effdate').val(msg.d.record.EffectDate);
                                $('#matdate').val(msg.d.record.MaturityDate);
                                $('#notional1').autoNumeric('set', msg.d.record.Notional1);
                                $('#notional2').autoNumeric('set', msg.d.record.Notional2);
                                $('#fixedfloat1').val(msg.d.record.FlagFixed1);
                                msg.d.record.FixAmt1 != null ? $('#firstfixing1').autoNumeric('set', msg.d.record.FixAmt1) : null;
                                msg.d.record.Rate1 != null ? $('#rate1').autoNumeric('set', msg.d.record.Rate1) : null;
                                $('#freq1').val(msg.d.record.Feq1);
                                $('#fixedfloat2').val(msg.d.record.FlagFixed2);
                                msg.d.record.FixAmt2 != null ? $('#firstfixing2').autoNumeric('set', msg.d.record.FixAmt2) : null;
                                msg.d.record.Rate2 != null ? $('#rate2').autoNumeric('set', msg.d.record.Rate2) : null;
                                $('#freq2').val(msg.d.record.Feq2);
                                msg.d.record.FlagFixed1 == '0' ? $("#firstfixing1").prop('disabled', false) : $("#firstfixing1").prop('disabled', true);
                                msg.d.record.FlagFixed2 == '0' ? $("#firstfixing2").prop('disabled', false) : $("#firstfixing2").prop('disabled', true);
                                $('#remark').val(msg.d.record.Remark);
                                SetDropdown(msg.d.record.Counterparty, msg.d.record.Portfolio, msg.d.record.Instrument, msg.d.record.Feq1, msg.d.record.Feq2, msg.d.record.CCY1, msg.d.record.CCY2);
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
                    SetDropdown(null, null, null, null, null, null, null);
                    $("#remarkrow").hide();
                }

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

            function GenerateObjects(blnIsSubmit) {
                var pData = "{ "
                        + " strTradeDate : '" + $('#tradedate').val() + "'"
                        + ", strInstrument : '" + $('#instrument').val() + "'"
                        + ", strCtpy : '" + $('#counterparty').combobox('value') + "'"
                        + ", strPortfolio : '" + $('#portfolio').val() + "'"
                        + ", strEffDate : '" + $('#effdate').val() + "'"
                        + ", strMatDate : '" + $('#matdate').val() + "'"
                        + ", strNotional1 : '" + $('#notional1').autoNumeric('get') + "'"
                        + ", strCCY1 : '" + $('#ccy1').val() + "'"
                        + ", strFFL1 : '" + $('#fixedfloat1').val() + "'"
                        + ", strFFix1 : '" + $('#firstfixing1').autoNumeric('get') + "'"
                        + ", strRate1 : '" + $('#rate1').autoNumeric('get') + "'"
                        + ", strFreq1 : '" + $('#freq1').val() + "'"
                        + ", strNotional2 : '" + $('#notional2').autoNumeric('get') + "'"
                        + ", strCCY2 : '" + $('#ccy2').val() + "'"
                        + ", strFFL2 : '" + $('#fixedfloat2').val() + "'"
                        + ", strFFix2 : '" + $('#firstfixing2').autoNumeric('get') + "'"
                        + ", strRate2 : '" + $('#rate2').autoNumeric('get') + "'"
                        + ", strFreq2 : '" + $('#freq2').val() + "'"
                        + ", strOverApprover : '" + $('#over_approver').val() + "'"
                        + ", strOverPCE : '" + $('#over_pce_amount').val() + "'"
                        + ", strOverSCE : '" + $('#over_sce_amount').val() + "'"
                        + ", strComment : '" + $('#over_comment').val() + "'"
                        + ", strRemark : '" + $('#remark').val() + "'"
                        + ", strProductId : " + productid
                        + ", blnIsSubmit : " + blnIsSubmit                 
                        + " }";

                $.ajax({
                    type: "POST",
                    url: '<%= Page.ResolveClientUrl("SwapEntryInfo.aspx/GenerateTrnObject") %>',
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

          /*  function CheckLimits(blnIsSubmit) {
                $.ajax({
                    type: "POST",
                    url: '<%= Page.ResolveClientUrl("SwapEntryInfo.aspx/CheckSwapLimits") %>',
                    data: '{blnIsSubmit : ' + blnIsSubmit + ' }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d.Result == 'OK') {
                            if (blnIsSubmit) {
                                submit_deal('', '');
                            }
                            else {
                                open_limit_dialog(false);
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
                            open_limit_dialog(true);
                        }
                    },
                    error: function () {

                    }
                });
            }*/

            function limitcheck_onclick() {
                if (ValidateForm() == true) {
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
                    url: '<%= Page.ResolveClientUrl("SwapEntryInfo.aspx/SubmitDeal") %>',
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
                if (blnNeedApprove)
                    $("#over-limit").show();
                else
                    $("#over-limit").hide();

                $('#limit-preset').jtable('load', { record: pces });
                $('#limit-set').jtable('load', { record: sces });
                $('#limit-country').jtable('load', { record: country });
                $('#limit-preset').show();
                $('#limit-set').show();
                $('#limit-country').show();

                $("#limit-dialog").dialog({
                    title: 'Limit Contribution',
                    height: 450,
                    width: 1024,
                    modal: true
                });

            }
            
            $("#fixedfloat1").change(function () {
                if ($('#fixedfloat1').val() == 1) {
                    $("#firstfixing1").val('');
                    $("#firstfixing1").prop('disabled', true);
                }
                else {
                    $("#firstfixing1").prop('disabled', false);
                }
            });

            $("#fixedfloat2").change(function () {
                if ($('#fixedfloat2').val() == 1) {
                    $("#firstfixing2").val('');
                    $("#firstfixing2").prop('disabled', true);
                }
                else {
                    $("#firstfixing2").prop('disabled', false);
                }
            });
            function SetDropdown(counterpartyVal, portfolioVal, instrumentVal, freq1Val, freq2Val, ccy1Val, ccy2Val) {
                set_options('counterparty', null, 'GetCounterpartyOptions', function () {
                    $('#counterparty').combobox('value', counterpartyVal);
                });
                set_options('instrument', 'Please select', 'GetSwapInstrumentOptions', instrumentVal != null ? function () {
                    $('#instrument').val(instrumentVal);
                    if ($('#instrument option:selected').text() == 'IRS') {
                        $('#notional2,#ccy2').prop('disabled', true);
                    }
                } : null);
                set_options('portfolio', 'Please select', 'GetPortfolioOptions', portfolioVal != null ? function () {
                    $('#portfolio').val(portfolioVal);
                } : null);
                set_options('freq1', '', 'GetFrequencyOptions', freq1Val != null ? function () {
                    $('#freq1').val(freq1Val);
                } : null);
                set_options('freq2', '', 'GetFrequencyOptions', freq2Val != null ? function () {
                    $('#freq2').val(freq2Val);
                } : null);
                set_options('ccy1', null, 'GetCurrencyOptions', ccy1Val != null ? function () {
                    $('#ccy1').val(ccy1Val);
                } : null);
                set_options('ccy2', null, 'GetCurrencyOptions', ccy2Val != null ? function () {
                    $('#ccy2').val(ccy2Val);
                } : null);
            }

            function ValidateForm() {
                var mess = '';
                mess = $("#tradedate").val() == '' ? 'Please input trade date.' : mess;
                mess = isValidDate($("#tradedate").val()) == false && mess == '' ? 'Invalid trade date.' : mess;
                mess = $("#instrument").val() == '-1' ? 'Please select instrument.' : mess;
                mess = ($('#counterparty').combobox('value') == null || $('#counterparty').combobox('value') == '') && mess == '' ? 'Please input counterparty.' : mess;
                mess = $("#portfolio").val() == '-1' && mess == '' ? 'Please select portfolio.' : mess;
               
                mess = $("#effdate").val() == '' ? 'Please input value date.' : mess;
                mess = isValidDate($("#effdate").val()) == false && mess == '' ? 'Invalid value date.' : mess;

                mess = $("#matdate").val() == '' ? 'Please input maturity date.' : mess;
                mess = isValidDate($("#matdate").val()) == false && mess == '' ? 'Invalid maturity date.' : mess;
                mess = StrToDate($("#effdate").val()) >= StrToDate($("#matdate").val()) && mess == '' ? "Maturity date must be after value date." : mess;
                mess = StrToDate($("#tradedate").val()) >= StrToDate($("#matdate").val()) && mess == '' ? "Maturity date must be after trade date." : mess;
                
                mess = $("#instrument").val() == '-1' ? 'Please select instrument.' : mess;
                mess = $("#notional1").autoNumeric('get') == '' && mess == '' ? "Please input payer leg's notional amount." : mess;
                mess = $("#notional1").autoNumeric('get') <= 0 && mess == '' ? "Invalid payer leg's notional amount." : mess;
                mess = $("#notional2").autoNumeric('get') == '' && mess == '' ? "Please input receiver leg's notional amount." : mess;
                mess = $("#notional2").autoNumeric('get') <= 0 && mess == '' ? "Invalid receiver leg's notional amount." : mess;
                mess = ($("#ccy1").val() == null || $("#ccy1").val() == '') && mess == '' ? "Please select payer leg's currency." : mess;
                mess = ($("#ccy2").val() == null || $("#ccy2").val() == '') && mess == '' ? "Please select receiver leg's currency." : mess;
                mess = $("#ccy1").val() == $("#ccy2").val() && $('#instrument option:selected').text() == 'CCS' && mess == '' ? "Payer leg's and receiver leg's currency must be different." : mess;
                mess = $("#rate1").val() == '' && mess == '' ? "Please input payer leg's Rate/Spread." : mess;
                mess = $("#rate2").val() == '' && mess == '' ? "Please input receiver leg's Rate/Spread." : mess;
                mess = $("#freq1").val() == '-1' && mess == '' ? "Please select payer leg's frequency." : mess;
                mess = $("#freq2").val() == '-1' && mess == '' ? "Please select receiver leg's frequency." : mess;
                mess = $("#firstfixing1").val() == '' && $("#fixedfloat1").val() == '0' && mess == '' ? "Please input payer leg's first fixing." : mess;
                mess = $("#firstfixing2").val() == '' && $("#fixedfloat2").val() == '0' && mess == '' ? "Please input receiver leg's first fixing." : mess;
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
                return new Date(p[2], p[1] - 1, p[0]);
            }

        </script>
    </form>
</asp:Content>
