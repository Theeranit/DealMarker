<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="LimitOverwriteReport.aspx.cs" Inherits="KK.DealMaker.Web.Report.LimitOverwriteReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
<div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Date:<input type="text" name="reportdate" id="reportdate" class="round input-form-textbox"/></label>
                    <label>Counterparty:<select id="counterparty" class="round input-from-dropdownlist"></select></label>
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
            //Prepare jtable plugin
            var titleTable = 'Limit Overwrite Report';
            $('#TableContainer').jtable({
                title: titleTable,
                paging: true,
                pageSize: 50,
                sorting: false,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("LimitOverwriteReport.aspx/GetLimitOverwriteReport") %>'
                },
                toolbar: {
                    items: [{
                        icon: '../Images/excel_icon17x17.png',
                        text: '<a href="#" class="export">Export to Excel</a>',
                        click: function () {
                            if ($('#TableContainer').data('reportdate') != undefined && $('#TableContainer').data('reportdate') != "") {
                                window.open('<%= Page.ResolveClientUrl("ExportToCSV.aspx?reportName=LimitOverwrite&") %>' + 'strReportDate=' + $('#TableContainer').data('reportdate') + '&strCtpy=' + $('#TableContainer').data('ctpy'), '', 'width=500,height=200');
                            }
                        }
                    }]
                },
                fields: {
                    EngineDate: {
                        title: 'Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy',
                        width: '7%'
                    },
                    Trader: {
                        title: 'Trader',
                        width: '7%'
                    },
                    LimitApprover: {
                        title: 'Approver',
                        width: '7%'
                    },
                    Remark: {
                        title: 'Comment',
                        width: '7%'
                    },
                    DMK_NO: {
                        title: 'DMK NO',
                        width: '7%'
                    },
                    Counterparty: {
                        title: 'Counterparty',
                        width: '7%'
                    },
                    Product: {
                        title: 'Product',
                        width: '7%'
                    },
                    Instrument: {
                        title: 'Instrument',
                        width: '7%'
                    },
                    Notional1: {
                        title: 'Notional'
                        , type: 'number'
                        , listClass: 'right',
                        width: '7%'
                    },
                    CCY1: {
                        title: 'Currency',
                        width: '6%'
                    },
                    KKContribute: {
                        title: 'Utilization (PCE)'
                        , type: 'number'
                        , listClass: 'right',
                        width: '10%'
                    },
                    LimitOverwrite: {
                        title: 'Over Limit',
                        width: '7%'
                    },
                    LimitOverAmount: {
                        title: 'Over amount',
                        width: '23%'
                    }
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    strReportDate: $('#reportdate').val(),
                    strCtpy: $('#counterparty').val()
                });
                $('#TableContainer').data('reportdate', $('#reportdate').val());

                $('#TableContainer').data('ctpy', $('#counterparty').val());
            });
            //$('#LoadRecordsButton').click();
        });
        
    </script>
</asp:Content>

