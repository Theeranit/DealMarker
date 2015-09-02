<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="SpotRateMaster.aspx.cs" Inherits="KK.DealMaker.Web.Admin.SpotRateMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
 
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label >
                    Process Date:
                    <input type="text" name="processdate" id="processdate" class="round input-form-textbox"/></label>
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

            $('#processdate').datepicker({ dateFormat: "dd/mm/yy" });

            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Master: Spot Rate List',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'PROC_DATE DESC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("SpotRateMaster.aspx/GetByFilter") %>'
                    <% if (Writable) {
                        Response.Write(", createAction: '" + Page.ResolveClientUrl("SpotRateMaster.aspx/Create") + "'");
                        Response.Write(", updateAction: '" + Page.ResolveClientUrl("SpotRateMaster.aspx/Update") + "'");
                    } %>
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    CURRENCY_ID: {
                        title: 'CCY',
                        width: '45%',
                        options: '<%= Page.ResolveClientUrl("SpotRateMaster.aspx/GetCurrencyOptions") %>',
                        sorting: false
                    },
                    PROC_DATE: {
                        title: 'Process Date',
                        width: '45%',
                        type: 'date',
                        displayFormat: 'dd-M-yy'
                    },
                    RATE: {
                       title: 'Rate',
                       width: '10%'
                   }
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    processdate: $('#processdate').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
        });

    </script>
</asp:Content>
