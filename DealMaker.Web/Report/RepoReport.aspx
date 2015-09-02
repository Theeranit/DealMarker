<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="RepoReport.aspx.cs" Inherits="KK.DealMaker.Web.Report.RepoReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Report Date:<input type="text" name="reportdate" id="reportdate" class="round input-form-textbox"/></label>
                    <label>Report Type:
                        <select id="reporttype" class="round input-from-dropdownlist">
                            <option value="EXT">BOD</option>
                            <option value="">Intraday</option>
                        </select>                    
                    </label>
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
            var titleTable = 'Repo Report';
            $('#TableContainer').jtable({
                title: titleTable,
                paging: true,
                pageSize: 50,
                sorting: false,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("RepoReport.aspx/GetRepoReport") %>'
                },
                toolbar: {
                    items: [{
                        icon: '../Images/excel_icon17x17.png',
                        text: '<a href="#" class="export">Export to Excel</a>',
                        click: function () {
                            if ($('#TableContainer').data('reportdate') != undefined && $('#TableContainer').data('reportdate') != "" 
                                    && $('#TableContainer').data('reporttype') != undefined
                                    && $('#TableContainer').data('counterparty') != undefined && $('#TableContainer').data('counterparty') != "") {
                                var para = 'strReportDate=' + $('#TableContainer').data('reportdate')
                                            + '&strTitle=' + $('#tableTitle').text() 
                                            + '&strReportType=' + $('#TableContainer').data('reporttype') 
                                            + '&strCtpy=' + $('#TableContainer').data('counterparty');
                                window.open('<%= Page.ResolveClientUrl("ExportToCSV.aspx?reportName=Repo&") %>' + para, '', 'width=500,height=200');
                            }
                        }
                    }]
                },
                fields: {
                    PROCESSING_DATE: {
                        title: 'Report Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    SNAME: {
                        title: 'CTPY'
                    },
                    LIMIT_LABEL: {
                        title: 'Limit'
                    },
                    AMOUNT: {
                        title: 'Limit Amt'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    AVAILABLE: {
                        title: 'Available'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REV_AMOUNT: {
                        title: 'RRP'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_GOV_5_AMOUNT: {
                        title: 'RP_GOV_0-5'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_GOV_10_AMOUNT: {
                        title: 'RP_GOV_5-10'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_GOV_20_AMOUNT: {
                        title: 'RP_GOV_10-20'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_GOV_20s_AMOUNT: {
                        title: 'RP_GOV_20+'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_SOE_5_AMOUNT: {
                        title: 'RP_SOE_0-5'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_SOE_10_AMOUNT: {
                        title: 'RP_SOE_5-10'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_SOE_20_AMOUNT: {
                        title: 'RP_SOE_10-20'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    REP_SOE_20s_AMOUNT: {
                        title: 'RP_SOE_20+'
                        , type: 'number'
                        , listClass: 'right'
                    }
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#tableTitle').text($('#reporttype option:selected').text() + ' Repo Report');
                $('#TableContainer').jtable('load', {
                    strReportDate: $('#reportdate').val(),
                    strReportType: $('#reporttype').val(),
                    strCtpy: $('#counterparty').val()
                });
                $('#TableContainer').data('reportdate', $('#reportdate').val());
                $('#TableContainer').data('reporttype', $('#reporttype').val());
                $('#TableContainer').data('counterparty', $('#counterparty').val());
            });
        });
        
    </script>
</asp:Content>
