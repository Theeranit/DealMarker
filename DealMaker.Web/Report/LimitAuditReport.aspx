<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="LimitAuditReport.aspx.cs" Inherits="KK.DealMaker.Web.Report.LimitAuditReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
<div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Log Date:<input type="text" name="logdatefrom" id="logdatefrom" class="round input-form-textbox"/></label>
                    <label>to:<input type="text" name="logdateto" id="logdateto" class="round input-form-textbox"/></label>
                    <label>Counterparty:<select id="counterparty" class="round input-from-dropdownlist"></select></label>
                    <label>Country:<select id="country" class="round input-from-dropdownlist"></select></label>
                    <label>Limit Type:<select id="event" class="round input-from-dropdownlist"></select></label>
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

            $('#logdatefrom,#logdateto').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= CurrentProcessDate %>');

            set_options('counterparty', 'ALL', 'GetCounterpartyOptions');
            set_options('country', 'ALL', 'GetCountryOptions');
            set_options('event', 'ALL', 'GetLimitEventOptions');

            //Prepare jtable plugin
            var titleTable = 'Limit Audit Report';
            $('#TableContainer').jtable({
                title: titleTable,
                paging: true,
                pageSize: 50,
                sorting: false,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("LimitAuditReport.aspx/GetLimitAuditReport") %>'
                },
                toolbar: {
                    items: [{
                        icon: '../Images/excel_icon17x17.png',
                        text: '<a href="#" class="export">Export to Excel</a>',
                        click: function () {
                            if ($('#TableContainer').data('logdatefrom') != undefined && $('#TableContainer').data('logdatefrom') != "" && $('#TableContainer').data('logdateto') != undefined && $('#TableContainer').data('logdateto') != "") {
                                var para = 'strReportDate=' + $('#TableContainer').data('logdatefrom')+ '&strReportDateto=' + $('#TableContainer').data('logdateto') + '&strCtpy=' + $('#counterparty').val() + '&strCountry=' + $('#country').val() + '&strEvent=' + $('#event').val();
                                window.open('<%= Page.ResolveClientUrl("ExportToCSV.aspx?reportName=LimitAudit&") %>' + para, '', 'width=500,height=200');
                            }
                        }
                    }]
                },
                fields: {
                    LOG_DATE_STR: {
                        title: 'Log Date',
                        width: '12%',
                    },
                    ENTITY: {
                        title: 'Entity'
                    },
                    LIMIT: {
                        title: 'Limit Type'
                    },
                    USER: {
                        title: 'User'
                    },
                    DETAIL: {
                        title: 'Detail',
                        width: '58%',
                         display: function (data) {
                                return data.record.DETAIL.replace(/; /g,"<br>");
                            }
                    }
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    strLogDatefrom: $('#logdatefrom').val(),
                    strLogDateto: $('#logdateto').val(),
                    strCtpy: $('#counterparty').val(),
                    strCountry: $('#country').val(),
                    strEvent: $('#event').val()
                });
                $('#TableContainer').data('logdatefrom', $('#logdatefrom').val());
                $('#TableContainer').data('logdateto', $('#logdateto').val());
            });
            //$('#LoadRecordsButton').click();
        });
        
    </script>
</asp:Content>

