<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="CountryReport.aspx.cs" Inherits="KK.DealMaker.Web.Report.CountryReport" %>
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
                    <label>Country:<select id="country" class="round input-from-dropdownlist"></select></label>
                    <label>Status:
                        <select id="status" class="round input-from-dropdownlist">
                            <option value="">ALL</option>
                            <option value="NORMAL">NORMAL</option>
                            <option value="THRESHOLD">THRESHOLD</option>
                            <option value="EXCEED">EXCEED</option>
                            <option value="EXPIRED">EXPIRED</option>
                        </select>                    
                    </label>
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
            
            set_options('country', 'ALL', 'GetCountryOptions');

            //Prepare jtable plugin
            var titleTable = 'Country Limit Report';
            $('#TableContainer').jtable({
                title: titleTable,
                paging: true,
                pageSize: 50,
                sorting: false,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("CountryReport.aspx/GetCountryReport") %>'
                },
                toolbar: {
                    items: [{
                        icon: '../Images/excel_icon17x17.png',
                        text: '<a href="#" class="export">Export to Excel</a>',
                        click: function () {
                             if ($('#TableContainer').data('reportdate') != undefined && $('#TableContainer').data('reportdate') != ""
                                    && $('#TableContainer').data('reporttype') != undefined
                                    && $('#TableContainer').data('country') != undefined && $('#TableContainer').data('country') != "") {
                                window.open('<%= Page.ResolveClientUrl("ExportToCSV.aspx?reportName=Country&") %>' + 'strReportDate=' + $('#TableContainer').data('reportdate') 
                                                                                                                + '&strTitle=' + $('#tableTitle').text()
                                                                                                                + '&strReportType=' + $('#TableContainer').data('reporttype')
                                                                                                                + '&strCountry=' + $('#TableContainer').data('country')
                                                                                                                + '&strStatus=' + $('#TableContainer').data('status'), '', 'width=500,height=200');
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
                    COUNTRY_LABEL: {
                        title: 'Country'
                    },
                    EXPIRE_DATE: {
                        title: 'Expired Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    GEN_AMOUNT: {
                        title: 'Limit Amount'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    TEMP_AMOUNT: {
                        title: 'Temp Amount'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    PCE_CONTRIBUTE: {
                        title: 'PCE Utilization'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    SET_CONTRIBUTE: {
                        title: 'SET Utilization'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    FLOW_DATE: {
                        title: 'Utilization Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    AVAILABLE: {
                        title: 'Available'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    STATUS: {
                        title: 'Status'
                    }
                },
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#tableTitle').text($('#reporttype option:selected').text() + ' Country Limit Report');
                $('#TableContainer').jtable('load', {
                    strReportDate: $('#reportdate').val(),
                    strReportType: $('#reporttype').val(),
                    strCountry: $('#country').val(),
                    strStatus: $('#status').val()
                });
                $('#TableContainer').data('reportdate', $('#reportdate').val());
                $('#TableContainer').data('reporttype', $('#reporttype').val());
                $('#TableContainer').data('country', $('#country').val());
                $('#TableContainer').data('status', $('#status').val());
            });

        });

    </script>
</asp:Content>
