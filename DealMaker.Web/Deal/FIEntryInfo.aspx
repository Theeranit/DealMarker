<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FIEntryInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.FIEntryInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <form id="form1" runat="server">
    <div id="message-dialog" style="display:none;">
        <label id="message" />
    </div>
    <div id="loading" style="display:none;">
        <p align="center"><img alt="Loading" src="../Images/loading.gif" /></p>
    </div>
    <div class="table-container">
        <table>
            <tr>
                <td>
                    <div><label for="tradedate">Trade Date:</label></div>               
                    <div><input type="text" name="tradedate" id="tradedate" class="round input-form-textbox-horizontal"/></div>   
                </td>
                <td>
                    <div><label>Buy/Sell :</label></div>  
                    <div><select id="buysell" class="round input-from-dropdownlist-horizontal">
                        <option value="B">Buy</option>
                        <option value="S">Sell</option>
                    </select></div>  
                </td>
                <td>
                    <div><label>Instrument :</label></div>  
                    <div><select id="instrument" name="instrument"class="round input-from-dropdownlist-horizontal"></select></div> 
                    <input type="hidden" id="lotsize" name="lotsize" /> 
                </td>
                <td>
                    <div><label>Counterparty :</label></div>  
                     <div><select id="counterparty" name="counterparty" class="round input-from-dropdownlist-horizontal"></select></div>  
                </td>
                <td>
                    <div><label>Portfolio :</label></div>  
                    <div><select id="portfolio" class="round input-from-dropdownlist-horizontal"></select></div>  
                </td>
                <td>
                    <div><label>Settlement Date :</label></div> 
                    <div><input type="text" name="settledate" id="settledate" class="round input-form-textbox-horizontal"/></div> 
                </td>
                 <td>
                    <div><label>Unit :</label></div> 
                    <div><input type="text" name="unit" id="unit" class="round input-form-number-horizontal"/></div> 
                </td>
                 <td>
                    <div><label>Yield :</label></div> 
                    <div><input type="text" name="yield" id="yield" class="round input-form-number-horizontal"/></div> 
                </td>
                 <td>
                    <div><label>Clean Price :</label></div> 
                    <div><input type="text" name="cprice" id="cprice" class="round input-form-number-horizontal"/></div> 
                </td>
                 <td>
                    <div><label>Gross Price :</label></div> 
                    <div><input type="text" name="gprice" id="gprice" class="round input-form-number-horizontal"/></div> 
                </td>
                <td>
                    <div><label>Gross Amount :</label></div><label id='ccy'></label>
                    <div><input type="text" name="notional" id="notional" class="round input-form-number-horizontal"/></div> 
                </td>
            </tr>
             <tr>
                <td>
                    <div><label>Primary Market :</label></div>
                    <div><input type="checkbox" id='pmarket' name='pmarket' /></div>
                </td>
                <td>
                    <div><label>non-DVP :</label></div>
                    <div><input type="checkbox" id='settleFlag' name='settleFlag' /></div>
                </td>
                 <td>
                    <div><label>Yield Type :</label></div>  
                    <div><select id="ytype" class="round input-from-dropdownlist-horizontal"></select></div>  
                </td>
                 <td>
                    <div><label>Report By :</label></div>  
                    <div><select id="reportby" class="round input-from-dropdownlist-horizontal"></select></div>  
                </td>
                 <td>
                    <div><label>Purpose :</label></div>  
                    <div><select id="purpose" class="round input-from-dropdownlist-horizontal"></select></div>  
                </td>
                 <td>
                    <div><label>Term :</label></div> 
                    <div><input type="text" name="term" id="term" class="round input-form-number-horizontal"/></div> 
                </td>
                 <td>
                    <div><label>Rate :</label></div> 
                    <div><input type="text" name="rate" id="rate" class="round input-form-number-horizontal"/></div> 
                </td>
                 <td>
                    <div><label>TBMA Remark :</label></div> 
                    <div><input type="text" name="tbmaremark" id="tbmaremark" class="round input-form-textbox-horizontal"/></div> 
                </td>
            </tr>
            <tr id="remarkrow">
            <td>
                <div><label>Comment :</label></div> 
                <div><input type="text" name="remark" id="remark" class="round input-form-textbox-horizontal"/></div> 
            </td>
            </tr>
            <tr>
                <td>
                    <input type="button" id="limitcheck" value="Limit Check" onclick="return limitcheck_onclick()" class="round blue button-submit"/>
                </td>
                <td>
                    <% if(Writable) { %>
                        <input type="button" id="submit" value="submit" onclick="return submit_onclick()" class="round blue button-submit"/>
                    <% } %>
                </td>
                <td>
                    <% if(Writable) { %>
                        <div><label>Refresh input screen :</label></div> 
                        <div><input type="checkbox" id='isrefresh' name='isrefresh' checked/></div>
                    <% } %>
                </td>
            </tr>
            <tr>
                <td colspan="11">
                    <hr />                    
                </td>
            </tr>
            <tr>
                <td colspan="11">
                    <div id="TableContainer">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="11">
                    <% if(Writable) { %>
                    <input type="button" id="SendReport" value="Report" onclick="return SendReport_onclick()" class="round blue button-submit"/>
                    <% } %>
                    <input type="hidden" id="TransID" name="TransID" />
                </td>
            </tr>
        </table>
        </div>
    <div id="delete-dialog" title="Are you sure?" style="display:none;">
        <p>
            <span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
            <span class="jtable-delete-confirm-message">This record will be deleted. Are you sure?</span>
        </p>
        <p>
            <label>Comment : </label>
            <input type="text" id="deleteremark" class="round input-form-textbox"/>
        </p>
        <p id="requiremessage" style="color:Red;display:none">
            <label>Please input comment</label>
        </p>
    </div>
    <script type="text/javascript">
        var productid = null;
        var trnRecord = null;
        var myVarInterval ;
        $(document).ready(function () {
            productid = '<%= ProductId %>';
            $('#title').text('Fixed Income Deal Entry');

            $('#tradedate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= CurrentProcessDate %>');
            $('#settledate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= GetSpotDateString() %>');
            $('#unit,#term').autoNumeric('init', { mDec: 0 });
            $('#yield,#cprice,#gprice,#rate').autoNumeric('init', { mDec: 6 });
            $('#notional').autoNumeric('init', { mDec: 2 });
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
                    url: window.location.pathname + "/" + "GetEditByID", //'<%= Page.ResolveClientUrl("FIEntryInfo.aspx/' + strFunction + '" ) %>',
                    data: '{ id : ' + productid + ' }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d.Result == 'OK') {
                            $('#tradedate').val(msg.d.record.TradeDate);
                            $('#buysell').val(msg.d.record.BuySell);
                            $('#settledate').val(msg.d.record.MaturityDate);
                            $('#notional').val($.number(msg.d.record.Notional1, 2));
                            $('#yield').val($.number(msg.d.record.Yield, 6));
                            $('#unit').val($.number(msg.d.record.Unit));
                            $('#cprice').val($.number(msg.d.record.CPrice, 6));
                            $('#gprice').val($.number(msg.d.record.GPrice, 6));
                            $('#remark').val(msg.d.record.Remark);
                            $('#term').val(msg.d.record.Term != "" ? $.number(msg.d.record.Term) : "");
                            $('#rate').val(msg.d.record.Rate != "" ? $.number(msg.d.record.Rate, 6) : "");
                            $('#tbmaremark').val(msg.d.record.TBMARemark);
                            if (msg.d.record.PMarket == "1")
                                $('#pmarket').prop('checked', true);
                            if (msg.d.record.SettleFlag == "1")
                                $('#settleFlag').prop('checked', true);
                            $('#lotsize').val(msg.d.record.LotSize);
                            SetDropdown(msg.d.record.Counterparty, msg.d.record.Portfolio, msg.d.record.Instrument, msg.d.record.YType, msg.d.record.ReportBy, msg.d.record.Purpose);
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
                SetDropdown(null, null, null, null, null, null);
                $("#remarkrow").hide();
            }

            $('#gprice').change(function(){CalGrossAmount();}); 
            $('#unit').change(function(){CalGrossAmount();});

            $('#settledate').change(function(){CallTBMA(true);}); 
            $('#yield').change(function(){CallTBMA(true);}); 
            $('#cprice').change(function(){CallTBMA(false);});

            $('#instrument').combobox({
                create: function (event, ui) { $(".ui-combobox-input").css('width', '90px'); },
                change: function (evt, ui) {
                    $('#instrument').combobox('value', $('#instrument').combobox('value'));
                    GetCurrency();
                    GetLotSize();
                    CallTBMA(true);
                }
            });

            $('#counterparty').combobox({
                create: function (event, ui) { $(".ui-combobox-input").css('width', '90px'); },
                change: function (evt, ui) {
                    $('#counterparty').combobox('value', $('#counterparty').combobox('value'));
                }
            });

            $('#TableContainer').jtable({
                title: 'Bond Deal Today',
                paging: true,
                pageSize: 10,
                selecting: true, //Enable selecting
                multiselect: true, //Allow multiple selecting
                selectingCheckboxes: true, //Show checkboxes on first column
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/GetDealInternalByProcessDate") %>'
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    InsertState: {
                        title: 'Time(min)',
                        display: function (data) {
                            if (data.record.InsertState == "Sent" || data.record.InsertState == "Cancelled")
                                return '<div style="background-color:gray;width:100%;text-align:center">' + data.record.InsertState + '</div>';
                            else if (data.record.InsertState == "Late")
                                return '<div style="background-color:red;width:100%;text-align:center">' + data.record.InsertState + '</div>';
                            else if (parseInt(data.record.InsertState) > 600)
                                return '<div style="background-color:rgb(26, 245, 14);width:100%;text-align:center" class="countdownTimer" value="' + data.record.InsertState + '">' + countdown(data.record.InsertState) + '</div>';
                            else
                                return '<div style="background-color:yellow;width:100%;text-align:center" class="countdownTimer" value="' + data.record.InsertState + '">' + countdown(data.record.InsertState) + '</div>';
                        }
                    },
                    DMK_NO: {
                        title: 'DMK NO'
                    },
                    OPICS_NO: {
                        title: 'OPICS NO'
                    },
                    TradeDate: {
                        title: 'Trade Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    MaturityDate: {
                        title: 'Settlement Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    Instrument: {
                        title: 'Instrument'
                    },
                    BuySell: {
                        title: 'B/S'
                    },
                    Portfolio: {
                        title: 'Portfolio'
                    },
                    Counterparty: {
                        title: 'CTPY'
                    },
                    Yield: {
                        title: 'Yield',
                        type: 'number',
                        displayFormat: '6',
                        listClass: 'right'
                    },
                    GrossValue: {
                        title: 'Gross Value',
                        type: 'number',
                        displayFormat: '2',
                        listClass: 'right'
                    },
                    CCY: {
                        title: 'CCY'
                    },
                    Trader: {
                        title: 'Trader'
                    },
                    Status: {
                        title: 'Status'
                    },
                    PCE: {
                        title: 'PCE'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    Sender: {
                        title: 'Sender',
                        visibility: 'hidden'
                    },
                    Unit: {
                        title: 'Unit',
                        visibility: 'hidden'
                    },
                    CleanPrice: {
                        title: 'CleanPrice',
                        visibility: 'hidden'
                    },
                    GrossPrice: {
                        title: 'GrossPrice',
                        visibility: 'hidden'
                    },
                    YieldType: {
                        title: 'YieldType',
                        visibility: 'hidden'
                    },
                    ReporyBy: {
                        title: 'ReportBy',
                        visibility: 'hidden'
                    },
                    Purpose: {
                        title: 'Purpose',
                        visibility: 'hidden'
                    },
                    Term: {
                        title: 'Term',
                        visibility: 'hidden'
                    },
                    Rate: {
                        title: 'Rate',
                        visibility: 'hidden'
                    },
                    TBMA_Remark: {
                        title: 'TBMA_Remark',
                        visibility: 'hidden'
                    },
                    SendTime: {
                        title: 'SendTime',
                        visibility: 'hidden'
                    },
                    PrimaryMarket: {
                        title: 'PrimaryMarket',
                        visibility: 'hidden'
                    },
                    NonDVP: {
                        title: 'NonDVP',
                        visibility: 'hidden'
                    }
                    <% if (Writable) { %>
                        ,CustomAction: {
                            title: '',
                            width: '1%',
                            sorting: false,
                            create: false,
                            edit: false,
                            list: true,
                            display: function (data) {
                                    return '<button title="Edit Record" class="jtable-command-button jtable-edit-command-button" onclick="editProduct(\'' + data.record.ID + '\',\'' + data.record.Product + '\' ); return false;"><span>Edit Record</span></button>';
                            
                            }
                        } ,CustomAction2: {
                            title: '',
                            width: '1%',
                            sorting: false,
                            create: false,
                            edit: false,
                            list: true,
                            display: function (data) {
                                    return '<button title="Delete Record" class="jtable-command-button jtable-delete-command-button" onclick="deleteProduct(\'' + data.record.ID + '\'); return false;"><span>Delete</span></button>';
                               
                            }
                        }
                    <% } %>
                },
                recordsLoaded: function (event, data) {
                    $('#TableContainer').jtable('deselectRows');
                    clearInterval(myVarInterval);
                    myVarInterval = setInterval(function () {
                        $(".countdownTimer").each(function (i, n) {
                            remain = parseInt($(n).attr('value'));
                            $(n).text(countdown(remain));
                            $(n).attr('value', remain - 1);
                            if (remain <= 0) {
                                $(n).removeClass('countdownTimer');
                                $(n).text('Late');
                                $(n).css('background-color', 'red');
                            }
                            else if (remain <= 600 && $(n).css('background-color') != 'rgb(255, 255, 0)') {
                                $(n).css('background-color', 'yellow');
                            }
                        });

                    }, 1000);
                },
                rowInserted: function (event, data) {
                    if (data.record.InsertState == 'Sent' || data.record.InsertState == 'Cancelled') {
                        data.row.find('input:checkbox').remove();
                    }
                },
                selectionChanged: function () {
                    //Get all selected rows
                    var $selectedRows = $('#TableContainer').jtable('selectedRows');
                    $('#TransID').val('');
                    if ($selectedRows.length > 0) {
                        //Show selected rows
                        $selectedRows.each(function () {
                            var record = $(this).data('record');
                            if (record.InsertState == 'Sent') {
                                $(this).removeClass('jtable-row-selected');
                            }
                            else {
                                if ($('#TransID').val() != '') $('#TransID').val($('#TransID').val() + ",");
                                $('#TransID').val($('#TransID').val() + record.ID);
                            }

                        });
                    } else {
                        $('#TransID').val('');
                        //No rows selected
                        //alert('No row selected! Select rows to see here...');
                    }
                }
            });

            //ToolTip
            var cols = new Array('Sender', 'Unit', 'Clean Price', 'Gross Price'
                                , 'Yield Type', 'Report By', 'Purpose'
                                , 'Term', 'Rate', 'TBMA Remark'
                                , 'Send Time', 'Primary Market', 'Non-DVP'); // header for outer columns
            $('table.jtable > tbody').on('mouseover', 'tr.jtable-data-row', function (e) {
                var trEle = $(this);
                var tdEle = trEle.children('td:hidden');
                var titleStr = '';
                $.each(tdEle, function (index, ele) {
                    if (cols[index] != null) {
                        titleStr += cols[index];
                        titleStr += ' = ';
                        titleStr += $(ele).text();
                        titleStr += (index < tdEle.length - 1 ? '<br>' : '');
                    }
                });
                trEle.children('td').eq(2).attr('alt', titleStr);
            });
            $('#TableContainer').jtable('load');
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
        });
        function SendReport_onclick() {
            if ($('#TransID').val() == "") {
                alert('Please select DMK deal before report.');
                return false;
            }

            //do something
            var pData = "{ TransID : '" + $('#TransID').val() + "'"
                        + " }";

            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/SendFIReport") %>',
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
                                $('#TableContainer').jtable('load');
                                $(this).dialog("close");
                                $('#TransID').val('');

                            }
                        }
                    });


                },
                error: function () { }
            });
        }
        function countdown(_seconds_left) {
            // find the amount of "seconds" between now and target
            var seconds_left = parseInt(_seconds_left);
            // do some time calculations
            //days = parseInt(seconds_left / 86400);
            //seconds_left = seconds_left % 86400;

            //hours = parseInt(seconds_left / 3600);
            //seconds_left = seconds_left % 3600;

            minutes = parseInt(seconds_left / 60);
            seconds = parseInt(seconds_left % 60);
            var secstr = "0" + seconds;
            // format countdown string + set tag value
            return minutes + ":" + secstr.substr(secstr.length-2, 2); //days + "d, " + hours + "h, "
                                //+
                                
        
        }
        function submit_onclick() {
            //over limit -> limit check_onclick -> verify approver -> call save deal
            //else -> call save deal
            $('input[id="submit"]').prop('disabled', true);
            GenerateObjects(true);
        }
        
        function limitcheck_onclick() {
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
                url: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/SubmitDeal") %>',
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
                            close: function (event, ui) {
                                if ($('#isrefresh').prop('checked'))
                                    window.parent.document.getElementById("Iframe").src = window.location.pathname;
                                else
                                    $('#TableContainer').jtable('load');                                    
                            }
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

        function open_limit_dialog(blnNeedApprove,pces , sces, country) {
            if (blnNeedApprove)
                $("#over-limit").show();
            else
                $("#over-limit").hide();

            $('#limit-preset').jtable('load', {record : pces});
            $('#limit-preset').show();

            if ($('#settleFlag').prop('checked') == true) {
                $('#limit-set').jtable('load', { record: sces });
                $('#limit-set').show();
            }
            else {
                $('#limit-set').hide();            
            }

            $('#limit-country').jtable('load', {record : country});
            $('#limit-country').show();

            $("#limit-dialog").dialog({
                title: 'Limit Contribution',
                height: 450,
                width: 1200,
                modal: true
            });

        }

        function GenerateObjects(blnIsSubmit) {
            var pData = "{ strTradeDate : '" + $('#tradedate').val() + "'"
                        + ", strBuySell : '" + $('#buysell').val() + "'"
                        + ", strInstrument : '" + $('#instrument').combobox('value') + "'"
                        + ", strCtpy : '" + $('#counterparty').combobox('value') + "'"
                        + ", strPortfolio : '" + $('#portfolio').val() + "'"
                        + ", strSettlementDate : '" + $('#settledate').val() + "'"
                        + ", strYield : '" + $('#yield').autoNumeric('get') + "'"
                        + ", strUnit : '" + $('#unit').autoNumeric('get') + "'"
                        + ", strCleanPrice : '" + $('#cprice').autoNumeric('get') + "'"
                        + ", strGrossPrice : '" + $('#gprice').autoNumeric('get') + "'"
                        + ", strNotional : '" + $('#notional').autoNumeric('get') + "'"
                        + ", strCCY : '" + $('#ccy').data('id') + "'"
                        + ", strPceFlag : '" + ($('#pmarket').prop('checked') ? '1' : '0').toString() + "'"
                        + ", strSettleFlag : '" + ($('#settleFlag').prop('checked') ? '1' : '0').toString() + "'"
                        + ", strYeildType : '" + $('#ytype').val() + "'"
                        + ", strReportBy : '" + $('#reportby').val() + "'"
                        + ", strPurpose : '" + $('#purpose').val() + "'"
                        + ", strTerm: '" + $('#term').autoNumeric('get') + "'"
                        + ", strRate : '" + $('#rate').autoNumeric('get') + "'"
                        + ", strTBMARemark : '" + $('#tbmaremark').val() + "'"
                        + ", strRemark : '" + $('#remark').val() + "'"
                        + ", strProductId : " + productid
                        + ", blnIsSubmit : " + blnIsSubmit
                        + " }";
            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/GenerateTrnObject") %>',
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

        function SetDropdown(counterpartyVal, portfolioVal, instrumentVal,ytypeVal,reportbyVal,purposeVal) {
            set_options('counterparty', null, 'GetCounterpartyOptions', function () {
                $('#counterparty').combobox('value', counterpartyVal);
            });
            set_options('instrument', null, 'GetFIInstrumentOptions', function () {
                $('#instrument').combobox('value', instrumentVal);
                if(instrumentVal != null)  GetCurrency();
            });
            set_options('portfolio', 'Please select', 'GetPortfolioOptions', portfolioVal != null ? function () {
                $('#portfolio').val(portfolioVal);
            } : null);
            set_options('ytype', null, 'GetYeildTypeOptions', ytypeVal != null ? function () {
                $('#ytype').val(ytypeVal);
            } : null);
            set_options('reportby', null, 'GetReportByOptions', reportbyVal != null ? function () {
                $('#reportby').val(reportbyVal);
            } : null);
            set_options('purpose', null, 'GetPurposeOptions', purposeVal != null ? function () {
                $('#purpose').val(purposeVal);
            } : null);
        }
        function GetCurrency() {
            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/GetCCYByInstrumentID") %>', //'<%= Page.ResolveClientUrl("FIEntryInfo.aspx/' + strFunction + '" ) %>',
                data: '{ id : "' + $('#instrument').combobox('value') + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d.Result == 'OK') {
                        $('#ccy').text('('+msg.d.record.label+')');
                        $('#ccy').data('id',msg.d.record.id);
                    }
                    else {
                        $('#ccy').text('(No Data.)');
                    }
                },
                error: function () {
                    $('#ccy').text('(No Data.)');
                }
            });
        }
        
        function GetLotSize() {
            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/GetLotSizeByInstrumentID") %>', //'<%= Page.ResolveClientUrl("FIEntryInfo.aspx/' + strFunction + '" ) %>',
                data: '{ ID : "' + $('#instrument').combobox('value') + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d.Result == 'OK') {
                        $('#lotsize').val(msg.d.lotsize);
                    }
                    else {
                        $('#lotsize').val(0);
                    }
                    CalGrossAmount()
                },
                error: function () {                   
                    $('#lotsize').val(0);
                    CalGrossAmount()
                }
            });
        }
        function editProduct(ID,Product){

         var path ='Deal/FIEntryInfo.aspx?id=';
            if(path != ''){
                window.parent.document.getElementById("Iframe").src = path + ID;  
            }
        }

        function deleteProduct(ID1){
            $("#delete-dialog").dialog({
                title: 'Are you sure?',
                height: 220,
                width: 350,
                modal: true,
                close: function( event, ui ) { $('#deleteremark').val(""); $('#requiremessage').hide();},
                buttons: {
                        Cancel: function() {
                            $( this ).dialog( "close" );
                        },
                        "Delete": function() {
                        if($.trim($('#deleteremark').val()) != ""){
                                $('#TableContainer').jtable('updateRecord', {
                                    record: {
                                        ID: ID1,                                                        
                                        REMARK: $('#deleteremark').val()
                                    },
                                    url: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/CancelDeal") %>',
                                    success: function(){                                                        
                                        $('#TableContainer').jtable('load');
                                    }
                                });
                                $( this ).dialog( "close" );
                            }
                            else{
                            $('#requiremessage').show();
                            }
                        }
                        }
            });
        
        }

        function CalGrossAmount(){
            var gprice = $('#gprice').autoNumeric('get');
            var unit = $('#unit').autoNumeric('get');
            var lotsize = $('#lotsize').val();
            if(gprice != "" && unit != "" && lotsize != ""){
                $('#notional').autoNumeric('set', gprice * unit * lotsize / 100);
            }
            else {
                $('#notional').val('');
            }
        }

        function CallTBMA(yield2Price) {
            var enable = <%= IsTBMACalEnable() %>;

            if (enable == '1') {
                var setdate = $('#settledate').val();
                var instrumentid = $('#instrument').combobox('value');
                var ytype = $('#ytype').val();
                var yield = $('#yield').autoNumeric('get') == "" ? 0 : $('#yield').autoNumeric('get');
                var cprice = $('#cprice').autoNumeric('get') == "" ? 0 : $('#cprice').autoNumeric('get');

                if(setdate != "" && instrumentid != null && (yield != "" || cprice != ""))
                {
                   var pData = "{ instrumentid : '" + instrumentid + "'"
                                + ", setdate : '" + setdate + "'"
                                + ", yield : " + yield
                                + ", cprice : " + cprice
                                + ", ytype : '" + ytype + "'"
                                + ", y2p : " + yield2Price
                                + " }";
                    $.ajax({
                        type: "POST",
                        url: '<%= Page.ResolveClientUrl("FIEntryInfo.aspx/CallTBMA") %>',
                        data: pData,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function(){
                            $("#loading").dialog({
                                title: 'Calling ThaiBMA Service',
                                height: 150,
                                width: 300,
                                modal: true,
                                closeOnEscape: false,
                                beforeclose: function (event, ui) { return false; },
                                dialogClass: "noclose"
                            });
                        },
                        success: function (msg) {
                            $("#loading").dialog('close');
                        
                            if (msg.d.Result == 'OK') {
                                $('#cprice').autoNumeric('set', msg.d.cprice);
                                $('#gprice').autoNumeric('set', msg.d.gprice);
                                $('#yield').autoNumeric('set', msg.d.yield);

                                CalGrossAmount();
                            }
                            else {
                                $("#message").text(msg.d.Message);
                                $("#message-dialog").dialog({
                                    title: 'ERROR',
                                    height: 200,
                                    width: 350,
                                    modal: true
                                });
                            }
                        
                        },
                        error: function (e) {
                            $("#loading").dialog('close');

                            $("#message").text(e.toString());
                            $("#message-dialog").dialog({
                                title: 'ERROR',
                                height: 200,
                                width: 350,
                                modal: true
                            });
                        }
                    });
                }
            }
        }
    </script>
    </form>
</asp:Content>
