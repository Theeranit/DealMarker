<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="RepoEntryInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.RepoEntryInfo" %>
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
                    <input type="text" name="tradedate" id="tradedate" class="round input-form-textbox"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Buy/Sell Security :</label>
                </td>
                <td>
                    <select id="buysell" class="round input-from-dropdownlist">
                        <option value="B">Buy</option>
                        <option value="S">Sell</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Instrument :</label>
                </td>
                <td>
                    <select id="instrument" name="instrument" ></select>
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
                    <select id="portfolio" class="round input-from-dropdownlist"></select>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Effective Date :</label>
                </td>
                <td>
                    <input type="text" name="effdate" id="effdate" class="round input-form-textbox"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Maturity Date :</label>
                </td>
                <td>
                    <input type="text" name="madate" id="madate" class="round input-form-textbox"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Bond Market Value :</label>
                </td>
                <td>
                    <input type="text" name="notional" id="notional" class="round input-form-number"/> &nbsp; <label id='ccy'></label>
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
                    <input type="button" id="limitcheck" value="Limit Check" onclick="return limitcheck_onclick()" class="round blue button-submit"/>
                </td>
                <td>
                    <% if(Writable) { %><input type="button" id="submit" value="submit" onclick="return submit_onclick()" class="round blue button-submit"/> <% } %>
                </td>
            </tr>
        </table>
    </div>

    
    <script type="text/javascript">
        var productid = null;
        var trnRecord = null;
        $(document).ready(function () {
            productid = '<%= ProductId %>';
            $('#title').text('Repo Deal Entry');

            $('#tradedate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= CurrentProcessDate %>');
            $('#madate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= GetSpotDateString() %>');
            $('#effdate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= GetSpotDateString() %>');
            $('#notional').autoNumeric('init');
            $('#notional').keyup(function (event) {
                if (event.which == 75 || event.which == 107) {
                    $('#notional').autoNumeric('set', $('#notional').autoNumeric('get') * 1000);
                }
                else if (event.which == 77 || event.which == 109) {
                    $('#notional').autoNumeric('set', $('#notional').autoNumeric('get') * 1000000);

                }
                return false;
            });


            if (productid != 'null') {
                $.ajax({
                    type: "POST",
                    url: window.location.pathname + "/" + "GetEditByID", 
                    data: '{ id : ' + productid + ' }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d.Result == 'OK') {
                            $('#tradedate').val(msg.d.record.TradeDate);
                            $('#buysell').val(msg.d.record.BuySell);
                            $('#madate').val(msg.d.record.MaturityDate);
                            $('#effdate').val(msg.d.record.EffectiveDate);
                            $('#notional').autoNumeric('set', msg.d.record.Notional);
                            $('#remark').val(msg.d.record.Remark);
                            SetDropdown(msg.d.record.Counterparty, msg.d.record.Portfolio, msg.d.record.Instrument);
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


            $('#instrument').combobox({
                change: function (evt, ui) {
                    $('#instrument').combobox('value', $('#instrument').combobox('value'));
                    //GetCurrency();
                }
            });

            $('#counterparty').combobox({
                change: function (evt, ui) {
                    $('#counterparty').combobox('value', $('#counterparty').combobox('value'));
                }
            });

        });
        function SetDropdown(counterpartyVal, portfolioVal, instrumentVal) {
            set_options('counterparty', null, 'GetCounterpartyOptions', function () {
                $('#counterparty').combobox('value', counterpartyVal);
            });
            set_options('instrument', null, 'GetFIInstrumentOptions', function () {
                $('#instrument').combobox('value', instrumentVal);
                //if (instrumentVal != null) GetCurrency();
            });
            set_options('portfolio', 'Please select', 'GetPortfolioOptions', function () {
                if (portfolioVal != null) {
                    $('#portfolio').val(portfolioVal);
                }
                else {
                    $("#portfolio option").filter(function () {
                        return this.text == 'BANKING';
                    }).attr('selected', true);
                }
            });
        }

        function submit_onclick() {
            if (ValidateForm() == true) {
                $('input[id="submit"]').prop('disabled', true);
                GenerateObjects(true);
            }
        }

        function limitcheck_onclick() {
          if (ValidateForm()==true) 
            GenerateObjects(false);
        }

        function submit_deal(strOverApprover, strOverComment) {
            var pData = "{ strOverApprover : '" + strOverApprover + "'"
                        + ", strOverComment : '" + strOverComment + "'"
                        + ", record : '" + trnRecord + "'"
                        + ", strProductId : " + productid
                        + " }";

            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("RepoEntryInfo.aspx/SubmitDeal") %>',
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

        function open_limit_dialog(blnNeedApprove, pces, country) {
            if (blnNeedApprove)
                $("#over-limit").show();
            else
                $("#over-limit").hide();

            $('#limit-preset').jtable('load', { record: pces });
            $('#limit-preset').show();

            $('#limit-country').jtable('load', { record: country });
            $('#limit-country').show();

            $("#limit-dialog").dialog({
                title: 'Limit Contribution',
                height: 450,
                width: 1024,
                modal: true
            });

        }

        function GenerateObjects(blnIsSubmit) {
            var pData = "{ strTradeDate : '" + $('#tradedate').val() + "'"
                        + ", strBuySell : '" + $('#buysell').val() + "'"
                        + ", strInstrument : '" + $('#instrument').combobox('value') + "'"
                        + ", strCtpy : '" + $('#counterparty').combobox('value') + "'"
                        + ", strPortfolio : '" + $('#portfolio').val() + "'"
                        + ", strEffectiveDate : '" + $('#effdate').val() + "'"
                        + ", strMaturityDate : '" + $('#madate').val() + "'"
                        + ", strNotional : '" + $('#notional').autoNumeric('get') + "'"
                        + ", strProductId : " + productid
                        + ", strRemark : '" + $('#remark').val() + "'"
                        + ", blnIsSubmit : " + blnIsSubmit                 
                        + " }";
            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("RepoEntryInfo.aspx/GenerateTrnObject") %>',
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
                                open_limit_dialog(false, msg.d.pcesce.PCERecords, msg.d.pcesce.CountryRecords);
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
                            open_limit_dialog(true, msg.d.pcesce.PCERecords, msg.d.pcesce.CountryRecords);
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

        function CheckLimits(blnIsSubmit) {
            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("RepoEntryInfo.aspx/CheckRepoLimit") %>',
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

        function ValidateForm() {
                 var mess = '';
                 mess = $("#tradedate").val() == '' ? 'Please input trade date.' : mess;
                 mess = ($('#instrument').combobox('value') == null || $('#instrument').combobox('value') == '') && mess == '' ? 'Please input instrument.' : mess;
                 mess = ($('#counterparty').combobox('value') == null || $('#counterparty').combobox('value') == '') && mess == '' ? 'Please input counterparty.' : mess;
                 mess = $("#portfolio").val() == '-1' && mess == '' ? 'Please select portfolio.' : mess;
                 mess = isValidDate($("#effdate").val()) == false && mess == '' ? 'Invalid effective date.' : mess;
                 mess = StrToDate($("#tradedate").val()) > StrToDate($("#effdate").val()) && mess == '' ? 'Effective date cannot be before trade date.' : mess;
                 mess = isValidDate($("#madate").val()) == false && mess == '' ? 'Invalid maturity date.' : mess;
                 mess = StrToDate($("#effdate").val()) > StrToDate($("#madate").val()) && mess == '' ? 'Maturity date cannot be before effective date.' : mess;
                 mess = $("#notional").autoNumeric('get') == '' && mess == '' ? 'Please input bond market value.' : mess;
                 mess = $("#notional").autoNumeric('get') <= 0 && mess == '' ? 'Invalid bond market value.' : mess;
                              
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
    </script>
</form>
</asp:Content>
