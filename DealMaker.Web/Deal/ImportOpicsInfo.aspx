<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="ImportOpicsInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.ImportOpicsInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Process Date:
                    <input type="text" name="processdate" id="processdate" class="validate[required] round input-form-textbox"/></label>
                    <input type="button" id="LoadRecordsButton" value="Import OPICS Deals" class="round blue button-submit" />
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

            $("#processdate").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", '<%= SessionInfo.Process.PreviousDate.ToString("dd/MM/yyyy") %>');
            $("#processdate").attr('disabled', true);

            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Imported OPICS Deals',
                paging: false,
                sorting: false,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("ImportOpicsInfo.aspx/ImportOPICSDeals") %>'
                },
                fields: {
                    Object: {
                        title: 'Object'
                    },
                    Total: {
                        title: 'Total imported'
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

        });

    </script>
</asp:Content>
