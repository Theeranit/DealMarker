<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="FactorConfigMaster.aspx.cs" Inherits="KK.DealMaker.Web.Admin.FactorConfigMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">

    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label >
                    Label:
                    <input type="text" name="name" id="name" class="round input-form-textbox"/></label>
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
                title: 'Master: PCCF Config List',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'LABEL ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/GetConfigByFilter") %>',
                    createAction: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/CreateConfig") %>',
                    updateAction: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/UpdateConfig") %>'
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    //CHILD TABLE DEFINITION FOR "FACTOR"
                    FACTOR: {
                        title: 'No.',
                        width: '5%',
                        sorting: false,
                        create: false,
                        edit: false,
                        display: function (FactorData) {
                            //Create an image that will be used to open child table
                            var $img = $('<img src="../images/list_metro.png" title="Edit Factor configuration" />');
                            //Open child table when user clicks the image
                            $img.click(function () {
                                $('#TableContainer').jtable('openChildTable',
                                    $img.closest('tr'), //Parent row
                                    {
                                    title: FactorData.record.LABEL + ' - Factors',
                                    actions: {
                                        listAction: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/GetFactorByFilter?ID=' + FactorData.record.ID + '") %>',
                                        createAction: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/CreateFactor") %>',
                                        updateAction: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/UpdateFactor") %>'
                                    },
                                    fields: {
                                        PCCF_CONFIG_ID:{
                                            type: 'hidden',
                                            defaultValue: FactorData.record.ID
                                        },
                                        ID: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        TABLE: {
                                            title: 'Table',
                                            options: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/GetTableOptions") %>',
                                            inputClass: 'validate[required]'
                                        },
                                        ATTRIBUTE: {
                                            title: 'Factor',
                                            dependsOn: 'TABLE',
                                            options: function (data) {
//                                                if (data.source == 'list') {
//                                                    return '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/GetColumnOptions?tablename=DA_TRN") %>';
//                                                }
                                                return '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/GetColumnOptions?tablename=' + data.dependedValues.TABLE + '") %>'; 
                                            },
                                            inputClass: 'validate[required]'
                                        },
                                        VALUE: {
                                            title: 'Value',
                                            inputClass: 'validate[required]'
                                        },
                                        ISACTIVE: {
                                            title: 'Status',
                                            type: 'checkbox',
                                            values: { 'false': 'Inactive', 'true': 'Active' },
                                            defaultValue: 'true',
                                            formText: 'Active'
                                        }
                                    },
                                    //Initialize validation logic when a form is created
                                    formCreated: function (event, data) {
                                        data.form.validationEngine();
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                            });
                            //Return image to show on the person row
                            return $img;
                        }
                    },
                    LABEL: {
                        title: 'Label',
                        inputClass: 'validate[required]'
                    },
                    DESCRIPTION: {
                        title: 'Description'
                    },
                    PRODUCT_ID: {
                        title: 'Product Name',
                        width: '20%',
                        options: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/GetProductOptions") %>',
                        inputClass: 'validate[required]'
                    },
                    PCCF_ID: {
                        title: 'PCCF',
                        width: '20%',
                        options: '<%= Page.ResolveClientUrl("FactorConfigMaster.aspx/GetPCCFOptions") %>',
                        inputClass: 'validate[required]'
                    },
                    PRIORITY: {
                        title: 'Priority'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        defaultValue: 'true',
                        formText: 'Active'
                    }
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
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    name: $('#name').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
        });

    </script>

</asp:Content>
