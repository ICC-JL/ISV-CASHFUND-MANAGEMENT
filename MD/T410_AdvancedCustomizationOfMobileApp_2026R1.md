# T410 Advanced Customization of the Mobile App

**Developer Course - Customization**

**2026 R1**

**Revision:** 4/9/2026

---

## Contents

- Copyright
- How to Use This Course
- Preparing the Environment
- Part 1: Configuring a Screen
  - Lesson 1.1: Adding Fields and Actions
  - Lesson 1.2: Configuring the Layout of the Screen
  - Lesson 1.3: Configuring Related Containers
- Part 2: Redirecting the User to Different Containers and Screens
  - Lesson 2.1: Finding Object Names in the WSDL Schema
  - Lesson 2.2: Making Use of a Redirection That Is Implemented in Acumatica ERP
  - Lesson 2.3: Mapping a Smart Panel
- Part 3: Advanced Procedures
  - Lesson 3.1: Adding a URL Link on a Mobile App Screen
  - Lesson 3.2: Implementing Multiple Redirections
  - Lesson 3.3: Map an Action Using Workflow API

---

## Copyright

(C) 2026 Acumatica, Inc.

ALL RIGHTS RESERVED.

No part of this document may be reproduced, copied, or transmitted without the express prior consent of Acumatica, Inc.

3075 112th Avenue NE, Suite 200, Bellevue, WA 98004, USA

### Restricted Rights

The product is provided with restricted rights. Use, duplication, or disclosure by the United States Government is subject to restrictions as set forth in the applicable License and Services Agreement and in subparagraph (c)(1)(ii) of the Rights in Technical Data and Computer Software clause at DFARS 252.227-7013 or subparagraphs (c)(1) and (c)(2) of the Commercial Computer Software-Restricted Rights at 48 CFR 52.227-19, as applicable.

### Disclaimer

Acumatica, Inc. makes no representations or warranties with respect to the contents or use of this document, and specifically disclaims any express or implied warranties of merchantability or fitness for any particular purpose. Further, Acumatica, Inc. reserves the right to revise this document and make changes in its content at any time, without obligation to notify any person or entity of such revisions or changes.

### Trademarks

Acumatica is a registered trademark of Acumatica, Inc. HubSpot is a registered trademark of HubSpot, Inc. Microsoft Exchange and Microsoft Exchange Server are registered trademarks of Microsoft Corporation. All other product names and services herein are trademarks or service marks of their respective companies.

**Software Version:** 2026 R1

**Last Updated:** 04/09/2026

---

## How to Use This Course

The T410 Advanced Customization of the Mobile App course introduces extended functionality to customize the Acumatica mobile app by using the Acumatica Customization Platform.

The course is intended for developers who are learning how to customize the Acumatica mobile app or other Acumatica Framework-based mobile apps.

> This training course includes rather complex examples of customizing the mobile app. In case you cannot reproduce them, we advise you to have a developer support plan.

After you complete the course, you will have an understanding of how to perform advanced customization tasks of the Acumatica mobile app or other apps developed with the Acumatica Framework. Upon completion of the course, you will have learned how to perform the following types of customization activity:

- Mapping a new screen from the beginning of the process
- Organizing the UI elements on a screen
- Configuring redirection from one screen to another
- Using form customization to customize the mobile app

### What the Course Prerequisites Are

Before you begin this course, we recommend that you work through the lessons of the T400 Basic Customization of the Mobile App course. To learn more about the functionality of the Acumatica Customization Platform, we recommend that you complete the T190 Quick Start in Customization or W140 Customization Projects courses before completing the current course.

### What Is in a Part

Each of the parts of the course introduces you to various ways to customize the Acumatica mobile app and consists of lessons you are to complete. Each part begins with an explanation of the subject area that you are going to use in the lessons.

### What Is in a Lesson

Each lesson includes procedures you should complete, and describe the related concepts that you should learn.

Each lesson ends with the Lesson Summary topic, which summarizes the concepts you have learned during the lesson.

### Where the Source Code Is

You can find the source code of the customization described in this course and code snippets for the course in the MobileDevelopment\\T410 folder of the Help-and-Training-Examples repository in Acumatica GitHub.

Code snippets with code in the Mobile Site Map Definition Language (MSDL) have the JSON extension only so that the syntax can be highlighted.

### What the Documentation Resources Are

The complete documentation for Acumatica ERP and the Acumatica Framework is available at https://help.acumatica.com/ and is included in the Acumatica ERP instance. While viewing any form used in the course, you can click Open Help in the top pane of the Acumatica ERP screen to bring up a form-specific Help menu; you can use the links on this menu to quickly access form-related information and activities and to open a reference topic with detailed descriptions of the form elements.

### Which License You Should Use

For the educational purposes of this course, you use Acumatica ERP under the trial license, which does not require activation and provides all available features. For the production use of the Acumatica ERP functionality, an administrator has to activate the license the organization has purchased. Each feature may be subject to additional licensing; please consult the Acumatica ERP licensing policy for details.

---

## Preparing the Environment

In this training, you will learn how to customize the Acumatica mobile app. Before you start the practical steps of the course, you need to prepare the development environment, as described in Preparation of the Development Environment below, and consider the recommendations described in Required Role for Users.

### Preparation of the Development Environment

Before you install Acumatica ERP, review System Requirements for the Acumatica ERP Installation, and prepare the environment, as described in Preparing for Installing Acumatica ERP in the same guide.

You need to deploy an instance of Acumatica ERP with a company that contains specific data that you will use for the training.

To prepare the development environment, perform the following actions:

> You can use the Acumatica ERP instance you used to pass the T400 Basic Customization of the Mobile App training course.

1. Install Acumatica ERP, as described in Acumatica ERP Installation On-Premises: To Install the Acumatica ERP Configuration Wizard.
2. In the Acumatica ERP Configuration wizard, which appears after the installation is complete, select Deploy a New Acumatica ERP Instance to create a local instance of Acumatica ERP for the development environment.
3. On the Database Configuration page, create a new database for the development environment.
4. To populate the database with the data needed for the training course, on the Tenant Setup page, set up a tenant with the SalesDemo data inserted by specifying the following settings:
   - **Tenant Name:** MyCompany
   - **New:** Selected
   - **Insert Data:** SalesDemo
   - **Parent Tenant ID:** 1
   - **Visible:** Selected
5. On the Instance Configuration page, as the Local Path of the Instance, specify a path that is not in the C:\\Program Files (x86) or C:\\Program Files folder to avoid an issue with permission to work in these folders. For example, you can enter C:\\AcumaticaSites\\T410.

The system creates a new Acumatica ERP instance, adds a new company, and loads the selected data.

6. Use the following initial credentials to sign in to the new company:
   - **User Name:** admin
   - **Password:** setup
   
   Change the password when the system prompts you to do so.

7. Make sure your instance of Acumatica ERP can be accessed from other devices in your local network. To do this, on a computer in your local network, try to open the following webpage: `http://<COMPUTER_NAME>/<INSTANCE_NAME>` or `https://<COMPUTER_NAME>/<INSTANCE_NAME>`, where `<COMPUTER_NAME>` is the name of your computer in your local network and `<INSTANCE_NAME>` is the name of the Acumatica ERP instance you have installed. For more information, see To Access an Acumatica ERP Instance Running Locally from the Acumatica Mobile App.

8. If the Sign-In page of Acumatica ERP opens, the instance is accessible in your local network.

> You can run the ipconfig command in the Command Prompt program of the computer that is running the Acumatica ERP instance to find its IP address.

### Course Materials on GitHub

You can clone or download the course materials from the Help-and-Training-Examples repository in Acumatica GitHub to a folder on your computer. The course materials include the customization project and code snippets resulting from the course activities, and are located in the MobileDevelopment\\T410 folder of the repository. By using these materials, you can copy code instead of entering it manually and compare your resulting project with the one located on GitHub.

Code snippets with code in the Mobile Site Map Definition Language (MSDL) have the JSON extension only so that the syntax can be highlighted.

### Required Role for Users

Specialists who will work on customization projects should be assigned the **Customizer** role in the application instance that is to be customized and tested, as well as in the production application that should be updated with the customization project. With this role assigned, the specialists can use the customization tools and facilities of the Acumatica Customization Platform, and upload and publish customization projects.

A user with the Administrator role can assign the Customizer role to the needed users by using the Users (SM201010) form.

The admin account has already been assigned the Administrator role, so you do not need to assign the Customizer role to this account. In a production environment, however, you need to assign the Customizer role to all developers who will work on customization projects.

### Related Links

- Users (SM201010)
- To Assign the Customizer Role to a User Account
- User Access Rights for Customization

---

## Part 1: Configuring a Screen

In this part, you will add a screen that corresponds to the Invoices (SO303000) form and map it from the ground up. This process includes the following actions:

- Mapping the screen's fields and actions to the mobile screen
- Organizing the fields of the screen into a layout
- Configuring headers and groups for the fields
- Configuring tabs and data tabs

### Preparing the Mobile Site Map

You need to prepare the customization project before you start configuring the Invoices screen for the mobile app. This screen corresponds to the Invoices (SO303000) form in Acumatica ERP. You then add the Invoices screen to the mobile site map.

#### Step: Prepare the Mobile Site Map

To prepare the customization project and add the Invoices screen to the mobile site map, do the following:

1. In your Acumatica ERP instance, create a new customization project, and open it in the Customization Project Editor.
2. On the Mobile Application page, add the Invoices screen to the mobile app by using the Add New Screen button. This screen has the SO303000 identifier.
   
   Do not map any fields or actions. You will do it in later lessons.

3. On the Mobile Application page of the Customization Project Editor, update the mobile site map so that the app contains a tile for the Invoices screen. Click Update Sitemap on the More menu (under Actions), and add the necessary code on the Update: SITEMAP page. Save your changes.
4. Publish the customization project.
5. On the Mobile Workspaces (AU220012) form, add the Invoices screen to the Sales Orders workspace and save your changes to the customization project.

The Mobile Application page should contain two items.

**Related Links:**
- To Add a Screen to the Mobile Site Map (Example)
- To Update the Site Map of a Mobile App
- To Manage the Workspaces of the Mobile App

---

### Lesson 1.1: Adding Fields and Actions

In this lesson, you will start configuring the Invoices screen by adding to it fields that are shown on the screen and actions that can be performed there. After reviewing information on the types of actions, you will add some fields and actions. Finally, you will test the implemented screen on a mobile device.

#### Types of Actions

An action can be performed in the following types of objects or groups of MSDL objects:

- container
- list
- record
- selection

Depending on which object the action is applied to, MSDL includes the following MSDL objects:

- **containerAction:** Applied to the whole container
- **listAction:** Applied to the whole list of records
- **recordAction:** Applied to the opened record
- **selectionAction:** Applied to the items selected in a list

You can learn what actions are defined for a form by exploring the WSDL schema of the form. However, the WSDL schema does not indicate whether the action is applied to a container, a list, the opened record, or selected items. You should understand what the action is applied to by learning what the desired action on a form does.

For example, the Save and Cancel actions are applied to the record as a whole, so you should map them as recordAction objects. On the other hand, the Insert action adds a new record to the container, so you should map the Insert action as a containerAction object. The container actions are defined inside the corresponding containers.

> You must declare the Cancel action for all screens that include it in the WSDL schema. Without the Cancel action mapped, the changes discarded in the mobile app might not be discarded on the server.

#### Step 1: Add Fields to the Invoices Screen

To add fields to the Invoices screen, do the following:

1. Open the WSDL schema of the Invoices (SO303000) form. Find and explore the following complex types:
   - InvoiceSummary
   - Details
   - Taxes
   
   You will map elements from these complex types later in this lesson.

2. In the navigation pane of the Customization Project Editor, click Add SO303000 to open the Add SO303000 page.
3. In the Script area of the page, add the containers and fields that correspond to the complex types and elements you discovered earlier, as shown in the following code.

```msdl
add container "InvoiceSummary" {
  add field "Customer"
  add field "Date"
  add field "Type"
  add field "Status"
  add field "Description"
  add field "Amount"
  add field "Balance"
  add field "DocumentDiscounts"
  add field "FreightTotal"
  add field "TaxTotal"
}
add container "Details" {
  containerActionsToExpand = 1
  add field "Branch"
  add field "OrderNbr"
  add field "Warehouse"
  add field "Quantity"
  add field "UnitPrice"
  add field "Account"
  add field "Subaccount"
}
add container "Taxes" {
  add field "TaxID"
  add field "TaxRate"
  add field "TaxableAmount"
  add field "TaxAmount"
}
```

The container corresponding to the Summary area of the form is called the **primary container** (in this case, InvoiceSummary), and the containers corresponding to form tabs are called **secondary containers** (in this case, Details and Taxes). If a secondary container contains a grid, you can map it by adding a corresponding container, as shown in the code above. Adding secondary containers with fields will be explored in the next lesson.

Notice that the Details container contains the `containerActionsToExpand` attribute, which specifies how many container actions will be visible in the bottom part of the list view. For details, see container.

4. Save your changes, and publish your customization project.
5. Open the Acumatica mobile app, and sign in to your instance. In the Sales Orders workspace, tap the Invoices tile to make sure that the Invoices screen is mapped.

#### Step 2: Add Standard Actions to the Invoices Screen

In this step, you will first add mandatory actions (Save and Cancel) and the Insert and Release actions to the Invoices screen. To map these actions, do the following:

1. In the WSDL schema, learn the titles of the actions you want to add to the screen.
2. In the navigation pane of the Customization Project Editor, click Add SO303000 to open the Add SO303000 page.
3. In the Script area of the page, add the actions to the InvoiceSummary container, as shown in the following code.

```msdl
add container "InvoiceSummary" {
  # fields declaration
  ...
  add recordAction "Save" {
    behavior = Save
  }
  add recordAction "Cancel" {
    behavior = Cancel
  }
  add containerAction "Insert" {
    behavior = Create
  }
  add recordAction "Release" {
    behavior = Record
  }
}
```

In the code above, you have added actions and specified the behavior of each action. The possible values for the behavior attribute are listed in containerAction and recordAction.

> For actions that represent long-running operations (such as the Release action added in the preceding code), the system automatically detects and runs these actions asynchronously.

You will discover how to match an action on a form and its name in the WSDL schema in the next lessons.

4. Save your changes and publish your customization project.

#### Step 3: Test the Invoices Screen

In this step, you will test the mapped screen and actions. Do the following:

1. In the browser version of your Acumatica ERP instance, open the Invoices (SO303000) form.
2. Create an invoice with at least one detail line:
   1. On the Invoices form, create a new record.
   2. In the Customer box, select AACustomer.
   3. On the Details tab, click Add SO Line.
   4. In the Add SO Line dialog box which opens, select the SO006020 sales order.
   5. Click Add & Close.
   6. Save your changes.
   
   The status of the invoice is Balanced and the Release command is available on the form toolbar.

3. Open the Acumatica mobile app, and sign in to your instance. In the Sales Orders workspace, tap Invoices. The Invoices screen opens.

The actions you mapped have different appearances on the mobile screen. The Save button appears when you modify something in an invoice record. The Release command appears on the screen toolbar with an opened invoice that has not been released yet. The Cancel button corresponds to the Close Screen command. The Insert button appears in the list view of the Invoices screen.

4. Try to modify the invoice you created in Instruction 2. For example, change the Date value to the next day.
5. On the screen toolbar, tap Release.

The invoice is released and the invoice's status is changed to Open.

#### Lesson Summary

In this lesson, you have added a number of essential fields to the Invoices screen and learned how to map different types of actions.

**Related Links:**
- MSDL

---

### Lesson 1.2: Configuring the Layout of the Screen

By default, all fields mapped to a mobile screen are displayed one after another. It may not be very convenient, however, for all of the fields to be in a list on a single screen. MSDL offers the functionality to organize the fields on a screen in a more user-friendly way.

In this lesson, you will organize the fields and containers of the Invoices screen. First, you will create a header for the screen; then you will put multiple fields in a group. Finally, you will organize different kinds of tabs on the screen.

#### Step 1: Organize the Fields of the InvoiceSummary Container

To organize multiple fields of one container, you can use a layout object and define the fields within the object. To display the fields of the InvoiceSummary container in a header, do the following:

1. On the Mobile Application page of the Customization Project Editor, open the Add SO303000 page.
2. In the Script area, add the following code in place of the field definitions of the InvoiceSummary container.

```msdl
add layout "InvoiceHeader" {
  layout = "HeaderSimple"
  add layout "InvoiceHeaderRow1" {
    layout = "Inline"
    add field "Type"
    add field "Date"
  }
  add layout "InvoiceHeaderRow2" {
    layout = "Inline"
    add field "Status"
    add field "Amount"
  }
  add layout "InvoiceHeaderRow3" {
    layout = "Inline"
    add field "Customer"
  }
  add layout "InvoiceHeaderRow4" {
    layout = "Inline"
    add field "Description"
  }
}
```

Several fields you mapped before have been left outside of the header. You will organize them in the next step.

In this code, you have first defined a layout object of a HeaderSimple type (the InvoiceHeader object), and then in the InvoiceHeader layout object, you have defined layout objects of the Inline type that correspond to rows. You have added to each Inline layout object the fields that you want to be displayed in the same row.

3. Save your changes, and publish your customization project.
4. Open the Acumatica mobile app, and sign in to your instance. In the Sales Orders workspace, tap the Invoices tile and open any invoice.

Check the fields that you defined in the InvoiceSummary container.

> The fields in the header are compactly displayed and, therefore, disabled. If you want the fields to be enabled for editing, you should map them outside the header.

#### Step 2: Group the Fields

You can put multiple fields in an expanding group, regardless of the container from which the fields originate. To explore this functionality, you will add fields from the FinancialDetailsLinkToGL container and configure a group for them. Do the following:

1. On the Mobile Application page of the Customization Project Editor, open the Add SO303000 page.
2. In the Script area, add the fields of the FinancialLinkToGL container to the InvoiceSummary container, as shown in the following code.

The fields are located inside a group object. This group should be able to be collapsed and expanded and is collapsed by default.

```msdl
add group "FinancialDetails" {
  displayName = "Financial Details"
  collapsable = True
  collapsed = True
  add field "FinancialLinkToGL#ARAccount"
  add field "FinancialLinkToGL#Branch"
  add field "FinancialLinkToGL#ARSubaccount"
}
```

In the code above, you have defined the FinancialDetails group and added fields from the FinancialLinkToGL container to this group. The FinancialDetails group is located inside the InvoiceSummary container, and inside the group you need to access fields of the FinancialLinkToGL container. To access fields from a different container, you compose a string consisting of the container name, the `#` sign, and the field name.

3. Save your changes, and publish your customization project.
4. Open the Acumatica mobile app, and sign in to your instance. In the Sales Orders workspace, tap the Invoices tile.

Verify that the Financial Details group appears, and open it. By default, this group is collapsed because you used the `collapsed = True` attribute.

#### Step 3: Organize the Secondary Containers with Grids of the Invoices Screen

If you have multiple fields or containers on a screen, you can display them as tabs. Do the following to organize the previously defined fields and containers into tabs:

1. On the Mobile Application page of the Customization Project Editor, open the Add SO303000 page.
2. In the Script area, add the following code to the InvoiceSummary container.

```msdl
add layout "Other" {
  layout = Tab
  add field "FreightTotal"
  add field "TaxTotal"
}
add layout "Details" {
  layout = DataTab
  add containerLink "Details" {
    control = "ListItem"
  }
}
add layout "Taxes" {
  layout = DataTab
  add containerLink "Taxes" {
    control = "ListItem"
  }
}
```

This code unites two fields of the InvoiceSummary container into one tab; for two other tabs, it contains links to two previously defined containers, Details and Taxes.

> Tabs that contain links to containers should be of the DataTab type.

3. Save your changes, and publish your customization project.
4. Open the Acumatica mobile app, and sign in to your instance. In the Sales Orders workspace, tap the Invoices tile.

On the Invoices screen, the added tabs should look as shown in the following screenshot.

> If any fields or groups are not placed in the header layout or the tabs (as is the case for this screen), they are placed in the Summary tab, which is created automatically.

#### Lesson Summary

In this lesson, you have learned how to configure different types of layouts on a mobile app screen.

**Related Links:**
- layout
- group

---

### Lesson 1.3: Configuring Related Containers

In previous lessons, you have learned the following ways of using containers that are related to each other:

- Declaring a secondary container and creating a link to it within a data tab
- Displaying fields from one container in another container

In the browser version of Acumatica ERP, you can often see dialog boxes (smart panels) where the user can select multiple items to be used with the record selected on the form (for example, to add lines to a sales order). In this lesson, you will learn how to map a dialog box with a single table to the Acumatica mobile app. In the WSDL schema, this type of dialog box is represented by a many-to-one container -- that is, a container that supports selection of one item or option or multiple items or options. So, to map such dialog box, you use the container object.

> To map a smart panel which has more complex layout than a simple table, you need to use the dialog object. You will map a smart panel with a complex layout in Lesson 2.3: Mapping a Smart Panel.

On the Invoices (SO303000) form, you could use the Add Order dialog box that opens when a user clicks the Add Order button of the Details tab. The dialog box is used to add sales orders to the invoice. In this lesson, you will map the action that opens the dialog box and the dialog box itself.

#### Step 1: Learn the Names of Entities for Mapping

In this step, you will learn the names of the following entities which will be later used for mapping:

- The dialog box
- The action that opens the dialog box
- The action which adds a line from the dialog box

Do the following:

1. Find and explore the description of the Add Order dialog box in the WSDL schema of the Invoices (SO303000) form. The dialog box corresponds to the AddOrder complex type.
2. Find the name of the action associated with the Add Order button on the Details tab (which a user clicks to open the Add Order dialog box) as follows:
   1. Open the Invoices form.
   2. On the form title bar, click Settings > Inspect Element.
   3. On the table toolbar of the Details tab, click Add Order.
   4. In the Element Properties dialog box, which is opened, notice the Action Name value: **SelectShipment**.
   
   This value is the name of the action in the WSDL schema.
   
   To verify this, open the WSDL schema and explore the Action complex type. The SelectShipment action is present.

3. The Add Order dialog box contains three buttons: Add, Add & Close, and Cancel. You need to map the Add button so that a user can add a selected order to the table on the Details tab. To find the name of the action that is associated with this button, do the following:
   1. Open the Invoices form.
   2. On the table toolbar of the Details tab, click Add Order. The Add Order dialog box opens.
   3. Hold down Ctrl+Alt and click the Add button.
   4. In the Element Properties dialog box, which is opened, notice the Action Name value: **AddShipment**.
   
   This value is the name of the action in the WSDL schema.
   
   To verify this, open the WSDL schema and explore the Action complex type. The AddShipment action is present.

#### Step 2: Configure a Screen with Many-to-One Containers

In this step, you will configure the Add Order screen of the mobile app by using the AddOrder container. This screen will provide the same functionality of the Add Order dialog box on the Invoices form. You will also add the action that opens this mobile screen and the action that adds an order to the Details tab.

Do the following:

1. On the Mobile Application page of the Customization Project Editor, open the Add SO30300 page.
2. In the Script area, add the following code that maps the Add Order dialog box to the mobile app.

```msdl
add container "AddOrder" {
  type = SelectionActionList
  visible = False
  listActionsToExpand = 1
  add field "Selected" {
    special = ListSelection
  }
  add field "OrderType"
  add field "OrderNbr"
  add field "Location"
  add listAction "AddShipment" {
    icon = "system://Check"
    behavior = Void
  }
}
```

In the code above, you have added the AddOrder container. You have enabled selection of multiple items in the container by setting the container type to SelectionActionList. Inside the container, you have added an action of the listAction type that gives a user the ability to add orders that they select in the AddOrder container. You found the name of this action in Instruction 3 of the previous step.

> The corresponding action of the first listAction or selectionAction object that you add to a container will be displayed as a button in the bottom part of the screen if you have set listActionsToExpand = 1 for this container.

Note that for the Selected field, you specify `special = ListSelection` so that a user can select the record on the screen by tapping it. Otherwise, a user will have to perform an extra step: open the order, and switch on the Selected toggle. For details, see field in MSDL Reference.

3. In the Details container, add the action that opens a separate mobile screen that displays the contents of the AddOrder container, as shown in the following code.

```msdl
add containerAction "SelectShipment" {
  behavior = Void
  redirect = True
  redirectToContainer = "AddOrder$List"
}
```

Note that the name of the action should be the one you found in Instruction 2 of the previous step. The `redirectToContainer = "AddOrder$List"` instruction causes the AddOrder container to be opened as a list.

4. Save your changes, and publish your customization project.

#### Step 3: Test the Action and the Add Order Screen

To test Add Order action and the Add Order screen in the mobile app, do the following:

1. In Acumatica ERP, do the following to enter the data that will be displayed on the Add Order screen of the mobile app during the testing:
   1. In your Acumatica ERP instance, open the Sales Orders (SO301000) form.
   2. Create a sales order with at least one SO line:
      1. On the Sales Orders form, create a new record.
      2. In the Customer box, select AACustomer.
      3. On the Details tab, click Add Items. The Inventory Lookup dialog box opens.
      4. In the Inventory Lookup dialog box, select the first inventory item (AAComput01).
      5. Click Add & Close.
      6. The added inventory item is added to the Details tab.
   3. On the form toolbar, click Create Shipment.
   4. In the Specify Shipment Parameters dialog box, which is opened, click OK.
   5. The Shipments (SO302000) form opens.
   6. On the form toolbar, click Confirm Shipment. The status of the shipment changes to Confirmed.
   7. On the Invoices (SO303000) form, create an invoice for the same customer.
   8. On the Details tab, click Add Order. Make sure the sales order you created is displayed in the Add Order dialog box. Close the dialog box.
   9. Save your changes.

2. Open the Acumatica mobile app, and sign in to your instance. In the Sales Order workspace, tap the Invoices tile to open the Invoices screen.
3. Open the invoice that you have created in Instruction 1. The invoice should have the Balanced status so that you can update it.
4. Open the Details tab of the invoice.
5. Tap the Add Order button.
6. On the screen that opens, select a sales order and tap the Add button.
7. Return to the Invoices screen, make sure that you can see the added order, and save the invoice.

#### Lesson Summary

In this lesson, you have learned how to configure a many-to-one container opened from another container.

**Related Links:**
- containerAction

---

## Part 2: Redirecting the User to Different Containers and Screens

You can redirect the user to different screens and containers in the Acumatica mobile app in one of the following ways:

- Allow a redirection that is already implemented in Acumatica ERP
  - You will learn how to do it in Lesson 2.2: Making Use of a Redirection That Is Implemented in Acumatica ERP.
- Create a new redirection
  - You can implement a redirection to the following objects:
    - A container within the current screen. You have learned about this option in Lesson 1.2: Configuring the Layout of the Screen
    - Another screen. This redirection can be implemented by using form customization and is described in Lesson 3.1: Adding a URL Link on a Mobile App Screen

> You cannot redirect the user to a secondary container within another screen.

To be able to implement a redirection to another container or screen, you need to know the names of the actions that perform the redirection and the names of containers to which the redirection is performed. In Lesson 2.1: Finding Object Names in the WSDL Schema, you will learn several ways to learn name of entities in the WSDL schema.

---

### Lesson 2.1: Finding Object Names in the WSDL Schema

To map an object (a container, a field, or an action), you have to know its name in the WSDL schema of the form. The way you determine the object's name depends on the type of the object. In this lesson, you will learn how to find the names of the objects in the WSDL schema.

#### Step 1: Determine a Container's Name

The names of all of a form's containers are listed in the Content complex type of the WSDL schema.

The primary container of a form view usually has the following naming convention: `<FormTitle>Summary`. Sometimes the singular version of the form name is used in the container name instead of the plural version, which is used in the actual name. For example, for the Invoices (SO303000) form, the primary container is named InvoiceSummary in the WSDL schema. Sometimes the name of the primary container corresponds to the form name exactly, as it does for the Expense Receipts (EP301010) form, where the primary container's name is ExpenseReceipts.

The container of a tab, which is a secondary container, usually has the same name as the corresponding tab's name. For example, on the Invoices form, the container for the Details tab is named Details, the container for the Taxes tab is named Taxes, and the container for the Commissions tab is named Commissions.

Sometimes, the name of the container is different from the name of the corresponding tab. For example, the WSDL schema does not contain the Financial container for the Financial tab of the Invoices form.

To find the name of the container that corresponds to the Financial tab of the Invoices form, do the following:

1. Open the Invoices form.
2. Open the WSDL schema of the form.
3. In the WSDL schema, explore names of container in the Content complex type. It includes names for all tabs of the form.
4. In the Content complex type, find the container name that corresponds to the Financial tab.

In this case, the name of the container is not the same as the name of the tab; instead, the container name is **PaymentInformation**.

5. In the WSDL schema, find the PaymentInformation complex type. It contains the fields that you can map in the PaymentInformation container.

For a dialog box (which is also a secondary container) that opens when a user clicks a button or command corresponding to an action, the container usually has the same name as the dialog box. For example, the container of the Add Order dialog box (which is invoked on the Details tab of the Invoices form) is named AddOrder.

#### Step 2: Determine a Field's Name

The field name in the WSDL schema usually corresponds to the name of the field on the user interface of a form. But before you look for the field name, you should determine which container the field is in, because the same field name can exist in multiple containers.

To find the field name of the Date UI element on the Invoices (SO303000) form, do the following:

1. Open the Invoices form.
2. Open the WSDL schema of the form.
3. Determine the name of the primary container of the Invoices form in the WSDL schema.

Because you know that the Date UI element is located in the Summary area the form, you would need to first determine the name of the container that describes the Summary area of the Invoices form (the primary container). As you determined in Lesson 1.1, the name of the primary container is InvoiceSummary.

4. In the InvoiceSummary complex type of the WSDL schema, find the field name of the Date element in the WSDL schema.

#### Step 3: Determine an Action's Name

All the actions of a particular form are listed in the Actions complex type of the WSDL schema. You can find the name of a particular action there or learn it from the action's properties. You can view the action's properties by using the Element Inspector. You have already learned how to find the action name of the Add Order button by using the Element Inspector in Lesson 1.3: Configuring Related Containers.

#### Self-Guided Exercise: Find Object Names for Further Mapping

In Lesson 2.2, you will map the primary containers of the Vendors (AP303000) form and the Vendor Locations (AP303010) form, as well as the Add New Location action on the Locations tab of the Vendors form. Find the object names corresponding to the primary containers of the Vendors form and the Vendor Locations form, and the object name for the Add New Location button that adds a new vendor location to the Vendors form. This button is located on the toolbar of the Locations tab on the Vendors form. You should find these names in the WSDL schema by using the methods described above.

#### Lesson Summary

In this lesson, you have learned how to determine the name of objects (that is, containers, fields, and actions) in the WSDL schema.

**Related Links:**
- Getting the WSDL Schema

---

### Lesson 2.2: Making Use of a Redirection That Is Implemented in Acumatica ERP

On the Vendors (AP303000) form of Acumatica ERP, you can open the Vendor Locations (AP303010) form by clicking Add New Location on the table toolbar of the Locations tab. In this lesson, you will learn how to map this functionality to the mobile app.

First, as self-guided exercises, you will map the Vendors and Vendor Locations forms to add the corresponding screens to the mobile app. Then you will map an action that redirects the user from the Vendors screen to the Vendor Locations screen by using the logic of the Vendors form.

#### Step 1: Map the Vendors Form (Self-Guided)

In this step, you should do the basic mapping of the Vendors (AP303000) form: Add the Vendors screen and update the mobile site map, create the Payables workspace and add the Vendors screen to it. The Vendors screen should include all of the following:

- The VendorSummary container with some essential fields
- The Locations container with the LocationName and Active fields
- The Save and Cancel actions

#### Step 2: Map the Vendor Locations Form (Self-Guided)

In this step, you should do the basic mapping of the Vendor Locations (AP303010) form: Add the Vendor Locations screen and update the mobile site map. Note that when you are updating the mobile site map on the Update: SITEMAP page, the AP303010 item should have the visible attribute set to False because this screen should be opened only from the Vendors screen. You do not need to add the Vendor Locations screen to any workspace for the same reason.

The Vendor Locations screen should include the VendorLocationSummary container with some essential fields, and the standard Save and Cancel actions. Sample code for the screen is shown below.

```msdl
add screen AP303010 {
  openAs = Form
  add container "VendorLocationSummary" {
    add field "Vendor"
    add field "LocationID" {
      ForceType = String # to be able to enter new ID
    }
    add field "Status"
    add field "GeneralLocationInfo#LocationName"
    add field "NoteText"
    add recordAction "Save" {
      behavior = Save
    }
    add recordAction "Cancel" {
      behavior = Cancel
    }
  }
}
```

#### Step 3: Declare the Redirect Action

Now that you have mapped the Vendors (AP303000) and Vendor Locations (AP303010) forms, you should declare an action that redirects a user from the Vendors screen to the Vendor Locations screen as follows:

1. In Acumatica ERP, open the Vendors form.
2. On the table toolbar of the Locations tab, find the Add New Location button.

Learn the name of the corresponding action in the WSDL schema by inspecting the element. If you have completed the self-guided exercise in the previous lesson, you have already used the Element Inspector to determine the name of the action corresponding to the Add New Location button in the WSDL schema.

3. On the Mobile Application page of the Customization Project Editor, open the Add: AP303000 Vendors page, and add the action in the VendorSummary container, as shown in the following code.

```msdl
add recordAction "NewLocation" {
  displayName = "Add Location"
  behavior = Record
  redirect = True
}
```

You do not need to specify to which screen the action will redirect the user, because the default business logic of the Add New Location action includes this specification.

4. Save your changes, and publish your customization project.

#### Step 4: Test the Mapped Action

To test the Add Location action, do the following:

1. Open the Acumatica mobile app, and sign in to your instance. On the main menu, tap Payables > Vendors.
2. Select any vendor, and on the More menu, tap Add Location.

The Vendor Locations screen opens.

3. Specify details of the new location, and save your changes.

Now that you added the new location, you can open the Locations screen from the Vendor screen, tap the location and view its details.

#### Lesson Summary

In this lesson, you have learned how to map in the mobile app redirection from one screen to another when this redirection has already been implemented in Acumatica ERP.

**Related Links:**
- Displaying Any Field as a Text Field
- recordAction

---

### Lesson 2.3: Mapping a Smart Panel

In this lesson, you will map to the Acumatica mobile app a smart panel (that is, a complex dialog box) and an action that opens it.

In Lesson 1.3: Configuring Related Containers, you have mapped a smart panel that contains a single table by using just a container object. In this lesson, you will map a smart panel with a more complex layout. You will do this by using the dialog object, in which you will nest the container object.

> You need to nest the container object inside a dialog object when the smart panel has a complex layout (anything other than a simple table). For example, when a smart panel has a filter area. The dialog object also allows you to define logic of actions on the smart panel.

#### Mapping a Smart Panel

To map a smart panel, you use the dialog object. Inside the dialog object, you add the container that corresponds to the smart panel in the WSDL schema. To redirect a user to the dialog object, you define a redirectAction object and specify the name of the dialog object in the redirectToDialog property.

To map a smart panel by using the dialog object, you map the same basic objects:

- An action that opens the smart panel: You do this by mapping a recordAction object.
- A smart panel with fields from its different containers: You do this by adding a dialog object. Inside the dialog object, you add a container that corresponds to the smart panel. Inside the container object, you map fields from the smart panel.
- An action that closes the smart panel: You map such an action by doing the following:
  - Add a dialogAction object inside the dialog object. For details, see dialogAction.
  - Add a corresponding action object (recordAction, containerAction, selectionAction) inside the container of the smart panel. The action object should have the same name as the dialogAction object.
  - Specify includeDialogActions = true in the container where you add the action object (recordAction, containerAction, selectionAction).

> In case a smart panel initiates a long-running operation, and another dialog box is shown as the result of that operation, you can map only the original smart panel.

#### Customization Story

On the Sales Orders (SO301000) form, a user can specify the needed settings and run quick processing for an order by using the Process Order dialog box (smart panel). To open the Process Order dialog box, the user clicks the Quick Process button on the form toolbar. When the user clicks Process in the dialog box, the quick processing of the document is initiated. In this lesson, you will map the Quick Process button and smart panel on the Sales Orders form.

#### Step 1: Learn the Names of the Entities for Mapping

In this step, you will learn the names of the following entities which will later be used for mapping:

- The action that opens the Process Order dialog box
- The dialog box

On the Sales Orders (SO301000) form, do the following:

1. Investigate the quick processing on the form to learn which entities you will need to map:
   1. Add a sales order for the AACustomer customer, and add a line by doing the following:
      1. On the table toolbar of the Details tab, click Add Items.
      2. In the Add Items dialog box, which opens, select the AACOMPUT01 stock item (the first in the table).
      3. Click Add & Close.
   2. On the form toolbar, click Quick Process.
   
   The Process Order dialog box is shown.
   
   3. Review the containers on the dialog box. You will map fields from these containers later.
   4. Click Process.
   
   The long-running operation is initiated and the results are displayed in the Processing Results dialog box in the top-right corner of the form.
   
   > For the long-running operation to be initiated and completed, you will need to map a custom action that closes the Process Order dialog box.

2. By using the Element Inspector, learn the name of the action that corresponds to the Quick Process button: **QuickProcess**. Make sure that this is the name you can use for mapping by locating it in the WSDL schema.
3. Find and explore the description of the Process Order dialog box in the WSDL schema.

The Process Order dialog box includes multiple groups such as Shipping or Availability, that correspond to different containers in the WSDL schema. This means that you will need to map fields from these containers too. If you are using the Modern UI of the Sales Orders (SO301000) form, the corresponding WSDL schema unites the fields from the above groups into a single container: ProcessOrder_. However, for the purposes of this lesson, you will use the WSDL schema that corresponds to the Classic UI of the form, which still uses separate containers for the fields of each group.

In this lesson, you will map fields from the following containers: Availability, Shipping, and Invoicing. Fields that are not mapped will be assigned their default values.

#### Step 2: Map the Smart Panel

In this step, you will map the Process Order dialog box by using the names from the WSDL schema that you learned in the previous step. Do the following:

1. On the Mobile Application page of the Customization Project Editor, update the Sales Orders screen.

> If you are using the project from the T400 Basic Customization of the Mobile App training course, you can open the existing Update SO301000 Sales Orders page.

2. Map the Process Order dialog box by adding the dialog object inside the update screen 301000 instruction, as shown in the following code.

```msdl
add dialog QuickProcessD {
  add dialogAction "Process" {
    CloseDialog = True
    DialogResult = OK
    DisplayName = "Process"
  }
  openAs = Form
  add container "ProcessOrder" {
    includeDialogActions = true
    add field "WarehouseID"
    add field "ShipmentDate"
    add field "CustomDate"
    add field "ProcessOrderAvailability#GreenStatus"
    add field "ProcessOrderAvailability#YellowStatus"
    add field "ProcessOrderAvailability#RedStatus"
    add field "ProcessOrderAvailability#AvailabilityMessage"
    add field "ProcessOrderShipping#CreateShipment"
    add field "ProcessOrderShipping#PrintPickList"
    add field "ProcessOrderShipping#ConfirmShipment"
    add field "ProcessOrderShipping#PrintLabels"
    add field "ProcessOrderShipping#PrintShipmentConfirmation"
    add field "ProcessOrderShipping#UpdateIN"
    add field "ProcessOrderInvoicing#PrepareInvoicePrepareInvoiceFromShipment"
    add field "ProcessOrderInvoicing#PrepareInvoicePrepareInvoice"
    add field "ProcessOrderInvoicing#PrintInvoice"
    add field "ProcessOrderInvoicing#EmailInvoice"
    add field "ProcessOrderInvoicing#ReleaseInvoice"
    add recordAction "Process" {
      icon = "system://Check"
    }
  }
}
```

In the code above, first you have added the dialog object with the custom name QuickProcessD. Inside it, you have done the following:

- Added a dialog box action that saves changes to the dialog box (DialogResult = OK) and closes it (CloseDialog = True).
  
  > Since Acumatica 2023 R2, all actions are displayed in the screen menu by default. To display the action outside of the screen menu, you can specify FormActionsToExpand = 1 in the ProcessOrder container.

- Specified that the dialog box should be opened as a separate screen by specifying openAs = Form.
- Added a container that corresponds to the Process Order dialog box. Inside the container, you have mapped fields from its different groups. You have mapped the Process button (which saves changes in the Process Order dialog box) by adding the recordAction object to the ProcessOrder container.

3. Map the Quick Process button which opens the Process Order dialog box by adding the following recordAction object to the OrderSummary container.

```msdl
update container "OrderSummary" {
  add recordAction "QuickProcess" {
    redirect = true
    redirectToDialog = "QuickProcessD"
  }
}
```

4. Save your changes, and publish the customization project.

#### Step 3: Test the Mapped Smart Panel

To test the Process Order smart panel in the mobile app, do the following:

1. In Acumatica ERP, open the Sales Orders (SO301000) form and create a sales order for the AACustomer customer.
2. Add a line to the sales order by doing the following:
   1. On the toolbar of the Details tab, click Add Items.
   2. In the Add Items dialog box, which opens, select the AACOMPUT01 stock item (the first in the table).
   3. Click Add & Close.
3. Make sure that the Quick Process button highlighted in green appears on the form toolbar.
4. Save the sales order.
5. Open the Acumatica mobile app, and sign in to your instance. In the Sales Orders workspace, tap the Sales Orders tile.
6. Open the sales order that you have created in this step.
7. On the Sales Order screen toolbar, tap Quick Process.

The Process Order screen opens.

8. Review the settings, turn off the Update IN and Prepare Invoice toggles, and save your changes by tapping Process in the screen menu.

The long-running operation is performed, and the sales order is assigned the Completed status.

**Related Links:**
- Mapping a Smart Panel
- dialog
- dialogAction

---

## Part 3: Advanced Procedures

This part contains lessons that demonstrate the solving of some common issues when you are customizing the Acumatica mobile app. In these lessons, you will learn how to do the following:

- Add a URL link, as described in Lesson 3.1: Adding a URL Link on a Mobile App Screen
- Implement multiple redirections, as described in Lesson 3.2: Implementing Multiple Redirections
- Map an action to a mobile screen using Workflow API, as described in Lesson 3.3: Map an Action Using Workflow API

> Lesson 3.1 and Lesson 3.3 of this part require experience customizing Acumatica ERP forms. For details, see the Customization Guide and the training courses of the T series.

---

### Lesson 3.1: Adding a URL Link on a Mobile App Screen

It is impossible to add a URL link to a mobile app screen only by using MSDL unless the link already exists on the corresponding Acumatica ERP form because MSDL can be used to map only existing functionality of a form. Therefore, to have a link on a mobile app screen, you should first customize an Acumatica ERP form by adding a new action, and then map this action to the mobile screen.

In this lesson, you will customize the Invoices (SO303000) form by adding a toolbar button with a hyperlink, and then you will map this button to the Invoices mobile screen.

#### Step 1: Add a New Toolbar Button to the Invoices Form

To add a new toolbar button to the Invoices (SO303000) form of Acumatica ERP, do the following:

1. Using the Element Inspector, learn which graph described business logic on the Invoices form: **SOInvoiceEntry**.

You need to extend this graph to add a custom action.

> You can create a graph extension and add a new action by using the Code page in the Customization Project Editor or by using Visual Studio and an extension library.

2. Create a graph extension of the PX.Objects.SO.SOInvoiceEntry graph and add a new action to it as the following code shows.

> If you are using Visual Studio, use Acuminator to suppress the PX1016 error in a comment. In this activity, for simplicity, the extension is always active.

```csharp
public PXAction<PX.Objects.AR.ARInvoice> TestURL;
[PXButton]
[PXUIField(DisplayName = "Test URL")]
protected void testURL()
{
    // the body of the action delegate method
}
```

3. In the body of the action delegate method, add the following code, which redirects the user to the external URL.

```csharp
throw new PXRedirectToUrlException("http://www.acumatica.com",
    "Redirect:http://www.acumatica.com");
```

This code instructs the system to open http://www.acumatica.com in a new tab of the default browser. The code uses an exception instead of the usual methods, such as System.Diagnostics.Process.Start(), because web applications are normally forbidden to run these processes as opening external URLs.

4. If you are using the Code page in the Customization Project Editor, click SOInvoiceEntry in the table. The Custom Code page opens with the code of the graph extension. Remove all using statements except for `using PX.Data`.

If you are using Visual Studio, you can check which using statements are not necessary and remove them. For details, refer to the T190 Quick Start in Customization course.

5. Save your changes, and publish your customization project.
6. Open the Invoices form, and make sure that a new button has appeared on the form toolbar.

#### Step 2: Map the New Action to the Mobile App

Now that you have created the new TestURL action on the Invoices (SO303000) form, you will map it to the Invoices screen as follows:

1. Open the customized Invoices form.
2. Open the WSDL schema of the form.
3. Find the Actions complex type and the new TestURL action.
4. On the Mobile Application page of the Customization Project Editor, open the Add SO303000 page.
5. In the InvoiceSummary container, add the TestURL action, as shown in the following code.

```msdl
add recordAction "TestURL" {
  behavior = Void
  redirect = True
}
```

6. Save your changes and publish your customization project.
7. Open the Acumatica mobile app, and sign in to your instance. On the main menu, tap Sales Orders > Invoices, and create an invoice.
8. On the screen toolbar, open the menu, and tap Test URL.

The built-in browser with the Acumatica ERP main page opens.

#### Lesson Summary

In this lesson, you have learned how to add to a mobile app screen a link to an external URL. By using the described approach, you can implement any logic in a custom action and map it to the mobile app.

**Related Links:**
- To Add an Action
- recordAction

---

### Lesson 3.2: Implementing Multiple Redirections

Sometimes when multiple complex redirections are implemented in the mobile app, the Back button does not redirect the user properly to the previous screen. This can happen if redirections were not done correctly.

By using the procedure of adding purchase order lines (PO lines) to a purchase receipt on the Purchase Receipts screen, this lesson will show you how to correctly perform these redirections. The Purchase Receipts screen corresponds to the Purchase Receipts (PO302000) form in Acumatica ERP.

#### Step 1: Analyze the Procedure in the Browser Version of Acumatica ERP

To understand what you should map to the mobile app, you should first analyze the procedure of adding purchase order lines to a purchase receipt in Acumatica ERP. Do the following:

1. On the Purchase Orders (PO301000) form, create a purchase order with the Open status:
   1. On the Purchase Orders form, create a new record.
   2. In the Vendor box, select AAServices.
   3. On the Details tab, click Add Items. The Inventory Lookup dialog box opens.
   4. In the Inventory Lookup dialog box, select the line for the MGPEELPACK inventory ID with the WHOLESALE warehouse and click Add & Close.
   5. On the form toolbar, click Remove Hold. The purchase order is saved and gets the Pending Approval status.
   6. On the form toolbar, click Approve. The purchase order gets the Open status.

2. On the Purchase Receipts form, create a new receipt for the AAServices vendor.
3. On the table toolbar of the Details tab, click Add PO Line.

The Add Purchase Order Line dialog box opens. The dialog box contains the list of open purchase order lines, and you can select any number of lines to be added to the purchase receipt.

4. In the dialog box, select the unlabeled check box for each purchase order line you want to add, and click Add & Close.

The dialog box is closed, and the selected purchase order lines are added to the Details tab of the Purchase Receipts form.

5. Save your changes.

If you get an error that the Vendor Ref. box cannot be empty, enter any number in it.

6. Save the record.

Based on the scenario in which purchase order lines are added to a purchase receipt, in the mobile site map, you have to update the Purchase Receipts screen. (You do not have to create this screen from scratch because it is already mapped by default.) When you update the screen, you should map the Add PO Line button and the Add Purchase Order Line dialog box with its buttons.

#### Step 2: Update the Purchase Order Screen

Now you will map the actions and dialog box of the Purchase Receipts (PO302000) form to the Purchase Receipts screen as follows:

1. Open the Purchase Receipts (PO302000) form.
2. Open and analyze the WSDL schema of the form.
3. Find the internal names corresponding to the Add PO Line button, the Add Purchase Order Line dialog box, and the Add & Close button.

> You can use methods of determining WSDL schema names of form elements presented in Lesson 2.1: Finding Object Names in the WSDL Schema.

4. In the Customization Project Editor, open the Update: PO302000 Purchase Receipts page:
   1. On the Mobile Application page, click Update Existing Screen. The Update Existing Screen dialog box opens.
   2. In the Update Existing Screen dialog box, select the PO302000 screen.
   3. Click OK.
   
   The Update: PO302000 Purchase Receipts page opens.

5. Analyze the mapped objects in the Result Preview area.
6. Map the Add PO Line button, which will open the Add Purchase Order Line screen.

As you noticed in Instruction 3 while exploring the WSDL schema, it contains two actions that you need to map: AddPOOrderLine and AddPOOrderLine2. The first action corresponds to the Add PO Line button and opens the AddPOOrderLine container (which appears in the mobile app as the Add Purchase Order Line screen), and the second one saves the added lines and closes this container (the Add & Close button in the browser version).

Therefore, to add the Add PO Line button, which opens the Add Purchase Order Line screen, you should map AddPOOrderLine.

To make the Add PO Line button to appear in the Details container, to correspond to the UI of the browser version of the Purchase Receipts form (where the Add PO Line button is on the Details tab), add a container action in the Details container, as shown in the following code.

```msdl
update container "Details" {
  add containerAction "AddPOOrderLine" {
    Behavior=Void
    Redirect=true
    RedirectToContainer="AddPurchaseOrderLine$List"
  }
}
```

If you instead wanted the command to appear in the DocumentSummary container of the Purchase Receipts screen, you would add a recordAction object, as shown in the following code. (For details on types of actions, see Lesson 1.1: Adding Fields and Actions.)

```msdl
update screen PO302000 {
  update container "DocumentSummary" {
    add recordAction "AddPOOrderLine" {
      displayName = "Add PO Line"
      behavior = Void
      redirect = True
      redirectToContainer = "AddPurchaseOrderLine$List"
    }
  }
}
```

7. Map the Add Purchase Order Line dialog box by adding the AddPurchaseOrderLine container to the Purchase Receipts screen.

Within the container, declare the AddPOOrderLine2 action, which adds the selected lines to the Details container, as shown in the following code.

```msdl
add container "AddPurchaseOrderLine" {
  Type = SelectionActionList
  Visible = false
  add field "Selected"
  add field "OrderNbr"
  add field "InventoryID"
  add field "LineDescription"
  add field "OrderQty"
  add listAction "AddPOOrderLine2" {
    Behavior = Void
    displayName = "Add & Close"
    after = Close
  }
}
```

8. Save your changes and publish your customization project.

#### Step 3: Test the Changes in the Mobile App

Now that you have added the needed containers and actions, you can test the redirection on a mobile device as follows:

1. Open the Acumatica mobile app, and sign in to your instance. Open the Purchases workspace.
2. In the Purchases workspace, tap the Purchase Receipts tile.
3. On the Purchase Receipts screen, select the purchase receipt you created in Instruction 2 of Step 1.
4. On the Purchase Receipts screen which opens, scroll to the Details group and tap it.
5. On the Details screen, tap Add PO Line.

The Add Purchase Order Line screen opens. The title of the screen shows the number of selected lines.

6. On the Add Purchase Order Line screen, select a line or multiple lines that you want to add to the purchase receipt.
7. Open the More menu in the bottom part of the screen, and tap Add & Close.

> Note that the Add & Close command appears on the More menu because you did not specify the listActionsToExpand property for the AddPurchaseOrderLine container in Instruction 7 of Step 2.

The app returns to the Details screen with the selected line or lines added to the list.

8. Click the Back button to return to the Purchase Receipts screen.
9. On the Purchase Receipts screen, tap Save.

The purchase receipt is saved.

#### Lesson Summary

In this lesson, you have learned how to correctly map and configure multiple redirects to related containers.

---

### Lesson 3.3: Map an Action Using Workflow API

Workflow API provides capabilities to map an action to the mobile app from code.

In this lesson, you will learn how to map an action to the mobile app using Workflow API. You will map the Lock Budget and Unlock Budget actions of the Projects (PM301000) form.

> In the T400 Basic Customization of the Mobile App training course, you have mapped the same actions using the Actions page of the Customization Projects Editor, which is basically Workflow API with a frontend. If you are performing this lesson in the same project you created for the T400 Basic Customization of the Mobile App training course, you need to remove the customization of the Projects (PM301000) form from the Screens page.

In this lesson, you will modify the existing screen configuration where the action is already added. If the form where you want to map an action does not have an existing screen configuration, or if the required action has not been added to it, you can add it manually. For details on creating a screen configuration, see Preparing a Screen Configuration. For details on adding an action to the screen configuration, see Implementing Workflow Actions.

#### Process Overview

Mapping an action to the mobile screen consists of the following steps:

1. Extending the class where the workflow is defined.
2. Updating the screen configuration: In the screen configuration, you update the configuration of the action and call the IsExposedToMobile method for it.

#### Before You Proceed

If you are using an instance from the T400 Basic Customization of the Mobile App training course, you can skip this step.

Locking and unlocking of a project budget affects ability to edit the Original Budgeted Quantity, Unit Rate, and Original Budgeted Amount columns on the Revenue Budget and Cost Budget tabs of the Projects (PM301000) form. So to be able to test the Lock Budget and Unlock Budget commands in the mobile app, you need at least one of these tabs to be mapped in the mobile app. You can map the Revenue Budget tab by updating the Projects screen and using the following code.

```msdl
update screen PM301000 {
  add container "RevenueBudget" {
    add field "ProjectTask"
    add field "AccountGroup"
    add field "Description"
    add field "OriginalBudgetedQuantity"
    add field "UnitRate"
    add field "OriginalBudgetedAmount"
  }
}
```

To map a tab, you use the same container object as you use to map the Summary area of the form. The object should be located at the same level as the object for the Summary area of the form.

#### Step 1: Creating an Extension Library

In order to map an action using Workflow API, you need to have an extension library where you will customize the screen configuration for the form. For details on extension libraries, see To Create an Extension Library.

> You can skip this step if you created an extension library for performing Lesson 3.1: Adding a URL Link on a Mobile App Screen.

To create an extension library, do the following:

1. On the main menu of the Customization Project Editor, click Extension Library > Create New.
2. In the Project Name box of the Create Extension Library dialog box, which opens, enter **T410_Code**.

By default, the platform uses the App_Data\\Projects folder of the website as the parent folder for the solution project. If the website folder is outside of the C:\\Program Files (x86) and C:\\Program Files folders, we recommend that you not change it. Otherwise, we recommend that you specify a parent folder outside these folders to avoid an issue with permission to work in the C:\\Program Files (x86) and C:\\Program Files folders. For example, specify the following folder: C:\\AcumaticaSites\\T410.

3. Click OK to close the dialog box and start the process of creating the library.

The platform creates files of the extension library and the OpenSolution.bat batch file. Depending on the settings of your browser, the OpenSolution.bat file is saved either in the Downloads folder or in another location.

To open the extension library in Visual Studio, either execute the OpenSolution.bat file or open the location that you specified for the extension library and open the T410_Code.sln file that has been created inside it.

#### Step 2: Mapping an Action Using Workflow API

To map actions using Workflow API, do the following:

1. Learn the names of the Lock Budget and Unlock Budget actions in code by using the Element Inspector on the Projects (PM301000) form. They are lockBudget and unlockBudget.
2. Learn the name of the class where the workflow for the Projects form is defined:
   1. Learn the name of the graph that defines the logic of the Projects form using the Element Inspector. It is **ProjectEntry**.
   2. Open the extension library in Visual Studio.
   3. In the Solution Explorer, open the source code of Acumatica ERP that is located in the WebSite project of the library by navigating to the following node: `<Instance_Name>` > App_Data > CodeRepository > PX.Objects.
   4. The source code for the Projects form and its workflows is located in the PX.Objects.PM namespace.
   5. Open the PX.Objects.PM namespace.
   6. In the PX.Objects.PM namespace, locate the ProjectEntry.cs file. Near the file, notice the ProjecEntry_ApprovalWorkflow.cs and ProjectEntry_Workflow.cs files. In these files, workflows for the Projects form are located.
   
   > Workflows are usually defined in files whose names have the following structure: `<graph>_Workflow.cs` where graph is the name of the graph that defines logic of the form. The files with workflows are usually located at the same level as the graph files, or in the Workflow folder.
   
   7. Research the files. The ProjectEntry_Workflow class defines the default workflow for the Projects form, and the ProjecEntry_ApprovalWorkflow class defines an extension of the default workflow.
   
   In this lesson, you will customize the default workflow defined in the ProjectEntry_Workflow class.

3. Create an extension of the ProjectEntry_Workflow class:
   1. In the T410_Code project, create a new file based on the C# template with the following name: **ProjectEntry_CustomizedWorkflow.cs**.
   2. In the ProjectEntry_CustomizedWorkflow.cs file, add the following code that creates an extension of the ProjectEntry_Workflow class.

```csharp
using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.PM;

namespace T410_Code
{
    public class ProjectEntry_CustomizedWorkflow : PXGraphExtension<
        ProjectEntry_Workflow,
        ProjectEntry>
    {
        public override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<ProjectEntry,
                PMProject>());
        }
    }
}
```

In the code above, you have defined an extension of the ProjectEntry_Workflow class. As a second parameter of the extension, you have specified the graph of the Projects form. In the extension, you have overridden the `Configure(PXScreenConfiguration)` method, which initializes the screen configuration, and calls the `Configure(WorkflowContext)` method where you will update the workflow.

4. In the `Configure(WorkflowContext<ProjectEntry, PMProject> context)` method, add the following code that updates the lockBudget and unlockBudget actions in the screen configuration.

```csharp
protected virtual void Configure(WorkflowContext<ProjectEntry, PMProject> context)
{
    context.UpdateScreenConfigurationFor(screen => screen
        .WithActions(actions =>
        {
            actions.Update(
                g => g.lockBudget,
                a => a.IsExposedToMobile(true));
            actions.Update(
                g => g.unlockBudget,
                a => a.IsExposedToMobile(true));
        })
    );
}
```

In the code above, you update the configuration of the lockBudget and unlockBudget actions in the WithActions method. For each action, you call the IsExposedToMobile method to specify whether the action should be displayed in the mobile app.

5. For each of the assembly references with the PX prefix (such as PX.Common.dll) that are added to T410_Code project, make sure `<Private>False</Private>` is specified.

You can check this if you open the T410_Code.csproj file in text mode. You can also check the Copy Local property of a reference, which should be false.

6. Build the project.
7. Include the extension library to the customization project. For details, see To Add a Custom File to a Project.
8. Publish the customization project.

#### Step 3: Testing the Mapped Actions

To test the Lock Budget and Unlock Budget commands in the mobile app, do the following:

1. Open the mobile app, and tap Projects. The Projects workspace opens.
2. In the Projects workspace, tap Projects. The Projects list view opens.
3. Open the BUDGETBYM project.

In the form view, notice the Revenue Budget tab; also, on the screen menu, notice the new menu command (Lock Budget).

> The Lock Budget command may not be visible for some projects because locking of the project budget is not allowed, based on the project status. The Unlock Budget command is not visible for this record because the project budget is not locked.

4. Make sure the editing of the project budget is available as follows:
   1. On the Projects screen, tap Revenue Budget. The Revenue Budget screen opens with the list of balances.
   2. Tap a line.
   3. Make sure you can edit the Original Budgeted Quantity box.

5. In the Original Budgeted Quantity box, enter 7, and tap Update on the screen toolbar to save your changes.

The Revenue Budget screen opens with all affected values updated.

6. Return to the Projects screen and tap Save on the screen toolbar.
7. On the screen menu, tap Lock Budget.
8. On the Revenue Budget screen, open the edited line.
9. Make sure that the Original Budgeted Quantity, Unit Rate, and Original Budgeted Amount boxes are unavailable for editing.

10. Return to the Projects screen, and, on the screen menu, tap the Unlock Budget command, which is now available because the project budget has been locked.
11. On the Revenue Budget screen, open a line.
12. Make sure the editing of line details is available again.

#### Lesson Summary

In this lesson, you have learned how to map an action using Workflow API. You have created an extension library, an extension of a class that defines a workflow, and updated the screen configuration in this class.

**Related Links:**
- Screen Configuration: To Prepare a Screen Configuration with a Predefined Workflow
- To Create an Extension Library
- Activity 3.1.1: To Add an Action to an Existing Workflow
- To Add a Custom File to a Project
