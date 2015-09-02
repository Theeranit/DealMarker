<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FXSwapEntryInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.FXSwapEntryInfo" %>
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
                    <td colspan="2">
                        <label>Trade Date:</label>
                    </td>
                    <td colspan="2">
                        <input type="text" name="tradedate" id="tradedate" readonly="readonly" class="round input-form-textbox"/>
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
                    <td  colspan="2">
                        <label>Currency Pair :</label>
                    </td>
                    <td  colspan="2">
                          <select id="currencypair" name="currencypair" class="round input-from-dropdownlist"></select>
                    </td>
                </tr>
                <tr>
                    <td  colspan="2">
                        <label>Buy/Sell :</label>
                    </td>
                    <td  colspan="2">
                        <select id="buysell" name="buysell" class="round input-from-dropdownlist">
                            <option value="B">Buy</option>
                            <option value="S">Sell</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td  colspan="2">
                        <label>Contract CCY:</label>
                    </td>
                    <td  colspan="2">
                        <select id="ccy" name="ccy" class="round input-from-dropdownlist">

                        </select>
                    </td>
                </tr>
                <tr>
                    <td  colspan="2">
                        <label>Spot Date :</label>
                    </td>
                    <td  colspan="2">
                        <input type="text" name="spotdate" id="spotdate" class="round input-form-textbox"/>
                    </td>
                </tr>
                <tr>
                    <td  colspan="2">
                        <label>Spot Rate :</label>
                    </td>
                    <td  colspan="2">
                        <input type="text" id="spotrate" name="spotrate" class="round input-form-number"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <label>Good Fund Deal :</label>
                    </td>
                    <td  colspan="2">
                        <input type="checkbox" id='settleFlag' name='settleFlag' />                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                       <fieldset>
                        <legend>Near Leg</legend> 
                             <table>
                                     <tr>
                                        <td>
                                            <label>Buy/Sell :</label>
                                        </td>
                                        <td>
                                             <input type="text" id="buysellN" name="buysellN" readonly="readonly"   class="round input-form-textbox"/>
                                        </td>
                                    </tr>
                                                         <tr>
                                        <td>
                                            <label>Settlement Date:</label>
                                        </td>
                                        <td>
                                            <input type="text" name="setdateN" id="setdateN" class="round input-form-textbox"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Swap point :</label>
                                        </td>
                                        <td>
                                            <input type="text" id="swappointN" name="swappointN" class="round input-form-number"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Contract Amount :</label>
                                        </td>
                                        <td>
                                            <input type="text" name="contractamtN" id="contractamtN" class="round input-form-number"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Counter Amount :</label>
                                        </td>
                                        <td>
                                            <input type="text" name="counteramtN" id="counteramtN" class="round input-form-number"/>
                                        </td>
                                    </tr>
                            </table>
                       </fieldset>
                    </td>                   
                    <td colspan="2">
                         <fieldset>
                        <legend>Far Leg</legend> 
                            <table>
                                     <tr>
                                        <td>
                                            <label>Buy/Sell :</label>
                                        </td>
                                        <td>  
                                        <input type="text" id="buysellF" readonly="readonly" name="buysellF" class="round input-form-textbox"/>
                                        </td>
                                    </tr>
                                                         <tr>
                                        <td>
                                            <label>Settlement Date:</label>
                                        </td>
                                        <td>
                                            <input type="text" name="setdateF" id="setdateF" class="round input-form-textbox"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Swap point :</label>
                                        </td>
                                        <td>
                                            <input type="text" id="swappointF" name="swappointF" class="round input-form-number"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Contract Amount :</label>
                                        </td>
                                        <td>
                                            <input type="text" name="contractamtF" id="contractamtF"  class="round input-form-number"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Counter Amount :</label>
                                        </td>
                                        <td>
                                            <input type="text" name="counteramtF" id="counteramtF"  class="round input-form-number"/>
                                        </td>
                                    </tr>
                            </table>
                       </fieldset>
                    </td>
                </tr>  
                <tr id="Tr1">
                    <td  colspan="2">
                    &nbsp;
                    </td>
                    <td  colspan="2">
                    &nbsp;
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
                        <% if (Writable)
                           { %><input type="button" id="submit" value="submit" onclick="return submit_onclick()" class="round blue button-submit" /><% } %>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            var productid = 'null';
            var productid2 = 'null';
            var trnRecord1 = null;
            var trnRecord2 = null;
            $(document).ready(function () {
                productid = '<%= ProductId %>';
                $('#title').text('FX Swap Deal Entry');
                $('#setdateN,#setdateF,#spotdate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= GetSpotDateString() %>');
                $('#tradedate').val('<%= CurrentProcessDate %>');
                $('#spotrate').autoNumeric('init', { mDec: 8 });
                $('#contractamtN,#counteramtN,#contractamtF,#counteramtF').autoNumeric('init');
                $('#swappointN').autoNumeric('init', { vMin: '-9999999.99999999', vMax: '9999999.99999999' });
                $('#swappointF').autoNumeric('init', { vMin: '-9999999.99999999', vMax: '9999999.99999999' });
                $('#buysellN').val($('#buysell option:selected').text());
                $('#buysellF').val($('#buysell option:not(:selected)').text());

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
                                $('#spotdate').val(msg.d.record.SpotDate);
                                $('#buysell').val(msg.d.record.BSN);
                                $('#spotrate').val($.number(msg.d.record.SpotRate, 6));
                                msg.d.record.BSN == 'B' ? $('#buysellN').val('Buy') : $('#buysellN').val('Sell');
                                msg.d.record.BSF == 'B' ? $('#buysellF').val('Buy') : $('#buysellF').val('Sell');
                                $('#setdateN').val(msg.d.record.SetDateN);
                                $('#setdateF').val(msg.d.record.SetDateF);
                                $('#swappointN').autoNumeric('set', msg.d.record.SwapPoitN);
                                $('#swappointF').autoNumeric('set', msg.d.record.SwapPoitF);
                                $('#contractamtN').autoNumeric('set', msg.d.record.ContAmtN);
                                $('#counteramtN').autoNumeric('set', msg.d.record.CountAmtN);
                                $('#contractamtF').autoNumeric('set', msg.d.record.ContAmtF);
                                $('#counteramtF').autoNumeric('set', msg.d.record.CountAmtF);
                                $('#remark').val(msg.d.record.Remark);
                                $('#settleFlag').prop('checked', !msg.d.record.flag_settle);
                                productid2 = msg.d.productid2;
                                SetDropdown(msg.d.record.Counterparty, msg.d.record.Portfolio, msg.d.record.Instrument);
                                SetContractCCY(msg.d.CCY);
                                $('#ccy').val(msg.d.record.ContractCcy);
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

                $('#ccy').change(function () {
                    $('#counteramtN,#contractamtN,#contractamtF,#counteramtF').val('');
                });
                $('#spotrate,#swappointN,#swappointF').change(function () {
                    ContractAmtChange();
                });
                $('#buysell').change(function () {
                    $('#buysellN').val($('#buysell option:selected').text());
                    $('#buysellF').val($('#buysell option:not(:selected)').text());
                });

                $('#contractamtN').change(function () { ContractAmtChange(); });
                $('#counteramtN').change(function () { CounterAmtChange(); });
                $('#contractamtF').change(function () { ContractAmtFChange(); });
                $('#counteramtF').change(function () { CounterAmtFChange(); });
                $('#counteramtN').keyup(function (event) {
                    if (event.which == 75 || event.which == 107) {
                        $('#counteramtN').autoNumeric('set', $('#counteramtN').autoNumeric('get') * 1000);
                    }
                    else if (event.which == 77 || event.which == 109) {
                        $('#counteramtN').autoNumeric('set', $('#counteramtN').autoNumeric('get') * 1000000);

                    }
                    return false;
                });
                $('#contractamtN').keyup(function (event) {
                    if (event.which == 75 || event.which == 107) {
                        $('#contractamtN').autoNumeric('set', $('#contractamtN').autoNumeric('get') * 1000);
                    }
                    else if (event.which == 77 || event.which == 109) {
                        $('#contractamtN').autoNumeric('set', $('#contractamtN').autoNumeric('get') * 1000000);

                    }
                    return false;
                });
                $('#counteramtF').keyup(function (event) {
                    if (event.which == 75 || event.which == 107) {
                        $('#counteramtF').autoNumeric('set', $('#counteramtF').autoNumeric('get') * 1000);
                    }
                    else if (event.which == 77 || event.which == 109) {
                        $('#counteramtF').autoNumeric('set', $('#counteramtF').autoNumeric('get') * 1000000);

                    }
                    return false;
                });
                $('#contractamtF').keyup(function (event) {
                    if (event.which == 75 || event.which == 107) {
                        $('#contractamtF').autoNumeric('set', $('#contractamtF').autoNumeric('get') * 1000);
                    }
                    else if (event.which == 77 || event.which == 109) {
                        $('#contractamtF').autoNumeric('set', $('#contractamtF').autoNumeric('get') * 1000000);

                    }
                    return false;
                });
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
                    url: '<%= Page.ResolveClientUrl("FXSwapEntryInfo.aspx/GetInstrument") %>',
                    data: predate,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d.Result == 'OK') {
                            SetContractCCY(msg.d.CCY);
                            ContractAmtChange();
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
            function ContractAmtChange() {
                var ccyType = $('#ccy option:selected').attr('type');
                var spotrate = parseFloat($('#spotrate').autoNumeric('get'));
                var swappointN = parseFloat($('#swappointN').autoNumeric('get'));
                var swappointF = parseFloat($('#swappointF').autoNumeric('get'));
                var counteramtN = $('#counteramtN');
                var counteramtF = $('#counteramtF');
                var contractamtF = $('#contractamtF');
                if ($('#contractamtN').autoNumeric('get') == '') {
                    $('#contractamtN').autoNumeric('set', 0.00);
                }
                var contractamtNval = $('#contractamtN').autoNumeric('get');
                var flag_mul = $('#ccy').data('flag');
                if (ccyType == '1' && flag_mul) {
                    counteramtN.autoNumeric('set', contractamtNval * (spotrate + swappointN));
                    counteramtF.autoNumeric('set', contractamtNval * (spotrate + swappointF));
                }
                else if (ccyType == '2' && flag_mul) {
                    counteramtN.autoNumeric('set', contractamtNval / (spotrate + swappointN));
                    counteramtF.autoNumeric('set', contractamtNval / (spotrate + swappointF));
                }
                else if (ccyType == '1' && !flag_mul) {
                    counteramtN.autoNumeric('set', contractamtNval / (spotrate + swappointN));
                    counteramtF.autoNumeric('set', contractamtNval / (spotrate + swappointF));
                }
                else if (ccyType == '2' && !flag_mul) {
                    counteramtN.autoNumeric('set', contractamtNval * (spotrate + swappointN));
                    counteramtF.autoNumeric('set', contractamtNval * (spotrate + swappointF));
                }
                contractamtF.autoNumeric('set', contractamtNval);
            }

            function CounterAmtChange() {
                var ccyType = $('#ccy option:selected').attr('type');
                var spotrate = parseFloat($('#spotrate').autoNumeric('get'));
                var swappointN = parseFloat($('#swappointN').autoNumeric('get'));
                var swappointF = parseFloat($('#swappointF').autoNumeric('get'));
                var contractamtN = $('#contractamtN');
                var contractamtF = $('#contractamtF');
                var counteramtF = $('#counteramtF');
                var counteramtNval = $('#counteramtN').autoNumeric('get');
                var flag_mul = $('#ccy').data('flag');
                if (ccyType == '1' && flag_mul) {
                    contractamtN.autoNumeric('set',counteramtNval / (spotrate + swappointN));
                    counteramtF.autoNumeric('set',contractamtN.autoNumeric('get') * (spotrate + swappointF));
                }
                else if (ccyType == '2' && flag_mul) {
                    contractamtN.autoNumeric('set',counteramtNval * (spotrate + swappointN));
                    counteramtF.autoNumeric('set',contractamtN.autoNumeric('get') / (spotrate + swappointF));
                }
                else if (ccyType == '1' && !flag_mul) {
                    contractamtN.autoNumeric('set',counteramtNval * (spotrate + swappointN));
                    counteramtF.autoNumeric('set',contractamtN.autoNumeric('get') / (spotrate + swappointF));
                }
                else if (ccyType == '2' && !flag_mul) {
                    contractamtN.autoNumeric('set',counteramtNval / (spotrate + swappointN));
                    counteramtF.autoNumeric('set',contractamtN.autoNumeric('get') * (spotrate + swappointF));
                }
                contractamtF.autoNumeric('set',contractamtN.autoNumeric('get'));
            }

            function ContractAmtFChange() {
                var ccyType = $('#ccy option:selected').attr('type');
                var spotrate = parseFloat($('#spotrate').autoNumeric('get'));
                var swappointF = parseFloat($('#swappointF').autoNumeric('get'));
                var counteramtF = $('#counteramtF');
                if ($('#contractamtF').autoNumeric('get') == '') {
                    $('#contractamtF').autoNumeric('set', 0.00);
                }
                var contractamtFval = $('#contractamtF').autoNumeric('get');
                var flag_mul = $('#ccy').data('flag');
                if (ccyType == '1' && flag_mul) {
                    counteramtF.autoNumeric('set', contractamtFval * (spotrate + swappointF));
                }
                else if (ccyType == '2' && flag_mul) {
                    counteramtF.autoNumeric('set', contractamtFval / (spotrate + swappointF));
                }
                else if (ccyType == '1' && !flag_mul) {
                    counteramtF.autoNumeric('set', contractamtFval / (spotrate + swappointF));
                }
                else if (ccyType == '2' && !flag_mul) {
                    counteramtF.autoNumeric('set', contractamtFval * (spotrate + swappointF));
                }
            }

            function CounterAmtFChange() {
                var ccyType = $('#ccy option:selected').attr('type');
                var spotrate = parseFloat($('#spotrate').autoNumeric('get'));
                var swappointF = parseFloat($('#swappointF').autoNumeric('get'));
                var contractamtF = $('#contractamtF');
                if ($('#counteramtF').autoNumeric('get') == '') {
                    $('#counteramtF').autoNumeric('set', 0.00);
                }
                var counteramtFval = $('#counteramtF').autoNumeric('get');
                var flag_mul = $('#ccy').data('flag');
                if (ccyType == '1' && flag_mul) {
                    contractamtF.autoNumeric('set', counteramtFval / (spotrate + swappointF));
                }
                else if (ccyType == '2' && flag_mul) {
                    contractamtF.autoNumeric('set', counteramtFval * (spotrate + swappointF));
                }
                else if (ccyType == '1' && !flag_mul) {
                    contractamtF.autoNumeric('set', counteramtFval * (spotrate + swappointF));
                }
                else if (ccyType == '2' && !flag_mul) {
                    contractamtF.autoNumeric('set', counteramtFval / (spotrate + swappointF));
                }
            }

             function SetContractCCY(objPCCF) {
                 $('#ccy').append($('<option>', { value: objPCCF.CURRENCYID1 }).text(objPCCF.CURRENCY1).attr('type', '1'));
                 $('#ccy').append($('<option>', { value: objPCCF.CURRENCYID2 }).text(objPCCF.CURRENCY2).attr('type', '2'));
                 $('#ccy').data('flag', objPCCF.FLAG_MULTIPLY);

             }
             function GenerateObjects(blnIsSubmit) {
                 var pData = "{ "
                        + " strTradeDate : '" + $('#tradedate').val() + "'"
                        + ", strCtpy : '" + $('#counterparty').combobox('value') + "'"
                        + ", strPortfolio : '" + $('#portfolio').val() + "'"
                        + ", strCurrencyPair : '" + $('#currencypair').combobox('value') + "'"
                        + ", strContractCcy : '" + $('#ccy').val() + "'"
                        + ", strCounterCcy : '" + $('#ccy option:not(:selected)').val() + "'"
                        + ", strSpotRate : '" + $('#spotrate').autoNumeric('get')+ "'"
                        + ", strBSNear : '" + $('#buysellN').val().substr(0, 1) + "'"
                        + ", strSetDateNear : '" + $('#setdateN').val() + "'"
                        + ", strSwapPointNear : '" + $('#swappointN').autoNumeric('get') + "'"
                        + ", strContractAmtNear : '" + $('#contractamtN').autoNumeric('get') + "'"
                        + ", strCounterAmtNear : '" + $('#counteramtN').autoNumeric('get') + "'"
                        + ", strBSFar : '" + $('#buysellF').val().substr(0, 1) + "'"
                        + ", strSetDateFar : '" + $('#setdateF').val() + "'"
                        + ", strSwapPointFar : '" + $('#swappointF').autoNumeric('get') + "'"
                        + ", strContractAmtFar : '" + $('#contractamtF').autoNumeric('get') + "'"
                        + ", strCounterAmtFar : '" + $('#counteramtF').autoNumeric('get') + "'"
                        + ", strSpotDate : '" + $('#spotdate').val() + "'"
                        + ", strRemark : '" + $('#remark').val() + "'"
                        + ", strProductId1 : " + productid
                        + ", settleFlag : " + $('#settleFlag').prop('checked')
                        + ", blnIsSubmit : " + blnIsSubmit       
                        + " }";
                 $.ajax({
                     type: "POST",
                     url: '<%= Page.ResolveClientUrl("FXSwapEntryInfo.aspx/GenerateTrnObject") %>',
                     data: pData,
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (msg) {
                         if (msg.d.Result == 'OK') {
                             trnRecord1 = msg.d.record;
                             trnRecord2 = msg.d.record2;
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
                     url: '<%= Page.ResolveClientUrl("FXSwapEntryInfo.aspx/CheckFXSwapLimits") %>',
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
             }
             */
             function limitcheck_onclick() {               
                 if (ValidateForm() == true) {
                     GenerateObjects(false);
                 }
             }

             function submit_deal(strOverApprover, strOverComment) {

                 var pData = "{ strOverApprover : '" + strOverApprover + "'"
                        + ", strOverComment : '" + strOverComment + "'"
                        + ", record1 : '" + trnRecord1 + "'"
                        + ", record2 : '" + trnRecord2 + "'"
                        + ", strProductId1 : " + productid
                        + ", strProductId2 : '" + productid2 + "'"
                        + " }";

                 $.ajax({
                     type: "POST",
                     url: '<%= Page.ResolveClientUrl("FXSwapEntryInfo.aspx/SubmitDeal") %>',
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
                     width: 1024,
                     modal: true
                 });

             }

             function ValidateForm() {
                 var mess = '';
                 mess = $("#tradedate").val() == '' ? 'Please input trade date.' : mess;
                 mess = ($('#counterparty').combobox('value') == null || $('#counterparty').combobox('value') == '') && mess == '' ? 'Please input counterparty.' : mess;
                 mess = $("#portfolio").val() == '-1' && mess == '' ? 'Please select portfolio.' : mess;
                 mess = ($('#currencypair').combobox('value') == null || $('#currencypair').combobox('value') == '') && mess == '' ? 'Please input currency pair.' : mess;
                 mess = ($("#ccy").val() == null || $("#ccy").val() == '') && mess == '' ? 'Please select contract ccy:.' : mess;
                 mess = $("#spotrate").autoNumeric('get') == '' && mess == '' ? 'Please input spot rate.' : mess;
                 mess = $("#spotdate").val() == '' && mess == '' ? 'Please input spot date.' : mess;
                 mess = $("#setdateN").val() == '' && mess == '' ? 'Please input near leg\'s settlement date.' : mess;
                 mess = $("#setdateF").val() == '' && mess == '' ? 'Please input far leg\'s settlement date.' : mess;
                 mess = $("#swappointN").autoNumeric('get') == '' && mess == '' ? 'Please input  near leg\'s swap point.' : mess;
                 mess = $("#swappointF").autoNumeric('get') == '' && mess == '' ? 'Please input  far leg\'s swap point.' : mess;
                 mess = isValidDate($("#spotdate").val()) == false && mess == '' ? 'Invalid spot date.' : mess;
                 mess = isValidDate($("#setdateN").val()) == false && mess == '' ? 'Invalid near leg\'s settlement date.' : mess;
                 mess = isValidDate($("#setdateF").val()) == false && mess == '' ? 'Invalid far leg\'s settlement date.' : mess;
                 mess = StrToDate($("#tradedate").val()) > StrToDate($("#setdateN").val()) && mess == '' ? 'Near leg\'s settlement date cannot be before trade date.' : mess;
                 mess = StrToDate($("#setdateN").val()) >= StrToDate($("#setdateF").val()) && mess == '' ? 'Far leg\'s settlement date must be after near leg\'s settlement date.' : mess;

                 mess = $("#contractamtN").autoNumeric('get') == '' && mess == '' ? 'Please input near leg\'s contract amount.' : mess;
                 mess = $("#contractamtN").autoNumeric('get') <= 0 && mess == '' ? 'Invalid near leg\'s contract amount.' : mess;
                 mess = $("#counteramtN").autoNumeric('get') == '' && mess == '' ? 'Please input near leg\'s counter amount.' : mess;
                 mess = $("#counteramtN").autoNumeric('get') <= 0 && mess == '' ? 'Invalid near leg\'s counter amount.' : mess;
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
                 set_options('currencypair', null, 'GetFXSwapInstrumentOptions', function () {
                     $('#currencypair').combobox('value', currencypairVal);
                 });
             }

         </script>
    </form>
       
</asp:Content>
