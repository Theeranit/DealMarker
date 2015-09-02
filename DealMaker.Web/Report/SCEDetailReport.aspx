<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="SCEDetailReport.aspx.cs" Inherits="KK.DealMaker.Web.Report.SCEDetailReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label>
                    Report Date:
                    <input type="text" name="reportdate" id="reportdate" class="round input-form-textbox"/></label>
                    <label>Report Type:
                        <select id="reporttype" class="round input-from-dropdownlist">
                            <option value="EXT">BOD</option>
                            <option value="">Intraday</option>
                        </select>                    
                    </label>
                    <label>Counterparty:<select id="counterparty" class="round input-from-dropdownlist"></select></label>
                    <label>Product:<select id="product" name="product" class="round input-from-dropdownlist"></select></label>
                    <button type="submit" id="LoadRecordsButton" class="round blue button-submit">
                    Load records</button>
                </form>
            </div>
        </div>
    </div>
    <div class="table-container">
        <div id="TableContainer">
        </div>
    </div>
           
    <script type="text/javascript">

        $(document).ready(function () {

            $('#reportdate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= CurrentProcessDate %>');

            set_options('counterparty', 'ALL', 'GetCounterpartyOptions');
            set_options('product', 'ALL', 'GetProductOptions');

            //Prepare jtable plugin
            var titleTable = 'Settlement Limit Detailed Report';
            $('#TableContainer').jtable({
                title: titleTable,
                paging: true,
                pageSize: 50,
                sorting: false,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("SCEDetailReport.aspx/GetSCEDetailReport") %>'
                },
                toolbar: {
                    items: [{
                        icon: '../Images/excel_icon17x17.png',
                        text: '<a href="#" class="export">Export to Excel</a>',
                        click: function () {
                            if ($('#TableContainer').data('reportdate') != undefined && $('#TableContainer').data('reportdate') != ""
                                && $('#TableContainer').data('reporttype') != undefined
                                && $('#TableContainer').data('counterparty') != undefined && $('#TableContainer').data('counterparty') != ""
                                && $('#TableContainer').data('product') != undefined && $('#TableContainer').data('product') != "") {
                                window.open('<%= Page.ResolveClientUrl("ExportToCSV.aspx?reportName=SCEDetail&") %>' + 'strReportDate=' + $('#TableContainer').data('reportdate')
                                                                                                                        + '&strTitle=' + $('#tableTitle').text()
                                                                                                                        + '&strReportType=' + $('#TableContainer').data('reporttype')
                                                                                                                        + '&strCtpy=' + $('#TableContainer').data('counterparty')
                                                                                                                        + '&strProduct=' + $('#TableContainer').data('product'), '', 'width=500,height=200');
                            }
                        }
                    }]
                },
                fields: {
                    EngineDate: {
                        title: 'Report Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    DMK_NO: {
                        title: 'DMK NO'
                    },
                    OPICS_NO: {
                        title: 'OPICS NO'
                    },
                    Product: {
                        title: 'Product'
                    },
                    Instrument: {
                        title: 'Instrument'
                    },
                    Counterparty: {
                        title: 'Counterparty'
                    },
                    Notional1: {
                        title: 'Notional'
                    },
                    TradeDate: {
                        title: 'Trade Date'
                        , type: 'date'
                        ,displayFormat: 'dd-M-yy'
                    },
                    EffectiveDate: {
                        title: 'Effective Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    MaturityDate: {
                        title: 'Maturity Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    Leg: {
                        title: 'Leg'
                    },
                    Seq: {
                        title: 'Seq'
                    },
                    CashflowRate: {
                        title: 'Rate'
                    },
                    CashflowDate: {
                        title: 'Flow Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    CashflowAmount: {
                        title: 'Flow Amount'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    KKContribute: {
                        title: 'Utilization'
                        , type: 'number'
                        , listClass: 'right'
                    }
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#tableTitle').text($('#reporttype option:selected').text() + ' Settlement Limit Detailed Report');
                $('#TableContainer').jtable('load', {
                    strReportDate: $('#reportdate').val(),
                    strReportType: $('#reporttype').val(),
                    strCtpy: $('#counterparty').val(),
                    strProduct: $('#product').val()
                });
                $('#TableContainer').data('reportdate', $('#reportdate').val());
                $('#TableContainer').data('reporttype', $('#reporttype').val());
                $('#TableContainer').data('counterparty', $('#counterparty').val());
                $('#TableContainer').data('product', $('#product').val());
            });

        });

    </script>
</asp:Content>
