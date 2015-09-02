<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="DealView.aspx.cs" Inherits="KK.DealMaker.Web.View.DealView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering" style="background-color:transparent">
                <form id="Form1">
                <input type="hidden" value="Deal" name="reportName" />
                    <table id="tbForm">
                        <tr>
                            <td align="right" style="width:10%">
                                <label>DMK NO:</label>
                            </td>
                            <td align="left"  style="width:25%">
                                <input type="text" name="DMKNo" id="DMKNo" class="round input-form-textbox"/>
                            </td>
                            <td align="right" style="width:10%">
                                <label>Trade Date:</label>
                            </td>
                            <td align="left" style="width:25%">
                                <input type="text" name="tradedate" id="tradedate" class="round input-form-textbox"/>
                            </td>
                            <td align="right" style="width:10%">
                                <label>Effective Date:</label>
                            </td>
                            <td align="left" style="width:25%">
                                <input type="text" name="effdate" id="effdate" class="round input-form-textbox"/>
                            </td>
                            <td align="right" style="width:10%">
                                <label>Maturity Date:</label>
                            </td>
                            <td align="left" style="width:25%">
                                <input type="text" name="matdate" id="matdate" class="round input-form-textbox"/>
                            </td>     
                        </tr>
                        <tr>
                            <td align="right">
                                <label>OPICS NO:</label>                            
                            </td>
                            <td align="left">
                                <input type="text" name="OPICNo" id="OPICNo" class="round input-form-textbox"/>
                            </td>
                            <td align="right">
                                <label>Product:</label>                            
                            </td>
                            <td align="left">
                                <select id="product" name="product" class="round input-from-dropdownlist"></select>
                            </td> 
                            <td align="right">
                                <label>Counterparty:</label>
                            
                            </td>
                            <td align="left">
                                <select id="counterparty" name="counterparty" class="round input-from-dropdownlist"></select>
                            </td>
                            <td align="right">
                                <label>Instrument:</label>
                            </td>
                            <td align="left">
                                <select id="instrument" name="instrument" class="round input-from-dropdownlist"></select>
                            </td> 
                        </tr>
                        <tr>
                            <td align="right">
                                <label>Portfolio:</label>                            
                            </td>
                            <td align="left">
                                <select id="portfolio" name="portfolio" class="round input-from-dropdownlist"></select>
                            </td>   
                            <td align="right">
                                <label>User:</label>
                            </td>
                            <td align="left">
                                <select id="user" name="user" class="round input-from-dropdownlist"></select>
                            </td>
                            <td align="right">
                                <label>Status:</label>
                            </td>
                            <td align="left">
                                <select id="status" name="status" class="round input-from-dropdownlist"></select>
                            </td>   
                            <td align="right">
                                <label>OverwriteStatus:</label>
                            </td>
                            <td align="left">
                                <select id="overstatus" name="overstatus" class="round input-from-dropdownlist">
                                    <option value="">ALL</option>
                                    <option value="No">No</option>
                                    <option value="PCE">PCE</option>
                                    <option value="SET">SET</option>
                                    <option value="Yes">Yes</option>
                                </select> 
                            </td> 
                        </tr>
                        <tr>
                            <td align="right">
                                <label>Processing Date:</label>
                            </td>
                            <td align="left">
                                <input type="text" name="procdate" id="procdate" class="round input-form-textbox"/>
                            </td>
                            <td align="right">
                                <label>Settlement Limit:</label>
                            </td>
                            <td align="left">
                                <select id="settle" name="settle" class="round input-from-dropdownlist">
                                    <option value="">ALL</option>
                                    <option value="No">No</option>
                                    <option value="Yes">Yes</option>
                                </select> 
                            </td>
                        </tr>
                    </table>
                    <button type="submit" id="LoadRecordsButton" class="round blue button-submit">Load records</button>
                </form>
            </div>
        </div>
    </div>
    <div class="table-container">
        <div id="TableContainer">
        </div>
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
    <div id="message-dialog" style="display:none;">
            <label id="message" />
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $('#tradedate').datepicker({ dateFormat: "dd/mm/yy" });
            $('#effdate').datepicker({ dateFormat: "dd/mm/yy" });
            $('#matdate').datepicker({ dateFormat: "dd/mm/yy" });
            $('#procdate').datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", '<%= CurrentProcessDate %>');
            $('#product').combobox({
                    change: function (evt, ui) {
                    $('#product').combobox('value', $('#product').combobox('value'));
                }
            });
            $('#instrument').combobox({
                change: function (evt, ui) {
                    $('#instrument').combobox('value', $('#instrument').combobox('value'));
                }
            });
            $('#portfolio').combobox({
                change: function (evt, ui) {
                    $('#portfolio').combobox('value', $('#portfolio').combobox('value'));
                }
            });
            $('#counterparty').combobox({
                change: function (evt, ui) {
                    $('#counterparty').combobox('value', $('#counterparty').combobox('value'));
                }
            });
            $('#user').combobox({
                change: function (evt, ui) {
                    $('#user').combobox('value', $('#user').combobox('value'));
                    }
            });
            $('#status').combobox({
                change: function (evt, ui) {
                    $('#status').combobox('value', $('#status').combobox('value'));
                }
            }); 
            $('#overstatus').combobox({
                change: function (evt, ui) {
                    $('#overstatus').combobox('value', $('#overstatus').combobox('value'));
                }
            });
            $('#settle').combobox({
                change: function (evt, ui) {
                    $('#settle').combobox('value', $('#settle').combobox('value'));
                }
            });

            set_options('product', 'ALL', 'GetProductOptions', function () {
                     $('#product').combobox('value', '-1');
            });
            set_options('instrument', 'ALL', 'GetInstrumentOptions', function () {
                     $('#instrument').combobox('value', '-1');
            });
            set_options('portfolio', 'ALL', 'GetPortfolioOptions', function () {
                     $('#portfolio').combobox('value', '-1');
            });
            set_options('counterparty', 'ALL', 'GetCounterpartyOptions', function () {
                     $('#counterparty').combobox('value', '-1');
            });
            set_options('user', 'ALL', 'GetUserOptions', function () {
                     $('#user').combobox('value', '-1');
            });
            set_options('status', 'ALL', 'GetStatusOptions', function () {
                     $('#status').combobox('value', '-1');
            });
            
            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Trade Inquery List',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'DMK_NO ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("DealView.aspx/GetDealInquiryByFilter") %>'
                    <% if (Writable) { 
                        //Response.Write(", updateAction: '" + Page.ResolveClientUrl("DealView.aspx/CancelDeal") + "'"); 
                    } %>
                },
                toolbar: {
                    items: [{
                        icon: '../Images/excel_icon17x17.png',
                        text: '<a href="#" class="export">Export to Excel</a>',
                        click: function () {                        
                            window.open('<%= Page.ResolveClientUrl("../Report/ExportToCSV.aspx?reportName=DealView&") %>' + $("#tbForm input , #tbForm select").serialize(), '', 'width=500,height=200');
                        }
                    }]
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    LimitOverwrite: {
                        title: 'OW'
                        , width: '2%'
                        , display: function (data) {  

                            if (data.record.LimitOverwrite.toUpperCase() != 'NO') {
                                return $('<span style="background-color:red">' + data.record.LimitOverwrite + '</span>')
                            }
                            else {
                                return data.record.LimitOverwrite
                            }
                        }
                    },
                    LimitApprover: {
                        title: 'LimitApprover'
                        , visibility: 'hidden'
                    },
                    EntryDate: {
                        title: 'Entry Date'
                        , width: '10%'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    DMK_NO: {
                        title: 'DMK NO'
                        , width: '11%'
                    },
                    OPICS_NO: {
                        title: 'OPICS NO'
                        , width: '11%'
                    },
                    TradeDate: {
                        title: 'Trade Date'
                        , width: '10%'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    EffectiveDate: {
                        title: 'Effective Date'
                        , width: '10%'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    Instrument: {
                        title: 'Instrument'
                    },
                    MaturityDate: {
                        title: 'Limit End Date'
                        , width: '10%'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    BuySell: {
                        title: 'B/S'
                        , width: '3%'
                    },
                    Product: {
                        title: 'Product'
                        , width: '10%'
                    },
                    Portfolio: {
                        title: 'Portfolio'
                        //width: '11%',
                    },
                    Counterparty: {
                        title: 'CTPY'
                        //width: '11%',
                    },
                    Notional1: {
                        title: 'Notional'
                        , type: 'number'
                        , listClass: 'right'
                        , displayFormat: '2'
                    },
                    CCY1: {
                       title: 'Ccy1'
                        , visibility: 'visible'
                    },
                    PayRec1: {
                        title: 'PayRec1'
                        , visibility: 'hidden'
                    },
                    FixedFloat1: {
                        title: 'FixedFloat1'
                        , visibility: 'hidden'
                    },
                    Freq1: {
                       title: 'Freq1'
                        , visibility: 'hidden'
                    },
                    Fixing1: {
                        title: 'Fixing1'
                        , visibility: 'hidden'
                    },
                    Rate1: {
                        title: 'Rate1'
                        , visibility: 'hidden'
                    },
                    SwapPoint1: {
                        title: 'SwapPoint1'
                        , visibility: 'hidden'
                    },
                    Notional2: {
                        title: 'Notional'
                        , type: 'number'
                        , listClass: 'right'
                        , visibility: 'hidden'
                        , displayFormat: '2'
                    },
                    CCY2: {
                       title: 'Ccy2'
                        , visibility: 'hidden'
                    },
                    PayRec2: {
                        title: 'PayRec2'
                        , visibility: 'hidden'
                    },
                    FixedFloat2: {
                        title: 'FixedFloat2'
                        , visibility: 'hidden'
                    },
                    Freq2: {
                       title: 'Freq2'
                        , visibility: 'hidden'
                    },
                    Fixing2: {
                        title: 'Fixing2'
                        , visibility: 'hidden'
                    },
                    Rate2: {
                        title: 'Rate2'
                        , visibility: 'hidden'
                    },
                    SwapPoint2: {
                        title: 'SwapPoint2'
                        , visibility: 'hidden'
                    },
                    Trader: {
                        title: 'Trader'
                    },
                    OpicsTrader: {
                        title: 'OpicsTrader'
                        , visibility: 'hidden'
                    },
                    Status: {
                        title: 'Status'
                        , width: '5%'
                    },
                    KKContribute: {
                        title: 'PCE Amount'
                        , width: '12%'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    SettlementLimit: {
                        title: 'SET'
                    },
                    TBMA_SENT: {
                        title: 'TBMA'
                    },
                    Remark: {
                        title: 'Comment'
                        , visibility: 'hidden'
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
                                if (data.record && data.record.Status == 'OPEN') {
                                    return '<button title="Edit Record" class="jtable-command-button jtable-edit-command-button" onclick="editProduct(\'' + data.record.ID + '\',\'' + data.record.Product + '\' ); return false;"><span>Edit Record</span></button>';
                                }
                                else {return '';}
                            }
                        } ,CustomAction2: {
                            title: '',
                            width: '1%',
                            sorting: false,
                            create: false,
                            edit: false,
                            list: true,
                            display: function (data) {
                                if (data.record && data.record.Status == 'OPEN') {
                                    return '<button title="Delete Record" class="jtable-command-button jtable-delete-command-button" onclick="deleteProduct(\'' + data.record.ID + '\'); return false;"><span>Delete</span></button>';
                                }
                                else {return '';}
                            }
                        }
                    <% } %>
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.validationEngine();
                },
                //Validate form when it is being submitted
                formSubmitting: function (event, data) {
                    return data.form.validationEngine('validate');
                },
                //Dispose validation logic when form is closed
                formClosed: function (event, data) {
                    data.form.validationEngine('hide');
                    data.form.validationEngine('detach');
                },
                recordUpdated: function (event, data) {
                    if(data.serverResponse != null){
                    if( data.serverResponse.DealPairID !=''){
                         $('#TableContainer').jtable('updateRecord', {
                            record: {
                                  ID: data.serverResponse.DealPairID,                                                        
                                  Status: data.record.Status,
                                  Remark: data.record.Remark
                            },
                            clientOnly: true
                        });
                        }
                    }
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                if (ValidateForm()==true) {
                    $('#TableContainer').jtable('load', {
                        strDMKNo: $('#DMKNo').val(),
                        strOPICNo: $('#OPICNo').val(),
                        strProduct: $('#product').val(),
                        strCtpy: $('#counterparty').val(),
                        strPortfolio: $('#portfolio').val(),
                        strTradeDate: $('#tradedate').val(),
                        strEffDate: $('#effdate').val(),
                        strMatDate: $('#matdate').val(),
                        strInstrument: $('#instrument').val(),
                        strUser: $('#user').val(),
                        strStatus: $('#status').val(),
                        strOverStatus: $('#overstatus').val(),
                        strProcDate: $('#procdate').val(),
                        strSettleStatus: $('#settle').val()
                    });
                }
            });
            
            //ToolTip
            var cols = new Array('LimitOverwrite', 'LimitApprover', 'Entry Date', 'DMK NO', 'OPICS_NO'
                                , 'Trade Date', 'Effective Date', 'Instrument'
                                , 'Limit End Date', 'B/S', 'Product'
                                , 'Portfolio', 'Counterparty', 'Notional1', 'Ccy1'
                                , 'PayRec1', 'FixedFloat1', 'Freq1', 'Fixing1'
                                , 'Rate1', 'SwapPoint1', 'Notional2', 'Ccy2', 'PayRec2', 'FixedFloat2', 'Freq2'
                                , 'Fixing2', 'Rate2', 'SwapPoint2', 'Trader', 'OpicsTrader', 'Status', 'PCE Amount', 'SET', 'TBMA', 'Comment'); // header for outer columns
            $('table.jtable > tbody').on('mouseover', 'tr.jtable-data-row', function (e) {
                var trEle = $(this);
                var tdEle = trEle.children('td:not(:has("img,button,input"))');
                var titleStr = '';
                $.each(tdEle, function (index, ele) {
                    if(cols[index] != null){
                    titleStr += cols[index];
                    titleStr += ' = ';
                    titleStr += $(ele).text();
                    titleStr += (index < tdEle.length - 1 ? '<br>' : '');
                    }
                });
                trEle.children('td').eq(2).attr('alt', titleStr);
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
            track : true            
        });

        function editProduct(ID,Product){

         var path ='';

          switch(Product)
            {
            case 'FX SPOT':
                path =  'Deal/FXSpotEntryInfo.aspx?id='
                break;
            case 'FX FORWARD':
                path =  'Deal/FXForwardEntryInfo.aspx?id='
                break;
            case 'FX SWAP':
                path =  'Deal/FXSwapEntryInfo.aspx?id='
                break;          
            case 'SWAP':
                path =  'Deal/SwapEntryInfo.aspx?id='
                break;      
            case 'BOND':
                path =  'Deal/FIEntryInfo.aspx?id='
                break;             
            case 'REPO':
                path =  'Deal/RepoEntryInfo.aspx?id='
                break;          
            default:
                break;     
            }

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
                                                        url: '<%= Page.ResolveClientUrl("DealView.aspx/CancelDeal") %>'
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

        function ValidateForm() {
                 var mess = '';
                 mess = $("#procdate").val() == '' ? 'Please input processing date.' : mess;
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
    </script>
</asp:Content>
