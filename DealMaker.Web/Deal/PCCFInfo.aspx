<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="PCCFInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.PCCFInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Label:<input type="text" name="name" id="label" class="round input-form-textbox"/></label>
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
        
            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Pre-Settlement Limit Conversion Factor',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'LABEL ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("PCCFInfo.aspx/GetPCCFByFilter") %>'
                    <% if (Writable) {
                        Response.Write(", createAction: '" + Page.ResolveClientUrl("PCCFInfo.aspx/CreatePCCF") + "'");
                        Response.Write(", updateAction: '" + Page.ResolveClientUrl("PCCFInfo.aspx/UpdatePCCF") + "'");
                    } %>
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    LABEL: {
                        title: 'Label'
                    },
                    ISACTIVE: {
                        title: 'Active',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        defaultValue: 'true',
                        formText: 'Active'
                    },
                    C0D: {
                        title: '0D',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C1D: {
                        title: '1D',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    DEFAULT: {
                        title: 'Default',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C1: {
                        title: '1',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C2: {
                        title: '2',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C3: {
                        title: '3',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C4: {
                        title: '4',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C5: {
                        title: '5',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C6: {
                        title: '6',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C7: {
                        title: '7',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C8: {
                        title: '8',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C9: {
                        title: '9',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C10: {
                        title: '10',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C11: {
                        title: '11',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C12: {
                        title: '12',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C13: {
                        title: '13',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C14: {
                        title: '14',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C15: {
                        title: '15',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C16: {
                        title: '16',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C17: {
                        title: '17',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C18: {
                        title: '18',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C19: {
                        title: '19',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C20: {
                        title: '20',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    more20: {
                        title: '>20',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C21: {
                        title: '21',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C22: {
                        title: '22',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C23: {
                        title: '23',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    },
                    C24: {
                        title: '24',
                        width: '3%',
                        type: 'number',
                        displayFormat: '2'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="LABEL"]').addClass('validate[required]');
                    data.form.find('input[name="DEFAULT"],[name="C0D"],[name="C1D"]').number(true, 2);
                    data.form.find('input[name="more20"],[name="C1"],[name="C2"],[name="C3"],[name="C4"],[name="C5"]').number(true, 2);
                    data.form.find('input[name="C6"],[name="C7"],[name="C8"],[name="C9"],[name="C10"],[name="C11"]').number(true, 2);
                    data.form.find('input[name="C12"],[name="C13"],[name="C14"],[name="C15"],[name="C16"],[name="C17"]').number(true, 2);
                    data.form.find('input[name="C18"],[name="C19"],[name="C20"]').number(true, 2);
                    data.form.find('input[name="C21"],[name="C22"],[name="C23"],[name="C24"]').number(true, 2);
                    
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
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    label: $('#label').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
            
        });

    </script>
</asp:Content>
