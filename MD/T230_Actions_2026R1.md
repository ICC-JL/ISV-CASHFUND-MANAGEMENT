Developer Course
Customization
T230 Actions
2026 R1
Revision: 4/8/2026


Contents | 2
Contents
Copyright...............................................................................................................................................3
How to Use This Course.......................................................................................................................... 4
Company Story and Customization Description........................................................................................ 6
Initial Configuration............................................................................................................................... 9
To Deploy an Instance for the Training Course................................................................................................9
Lesson 1: Defining an Action and the Associated Button for a Form...........................................................11
Action Definition: General Information.......................................................................................................... 11
Activity 1.1: To Define an Action for a Form...................................................................................................13
Lesson 2: Defining an Action and the Associated Button for a Table.......................................................... 17
Activity 2.1: To Define an Action for a Table...................................................................................................17
Lesson 3: Implementing an Asynchronous Operation.............................................................................. 21
Asynchronous Operations: General Information........................................................................................... 21
Activity 3.1: To Implement an Asynchronous Operation...............................................................................25
Appendix: Initial Configuration..............................................................................................................30


Copyright | 3
Copyright
© 2026 Acumatica, Inc.
ALL RIGHTS RESERVED.
No part of this document may be reproduced, copied, or transmitted without the express prior consent of
Acumatica, Inc.
3075 112th Avenue NE, Suite 200, Bellevue, WA 98004, USA
Restricted Rights
The product is provided with restricted rights. Use, duplication, or disclosure by the United States Government is
subject to restrictions as set forth in the applicable License and Services Agreement and in subparagraph (c)(1)(ii)
of the Rights in Technical Data and Computer Soware clause at DFARS 252.227-7013 or subparagraphs (c)(1) and
(c)(2) of the Commercial Computer Soware-Restricted Rights at 48 CFR 52.227-19, as applicable.
Disclaimer
Acumatica, Inc. makes no representations or warranties with respect to the contents or use of this document, and
specifically disclaims any express or implied warranties of merchantability or fitness for any particular purpose.
Further, Acumatica, Inc. reserves the right to revise this document and make changes in its content at any time,
without obligation to notify any person or entity of such revisions or changes.
Trademarks
Acumatica is a registered trademark of Acumatica, Inc. HubSpot is a registered trademark of HubSpot, Inc.
Microso Exchange and Microso Exchange Server are registered trademarks of Microso Corporation. All other
product names and services herein are trademarks or service marks of their respective companies.
Soware Version: 2026 R1
Last Updated: 04/08/2026


How to Use This Course | 4
How to Use This Course
The T230 Actions training course shows you how to define various types of actions by using Acumatica Framework
and the customization tools of Acumatica ERP.
Actions in the application code are associated with buttons on form toolbars and table toolbars and
with commands on the More menu in Acumatica ERP.
The course is intended for application developers who are starting to learn how to customize Acumatica ERP.
The course is based on a set of examples that demonstrate the general approach to customizing Acumatica ERP.
The course is designed to give you ideas about how to develop your own embedded applications through the
customization tools. It demonstrates the implementation of actions and the customization of existing actions of
Acumatica ERP, as well as the implementation of the commands and buttons on the UI that are associated with
these actions. As you go through the course, you will continue the development of the customization for the cell
phone repair shop, which was performed in the T200 Maintenance Forms, T210 Customized Forms and Master-Details
Relationships, and T220 Data Entry and Setup Forms training courses (which we recommend that you take before
completing the current course).
Aer you complete all the lessons of the course, you will be familiar with the programming techniques used to
define and customize actions in Acumatica ERP.
We recommend that you complete the examples in the presented order because some examples use
the results of previous ones.
What the Course Prerequisites Are
To complete this course, you should be familiar with the basic concepts of Acumatica Framework. Also, we
recommend that you complete the T200 Maintenance Forms, T210 Customized Forms and Master-Details
Relationships, and T220 Data Entry and Setup Forms training courses before you begin this course.
To complete the course successfully, you should have the following knowledge:
•
Proficiency with C#, including:
•
Class structure
•
Object-oriented programming (inheritance, interfaces, and polymorphism)
•
Usage and creation of attributes
•
Generics
•
Delegates, anonymous methods, and lambda expressions
•
Knowledge of the following concepts of ASP.NET and web development:
•
Application states
•
The debugging of ASP.NET applications by using Visual Studio
•
The process of attaching to IIS by using Visual Studio debugging tools
•
Familiarity with the basics of Node.js
•
Understanding of the Model-View-ViewModel (MVVM) pattern
•
Basic knowledge of JavaScript
•
Understanding of TypeScript concepts, such as interface, type, classes, enum, generic, and union
•
Knowledge of HTML fundamentals
•
Knowledge of debugging in a browser
•
Experience with SQL Server, including:


How to Use This Course | 5
•
Writing and debugging complex SQL queries (WHERE clauses, aggregates, and subqueries)
•
Understanding the database structure (primary keys, data types, and denormalization)
•
The following experience with IIS:
•
Configuring and deploying ASP.NET websites
•
Configuring and securing IIS
What Is in a Lesson
Each lesson focuses on a particular development scenario that you can implement by using Acumatica ERP
customization tools and the Acumatica Framework. Each lesson consists of a brief description of the scenario and
an example of its implementation.
Where the Source Code Is
You can find the source code of the customization described in this course and code snippets for the course in the
Customization\T230 folder of the Help-and-Training-Examples repository in Acumatica GitHub.
What the Documentation Resources Are
The complete documentation for Acumatica ERP and the Acumatica Framework is available at https://
help.acumatica.com/ and is included in the Acumatica ERP instance. While viewing any form used in the course, you
can click Open Help in the top pane of the Acumatica ERP screen to bring up a form-specific Help menu; you can
use the links on this menu to quickly access form-related information and activities and to open a reference topic
with detailed descriptions of the form elements.
Which License You Should Use
For the educational purposes of this course, you use Acumatica ERP under the trial license, which does not require
activation and provides all available features. For the production use of the Acumatica ERP functionality, an
administrator has to activate the license the organization has purchased. Each feature may be subject to additional
licensing; please consult the Acumatica ERP licensing policy for details.


Company Story and Customization Description | 6
Company Story and Customization Description
In this course, you will continue the development to support the Smart Fix company’s cell phone repair shop; you
began this development while completing the T200 Maintenance Forms, T210 Customized Forms and Master-Details
Relationships, and T220 Data Entry and Setup Forms training courses.
In the T200 Maintenance Forms training course, you’ve created two simple maintenance forms for the Smart Fix
company:
•
Repair Services (RS201000), which contains a list of the company's repair services
•
Serviced Devices (RS202000), which lists the devices that can be serviced
In the T210 Customized Forms and Master-Details Relationships course, you’ve created another maintenance form,
Services and Prices (RS203000), and customized the Stock Items (IN202500) form of Acumatica ERP. On the Services
and Prices form, users can define and maintain the price for each provided repair service. You’ve customized the
Stock Items form to mark particular stock items as repair items—that is, items used for repair services.
In the T220 Data Entry and Setup Forms course, you’ve created the following forms:
•
The Repair Work Orders (RS301000) data entry form, where repair shop employees can create and manage
work orders for repairs
•
The Repair Work Order Preferences (RS101000) setup form, which an administrative user uses to specify the
company’s preferences for repair work orders
In To Deploy an Instance for the Training Course, you’ll load and publish the customization project that
contains the results of these courses.
In the T230 Actions course, on the Repair Work Orders form, you’ll implement commands on the More menu and
buttons on the form toolbar, as well as buttons on the table toolbars of tabs of this form. You’ll also implement the
underlying actions. By using these commands and buttons, users will be able to assign a work order to themselves
and update the prices of repair and labor items.
On a tab of the Services and Prices form, you will implement a button on the table toolbar, as well as the underlying
action. This action will execute a long-running operation. By clicking the button, users will be able to validate the
prices for the repair items for the selected repair service by using an external service.
•
In Acumatica ERP, actions in the application code are associated with buttons on form
toolbars and table toolbars and commands on the More menu.
•
As you develop the button that users can click to validate repair item prices from an external
service, you’ll learn how to execute its underlying action asynchronously, which can be a time-
consuming operation. However, this course doesn’t cover the process of connecting to an
actual external service to validate the prices.
Buttons on the Repair Work Orders and Services and Prices Forms
In this course, you will add the following buttons and commands to the Repair Work Orders (RS301000) form and
implement the underlying actions:
•
The Assign to Me button on the form toolbar and command on the More menu.
By clicking this button or command, a user can assign a repair work order to themselves—that is, cause the
system to insert their username into the Assignee box. The button and command will be available until the
repair work order status is changed to Assigned, so it will be available when the repair work order has the On
Hold or Ready for Assignment status.
•
The Update Prices button on the table toolbar of the Repair Items and Labor tabs.


Company Story and Customization Description | 7
If the price of a repair item or labor item has been changed on the Services and Prices form, a user will be
able to update this item's price on the Repair Work Orders form. The user can click this button on the Repair
Items tab for a repair item or on the Labor tab for a labor item. The Update Prices button will be available
until an invoice has been created for the selected repair work order.
The following screenshots show how the Repair Work Orders form will look with these buttons.
Figure: The Assign to Me and Update Prices buttons of the Repair Work Orders form
Figure: The Update Prices button on the Labor tab
In the course, you will also add the Validate Prices button on the table toolbar of the Repair Items tab of the
Services and Prices (RS203000) form. By clicking this button, a user can asynchronously validate the prices of the
repair items on this tab from an external service.
Below you can see how the Services and Prices form will look with the added button.


Company Story and Customization Description | 8
Figure: The Validate Prices button of the Services and Prices form


Initial Configuration | 9
Initial Configuration
You need to perform prerequisite actions before you start to complete the course.
To Deploy an Instance for the Training Course
This activity will walk you through the preparation and deployment of an Acumatica ERP instance that you can use
to perform the steps in the lessons of this training course.
Story
To perform customization tasks and complete the activities described in the lessons of this training course, you
need to deploy an instance of Acumatica ERP with the PhoneRepairShop customization project published and then
configure the instance.
Process Overview
In this activity, you will prepare the environment and install tools that will help you to perform customization tasks.
You will then deploy the instance of Acumatica ERP with the PhoneRepairShop customization project published
and the dataset from the T220 Data Entry and Setup Forms course. Finally, in Acumatica ERP, you will configure the
instance that you have deployed.
Step 1: Preparing the Environment
If you have completed any training course of the T series and are using the same environment for the
current course, you can skip this step.
To prepare the environment, do the following:
1. Make sure that the environment you’re going to use conforms to the System Requirements for the Acumatica
ERP Installation.
2. Make sure that the Web Server (IIS) features listed in Configuration of IIS Web Server Features are turned on.
3. Install the Acuminator extension for Visual Studio.
4. Clone or download the customization project and the source code of the extension library from the Help-
and-Training-Examples repository in Acumatica GitHub to a folder on your computer.
5. Install Acumatica ERP. On the Main Soware Configuration page of the Acumatica ERP Setup wizard, select
the Install Acumatica ERP and Install Debugger Tools check boxes.
If you’ve already installed Acumatica ERP without the debugger tools, you should uninstall
it and install it again with the Install Debugger Tools check box selected. The reinstallation
of Acumatica ERP doesn’t aﬀect existing Acumatica ERP instances. You can also install the
Acumatica ERP Tools separately. For details, see Acumatica ERP Installation On-Premises: To
Install the Acumatica ERP Tools (Optional).
Step 2: Deploying the Instance
To perform customization tasks, you need to deploy an instance of Acumatica ERP for the T230 Actions training
course on the instance.


Initial Configuration | 10
You deploy an Acumatica ERP instance and configure it as follows:
1. Open the Acumatica ERP Configuration wizard, and do the following:
a. Click Deploy a New Acumatica ERP Instance for T-Series Developer Courses.
b. On the Instance Configuration page, do the following:
a. In the Training Course box, select T230 Actions.
b. In the Local Path to the Instance box, select a folder that’s outside of the C:\Program Files
(x86), C:\Program Files, and C:\Users folders. (We recommend that you store the website
folder outside of these folders to avoid an issue with permission to work in these folders when you
customize the website.)
c. On the Database Configuration page, make sure that the name of the database is SmartFix_T230.
The system creates a new Acumatica ERP instance, adds a new tenant, loads the dataset to it, and publishes
the customization project that is needed for the activities of this training course.
2. Make sure that a Visual Studio solution is available in the App_Data\Projects\PhoneRepairShop
folder of the Acumatica ERP instance folder.
This is the solution of the extension library that you’ll modify in the activities of this training course.
3. Sign in to the new tenant by using the following credentials:
•
Username: admin
•
Password: setup
Change the password when the system prompts you to do so.
4. In the top right corner of the Acumatica ERP screen, click the username and then My Profile. The User
Profile (SM203010) form opens. On the General Info tab, under Personal Settings, select YOGIFON in the
Default Branch box; then click Save on the form toolbar.
In subsequent sign-ins to this account, you’ll be signed in to this branch.
5. Optional: Add the Customization Projects (SM204505), Site Map (SM200520), and Generic Inquiry (SM208000)
forms to your favorites. For details about how to add a form to your favorites, see The Acumatica ERP UI:
Favorites.


Lesson 1: Defining an Action and the Associated Button for a Form | 11
Lesson 1: Defining an Action and the Associated Button for
a Form
In this lesson, on the Repair Work Orders (RS301000) form, you will create an action, along with its associated
button on the form toolbar and the equivalent command on the More menu. You will also configure the visibility
and availability of this button and the corresponding command on the More menu.
Action Definition: General Information
To provide users with functionality that is specific to their business needs, you can create actions and make the
associated buttons and commands available on the UI. You implement the actions in a graph. You can configure the
availability and visibility of the buttons and commands based on specific criteria.
Learning Objectives
In this lesson, you will learn how to do the following:
•
Create an action, the associated button on the form toolbar, and the equivalent command on the More
menu
•
Configure the availability and visibility of the button and command depending on field values on the form
Applicable Scenarios
You implement an action in the following cases:
•
You want to redirect a user to a specific form or report.
•
You want to modify or validate data records and save changes to the database.
•
You want to start a background operation, which is executed on a separate thread.
Action Declaration in a Graph
The declaration of an action in a graph consists of the following:
•
A field of the PXAction<> type, which is declared as follows.
public PXAction<Shipment> CancelShipment;
In the PXAction<> type parameter, you should specify the main DAC of the primary data view. Otherwise,
the button corresponding to the action cannot be displayed on the form toolbar (and the corresponding
command cannot be displayed on the More menu).
•
A method that implements the action; this method has the PXButton and PXUIField attributes. This
method has the following forms of declaration:
•
Without parameters and returning void: This standard form of declaration is shown in the following
code example.
[PXButton]
[PXUIField(DisplayName = "Cancel Shipment")]
protected virtual void cancelShipment()
{
    ...


Lesson 1: Defining an Action and the Associated Button for a Form | 12
}
This type of declaration is used for an action that is executed synchronously and is not called from a
processing form.
•
With a parameter of the PXAdapter type and returning IEnumerable: You can see an example of this
form of declaration in the following code.
[PXButton]
[PXUIField(DisplayName = "Release")]
protected virtual IEnumerable release(PXAdapter adapter)
{
    ...
    return adapter.Get();
}
This type of declaration should be used when the action initiates a background operation or is called
from a processing form.
The field and the method should have the same name, diﬀering only in the capitalization of the first letter.
A graph includes the Actions collection of all PXAction<> objects defined in the graph.
Callback on the Action
When a user invokes an action through a button or command on the UI, the page sends a request to the server side
of the application (that is, it executes the callback). By default, for a button or command, the callback is always
executed—that is, the CommitChanges property of the PXButton attribute is true. If you do not need the form
to send the recent changes made on the form, set the CommitChanges property of the PXButton attribute to
false as follows.
[PXButton(CommitChanges = false)]
The CommitChanges property must be always set to true for the actions that cause changes to be saved to the
database.
Configuration of the Button and Command Associated with an Action
You can adjust the availability and visibility of a button or command (or both, if applicable) on the UI at runtime by
using event handlers, attributes, or Workflow API. For details, see Action Customization: Disabling or Enabling of an
Action and Action Customization: Visibility of an Action.
The following code example shows how to set the availability of a button inside an event handler. To do this, you
should use the methods of the PXAction<> class, as the following code example shows.
// Disabling the CancelShipment action
CancelShipment.SetEnabled(false);
You do not use the static methods of the PXUIField attribute, because these methods work only with the
attribute copies stored in PXCache objects.
For more information about actions and how to customize them, see Action Customization: General Information.


Lesson 1: Defining an Action and the Associated Button for a Form | 13
Activity 1.1: To Define an Action for a Form
The following activity will walk you through the process of implementing an action for a form. The action will be
represented by a button on the form toolbar of the form and the equivalent command on the More menu.
Story
Suppose that you are a developer working on the Repair Work Orders (RS301000) form in the PhoneRepairShop
customization project. You need to create an action that gives users the ability to assign the selected repair
work order to themselves and add the corresponding button to the form toolbar. You’ll define an action in the
graph of the form and configure the associated button (on the form toolbar) and its equivalent command (on the
More menu). The button and command will be visible only if the repair work order has the On Hold or Ready for
Assignment status and available if the Assignee box doesn’t already contain the current user's name.
When the user clicks the button or command, the contact ID associated with the current user, which is specified
in the Linked Entity box of the Users (SM201010) form, will be inserted into the Assignee box. (If the user has
selected an assignee before clicking the button or command, it will be overridden.) The clicking of the button or
command will not aﬀect the status of the repair work order.
Process Overview
In this activity, you’ll perform the following steps:
1. Creating the AssignToMe action, the associated Assign to Me button on the form toolbar, and the
command with the same name on the More menu
2. Configuring the availability and visibility of the Assign to Me button and the corresponding command on
the More menu
3. Testing the Assign to Me button and the associated action
Step 1: Implementing the AssignToMe Action
In this step, you’ll implement the AssignToMe action. The associated button will be placed on the form toolbar
and the equivalent command on the More menu (under the default Other category).
Commands on the More menu are listed under categories. You can change the category of the
command by changing the value of the Category property of the PXButton attribute.
To implement the AssignToMe action, do the following:
1. In Visual Studio, open the RSSVWorkOrderEntry.cs file.
2. Aer the Graph constructor region of the RSSVWorkOrderEntry class, insert the following code.
        #region Actions
        public PXAction<RSSVWorkOrder> AssignToMe = null!;
        [PXButton]
        [PXUIField(DisplayName = "Assign to Me", Enabled = true)]
        protected virtual void assignToMe()
        {
            // Get the current order from the cache.
            RSSVWorkOrder row = WorkOrders.Current;
            // Assign the contact ID associated with the current user


Lesson 1: Defining an Action and the Associated Button for a Form | 14
            row.Assignee = PXAccess.GetContactID();
            // Update the data record in the cache.
            WorkOrders.Update(row);
            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
        }
        #endregion
You don’t need to set the CommitChanges property of the PXButton attribute to True
because CommitChanges is True by default for PXButton.
3. Save your changes.
Step 2: Specifying the Availability and Visibility of the Assign to Me Button and Command (with
RowSelected)
The Assign to Me button on the form toolbar (and the equivalent command on the More menu) should be:
•
Visible for only orders with the On Hold or Ready for Assignment status
•
Available if the Assignee box does not already contain the employee name of the user who is currently
signed in
To satisfy these conditions, you should specify the Enabled and Visible properties of the AssignToMe action.
Do the following:
1. In Visual Studio, open the RSSVWorkOrderEntry.cs file.
2. Add the handler for the RowSelected event for the RSSVWorkOrder DAC, as shown in the following
code.
        // Manage visibility and availability of the actions.
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            if (e.Row == null) return;
            RSSVWorkOrder row = e.Row;
        }
3. In the _(Events.RowSelected<RSSVWorkOrder> e) event handler, add the following code to the
end of the method.
            AssignToMe.SetEnabled((row.Status ==
                WorkOrderStatusConstants.ReadyForAssignment ||
                row.Status == WorkOrderStatusConstants.OnHold) &&
                WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);
In the code above, you’ve checked the WorkOrders object status in the PXCache to disable the Assign to
Me button and command until the repair work order is saved in the database.
4. In the _(Events.RowSelected<RSSVWorkOrder> e) event handler, add the following code to the
end of the method.
            AssignToMe.SetVisible(row.Assignee != PXAccess.GetContactID());
5. Save your changes.


Lesson 1: Defining an Action and the Associated Button for a Form | 15
6. Rebuild the project.
Step 3: Testing the Assign to Me Button and the Associated Action
To test the AssignToMe action and the Assign to Me button, do the following:
1. In Acumatica ERP, open the Repair Work Orders (RS301000) form.
2. Create a work order, and specify the following settings:
•
Customer ID: C000000001
•
Service: Battery Replacement
•
Device: Nokia 3310
•
Description: Battery replacement, Nokia 3310
3. Save your changes.
The 000003 repair work order has been created.
Notice that the Assign to Me button is displayed on the form toolbar, as shown below.
Figure: The Assign to Me button on the Repair Work Orders form
4. On the form toolbar, click the Assign to Me button.
In the Assignee box, notice that the employee name associated with the current user is now specified; the
employee name associated with the user was copied from the Linked Entity box of the Users (SM201010)
form. Also, notice that the Assign to Me button is no longer displayed on the form toolbar.


Lesson 1: Defining an Action and the Associated Button for a Form | 16
Figure: The employee name in the Assignee box


Lesson 2: Defining an Action and the Associated Button for a Table | 17
Lesson 2: Defining an Action and the Associated Button for
a Table
In this lesson, on the Repair Work Orders (RS301000) form, you will create an action and the associated button on
the table toolbar of the Repair Items and Labor tabs.
Activity 2.1: To Define an Action for a Table
The following activity will walk you through the process of implementing an action for a table, which is represented
by a button on the table toolbar.
Story
Suppose that you need to give the users the ability to update the following prices on the Repair Work Orders
(RS301000) form aer entering new prices on the Services and Prices (RS203000) form:
•
The base prices of repair items on the Repair Items tab
•
The default prices of labor items on the Labor tab
You need to define an action for each tab, along with the associated button on the table toolbar of the tab. These
buttons will be available only if no invoice has been created for the repair work order selected on the form.
Process Overview
In this activity, you’ll implement actions with buttons on the table toolbar of the Repair Items and Labor tabs of
the Repair Work Orders (RS301000) form by performing the following steps:
1. Creating an action and the associated Update Prices button on the table toolbar of the Repair Items tab
2. Creating an action and the associated Update Prices button on the table toolbar of the Labor tab
3. Testing the Update Prices buttons and the associated actions
Step 1: Implementing the UpdateItemPrices Action
In this step, you will implement the UpdateItemPrices action in the RSSVWorkOrderEntry graph.
You’ll specify the availability of the button related to the action in the RowSelected event handler of the same
graph by using the SetEnabled() method of PXAction<>. You’ll hide the button from the form toolbar and
the corresponding command from the More menu by setting the DisplayOnMainToolbar property of the
PXButton attribute to false.
To implement the UpdateItemPrices action, do the following:
1. In the Actions region of the RSSVWorkOrderEntry graph, add the UpdateItemPrices action, as
the following code shows.
        public PXAction<RSSVWorkOrder> UpdateItemPrices = null!;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Update Prices", Enabled = true)]
        protected virtual void updateItemPrices()
        {
            var order = WorkOrders.Current;
            if (order.ServiceID == null || order.DeviceID == null) return;
            var repairItems = RepairItems.Select();


Lesson 2: Defining an Action and the Associated Button for a Table | 18
            foreach (RSSVWorkOrderItem item in repairItems)
            {
                RSSVRepairItem origItem = SelectFrom<RSSVRepairItem>.
                    Where<RSSVRepairItem.serviceID.IsEqual<@P.AsInt>.
                    And<RSSVRepairItem.deviceID.IsEqual<@P.AsInt>>.
                    And<RSSVRepairItem.inventoryID.IsEqual<@P.AsInt>>>.
                    View.Select(this, 
                        order.ServiceID, order.DeviceID, item.InventoryID).
                    FirstOrDefault();
                if (origItem != null)
                {
                    item.BasePrice = origItem.BasePrice;
                    RepairItems.Update(item);
                }
            }
            Actions.PressSave();
        }
In the action method, you’ve selected a repair item specified on the Services and Prices (RS203000) form
and assigned its price to a repair item specified on the Repair Work Orders (RS301000) form. (In the T220
Data Entry and Setup Forms training course, a similar scenario is implemented in the RowUpdated event
handler of the RSSVWorkOrderEntry graph.)
In the action method, you don’t need to update the RSSVWorkOrder.OrderTotal field, which contains
the total price of the work order. The value is updated automatically because the PXFormula attribute is
specified for the RSSVWorkOrderItem.BasePrice field.
2. In the RowSelected event handler, specify that the button associated with the action should be available
only when no invoice has been created for the order, as the following code shows.
            UpdateItemPrices.SetEnabled(WorkOrders.Current.InvoiceNbr == null);
3. Rebuild the project.
Step 2: Placing the Update Prices Button on the Repair Items Tab
In this step, you’ll place the Update Prices button on the table toolbar of the Repair Items tab. To place the
button, in the RS301000.ts file, you will specify the PXActionState property with the name of the
UpdateItemPrices action in the RSSVWorkOrderItem view class.
Thus, to place the Update Prices button on the table toolbar of the Repair Items tab, you should modify the
RS301000.ts file of the Repair Work Orders (RS301000) form. Do the following:
1. Click the RS301000.ts file on the Modern UI Files page of the Customization Project Editor.
2. In the Edit File dialog box, which opens, add PXActionState to the list of imports.
3. In the RSSVWorkOrderItem view class, specify the PXActionState property with the name of the
UpdateItemPrices action. The RSSVWorkOrderItem view class should look as follows aer you’ve
completed these steps.
@gridConfig({
 preset: GridPreset.Details
})
export class RSSVWorkOrderItem extends PXView {
    UpdateItemPrices: PXActionState;
    RepairItemType: PXFieldState;
    InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
    InventoryID_description: PXFieldState;


Lesson 2: Defining an Action and the Associated Button for a Table | 19
    BasePrice: PXFieldState;
}
4. Save your changes by clicking the Save button of the dialog box.
5. Publish the customization project.
Step 3: Placing the Update Prices Button on the Labor Tab—Self-Guided Exercise
In this self-guided exercise, you will implement the UpdateLaborPrices action in the RSSVWorkOrderEntry
graph. You’ll configure the availability of the button associated with the action on the form in the RowSelected
event handler of the same graph, hide the button from the form toolbar (and the corresponding command from the
More menu), and place the button on the Labor tab of the Repair Work Orders (RS301000) form.
In the action method, you’ll select a labor item specified on the Services and Prices (RS203000) form and assign its
price to a labor item specified on the Repair Work Orders (RS301000) form.
In the RowSelected event handler, you’ll make the UpdateLaborPrices action enabled when no invoice has
been created for the order.
To perform this step, use the skills you’ve learned in Steps 1 and 2 of this activity.
You can find the code changes made in this step in the Customization\T230\CodeSnippets
\Activity2.1_Step3 folder, which you’ve downloaded from Acumatica GitHub.
Step 4: Testing the Update Prices Buttons and Associated Actions
In this step, you will test the Update Prices buttons that you’ve added to the Repair Items and Labor tabs of the
Repair Work Orders (RS301000) form. To test the buttons and the underlying actions, do the following:
1. Modify the original repair and labor items by doing the following:
a. On the Service and Prices (RS203000) form, open the record with the following settings:
•
Service: Battery Replacement
•
Device: Nokia 3310
b. On the Repair Items tab, in the row with the BAT3310 inventory ID, enter 25 in the Price column.
c. On the Labor tab, in the row with the CONSULT inventory ID, enter 10 in the Default Price column.
d. Save your changes.
2. Update the prices by doing the following:
a. Open the Repair Work Orders (RS301000) form.
b. Select the 000003 repair work order, which you created in Step 3 of the Activity 1.1: To Define an Action
for a Form activity. On the Repair Items tab, make sure that the Update Prices button is displayed, as
shown below.


Lesson 2: Defining an Action and the Associated Button for a Table | 20
Figure: The Update Prices button on the Repair Items tab
c. On the Repair Items tab, notice the old price of the repair item with the BAT3310 inventory ID.
d. On the table toolbar, click Update Prices.
Notice that the item price has been updated and the Order Total in the Summary area of the form has
been updated.
e. On the Labor tab, notice the old price of the labor item with the CONSULT inventory ID.
f.
On the table toolbar, click Update Prices.
Notice that the item price has been updated and the Order Total has been updated.


Lesson 3: Implementing an Asynchronous Operation | 21
Lesson 3: Implementing an Asynchronous Operation
An operation that is expected to take a long time to complete should be executed asynchronously: that is, executed
on a separate thread so that it doesn’t block the main thread of an application and freeze up the user interface.
The Acumatica Framework provides you with a number of approaches for executing a long-running operation
asynchronously.
In this lesson, you will learn how to process operations whose execution takes a long time.
Asynchronous Operations: General Information
Each time a user action on a form triggers a request (a round trip), the system creates a new graph instance to
process that request. Aer the request is processed, the graph instance must be cleared from the memory of the
Acumatica ERP server. If your code performs a long-running operation—such as processing a document, processing
large volumes of data, or executing an action that needs to call an external API—you should run it asynchronously
in a separate thread.
The Acumatica Framework provides the following approaches for executing a long-running operation
asynchronously:
•
The IGraphLongOperationManager interface: Derives from the ILongOperationManager
interface. We recommend using this interface when you need to run an operation asynchronously from
within a graph or graph extension.
•
The ILongOperationManager interface: Provides a modern approach for handling execution of
asynchronous operations. We recommend that you use this interface when you need to execute an
operation asynchronously outside of a graph or graph extension, such as in a service where you don't have
access to a graph's instance.
•
The PXLongOperation class: Provides a legacy approach for asynchronous execution.
In this lesson, you’ll learn how to run an operation asynchronously by using the
IGraphLongOperationManager and ILongOperationManager interfaces, as well as the
PXLongOperation class.
Learning Objectives
In this lesson, you’ll learn how to do the following:
•
Implement a long-running action
•
Add the associated button to the table toolbar
•
Set up the long-running action to run asynchronously by using the LongOperationManager property of
a graph
Applicable Scenarios
You set up an operation to run asynchronously when this operation is expected to take a long time to complete.
Use of the IGraphLongOperationManager Interface
You don't need to implement the IGraphLongOperationManager interface manually because it’s already
injected as a dependency-injection service into PXGraph out of the box.
You can access this interface in a graph via the graph’s LongOperationManager property. In a graph extension,
you can access this property via Base.LongOperationManager. The interface has useful method overloads
—which don't require an object? key parameter—for executing a long-running operation. When you use one


Lesson 3: Implementing an Asynchronous Operation | 22
of these overloads, the framework automatically associates the long-running operation with the current graph
instance.
To execute a long-running operation asynchronously in your graph, you can use the following methods:
•
void StartOperation(Action<CancellationToken>? method), which takes in a
synchronous delegate as a parameter
•
void StartAsyncOperation(Func<CancellationToken, Task> method), which takes in an
asynchronous delegate as a parameter
The following code shows you how to use void StartOperation(Action<CancellationToken>?
method) to execute a long-running operation in a graph.
public class RepairWOProcessingGraph : PXGraph<RepairWOProcessingGraph>
{
    // Method that defines the long-running operation
    private void ProcessRepairWorkOrdersInternal(CancellationToken token)
    {
        // Long-running logic
        // Optional: Periodically honor cancellation
        token.ThrowIfCancellationRequested();
    }
    
    // Method that kicks off the long-running operation   
    public void ProcessRepairWorkOrders()
    {
        // In a graph, the property LongOperationManager (IGraphLongOperationManager) 
        // is already available/injected.
        LongOperationManager.StartOperation(ProcessRepairWorkOrdersInternal);
    }
}
In a graph extension, you should use Base.LongOperationManager to execute
a long-running operation. So in the preceding code example, you would use
Base.LongOperationManager.StartOperation(ProcessRepairWorkOrdersInternal);
Note that the void StartOperation(Action<CancellationToken>? method) method
shown in the code example above is an overload of the void StartOperation(object? key,
Action<CancellationToken>? method) of the ILongOperationManager interface. Similarly, the
void StartAsyncOperation(Func<CancellationToken, Task> method) method is an overload
of void StartOperation(object? key,Func<CancellationToken, Task> method). We
recommend that you use these overloads because they don't require you to provide an object? key parameter.
When you use these overloads, the framework automatically associates the long-running operation with the
current graph instance.
For the full list of methods available in the IGraphLongOperationManager interface, see API Reference.
Use of the ILongOperationManager Interface
You use the ILongOperationManager when you need to execute a long-running operation asynchronously
outside of a graph or a graph extension, such as in a service where you don't have access to a graph's instance.
To do this, you need to first use dependency injection to inject the ILongOperationManager service into a
type. You can then use this type to execute a long-running operation asynchronously, as shown below.


Lesson 3: Implementing an Asynchronous Operation | 23
If a service operates in the context of a specific graph instance and the long-running operation
is logically associated with that graph, you should use the graph’s LongOperationManager
property instead of injecting the ILongOperationManager service. This keeps the operation
properly associated with the graph lifecycle and UI context.
public class RepairWOProcessingService
{
    // Inject ILongOperationManager via DI
    private readonly ILongOperationManager _longOperationManager;
    public RepairWOProcessingService(ILongOperationManager longOperationManager)
    {
       // Initialize _longOperationManager
        _longOperationManager = longOperationManager;
    }
    // Method that defines the long-running operation
    private void ProcessRepairWorkOrdersInternal(CancellationToken token)
    {
        // Long-running logic
        // Optional: periodically honor cancellation
        token.ThrowIfCancellationRequested();
    }
    // Method that kicks off the long-running operation for the service   
    public void ProcessRepairWorkOrders()
    {
        // Unique ID for the long-running operation
        var key = Guid.NewGuid();
        
        // Use the StartOperation method of the ILongOperationManager interface 
        // to execute the long-running operation asynchronously in a separate thread.
        _longOperationManager.StartOperation(key, ProcessRepairWorkOrdersInternal);
    }
}
Note that in the example above, the void StartOperation(object? key,
Action<CancellationToken>? method) method of the ILongOperationManager interface is
used to execute a long-running operation asynchronously, which takes in a object? key and a synchronous
delegate as parameters. The interface also provides the void StartOperation(PXGraph graph,
Action<CancellationToken>? method) overload, which you can use to execute a long-running operation
when a graph's instance is available. For this method, you pass in the graph's instance instead of a object? key.
The ILongOperationManager interface also provides the void StartAsyncOperation(object?
key, Func<CancellationToken, Task> method) method, which takes in an asynchronous delegate as
a parameter. It has the void StartAsyncOperation(PXGraph graph, Func<CancellationToken,
Task> method) method as an overload. For details, see the API Reference.
Use of the PXLongOperation Class
You can use the PXLongOperation class when you need to execute a long-running operation asynchronously.


Lesson 3: Implementing an Asynchronous Operation | 24
This is a legacy approach. We recommend that you instead use the ILongOperationManager and
IGraphLongOperationManager interfaces.
To make the system invoke the method in a separate thread, you can use the
PXLongOperation.StartOperation method. Within the method that you pass to StartOperation, you
can, for example, create a new instance of a graph and invoke a processing method on that instance. The following
code snippet demonstrates how you can execute code asynchronously as a long-running operation in a method of
a graph.
To instantiate graphs from code, use the PXGraph.CreateInstance<T>() method. Do not use
the new T() graph constructor because in this case, no extensions or overrides of the graph are
initialized.
public class MyGraph : PXGraph
{
  ...
  public void MyMethod()
  {
    ...
    PXLongOperation.StartOperation(this, delegate()
    {
      // insert the delegate method code here
      ...
      GraphName graph = PXGraph.CreateInstance<GraphName>();
      foreach (... in ...)
      {
        ...
      }
      ...
    });
  ...
  }
  ...
}
Do not pass a reference to this graph to the StartOperation method from a graph extension. If
you did, the system wouldn’t be able to notify the UI about whether any errors occurred during the
asynchronous operation or whether the operation was completed.
If you need to save data to the database inside a long-running operation, call the Save.Press() method of the
current graph. We recommend not using the Actions.PressSave() method because it performs an external
call and should be used from the UI only.
The following code shows an example of a method called InvoiceOrder that's executed asynchronously. This
method is being called within the delegate that you pass to PXLongOperation.StartOperation() method.
PXLongOperation.StartOperation(this, delegate ()
{
  InvoiceOrder(graphCopy); 
});
The PXLongOperation.StartOperation() method creates a separate thread and executes the specified
delegate asynchronously on this thread. The method passed into PXLongOperation.StartOperation()
matches the following delegate type, which has no input parameters.


Lesson 3: Implementing an Asynchronous Operation | 25
delegate void PXToggleAsyncDelegate();
In the example, the (delegate()) anonymous method definition is used to shorten the code.
Inside the delegate() method, you shouldn’t use members of the current graph, because this would lead to
synchronous execution of the method. Instead, use a copy of the graph, which you can create by using the var
graphCopy = this.Clone(); statement.
Invocation of a Long-Running Operation in an Action
If you need to invoke a long-running operation in an action, the action handler for a processing operation must
return IEnumerable. If you use the void action handler instead, the processing of the long-running operation
and its result will not be displayed in the UI.
Below you can see an example of an action definition that runs a long-running operation.
public PXAction<RSSVWorkOrder> Assign = null!;
[PXButton]
[PXUIField(DisplayName = "Assign", Enabled = false)]
protected virtual IEnumerable assign(PXAdapter adapter)
{
    <Invocation of a long-running operation>
}
Related Links
•
IGraphLongOperationManager
•
ILongOperationManager
•
PXLongOperation
•
Asynchronous Operations: How They are Handled in Acumatica ERP
Activity 3.1: To Implement an Asynchronous Operation
The following activity will walk you through the process of implementing a long-running action and executing it
asynchronously.
Story
Suppose that as part of the PhoneRepairShop customization project, you need to create an action that users can
invoke to validate the prices of the repair items. These items are listed on the Repair Items tab of the Services
and Prices (RS203000) form. Price validation will be performed by an external service and may take a long time to
complete—so it should be executed asynchronously.
To implement this, you need to define an action in the graph of the form and configure the associated button on
the table toolbar. You also need to write the code that will validate the prices and execute this code asynchronously
by using the LongOperationManager.StartOperation method.
Process Overview
In this activity, you will create an action (and the associated button on the table toolbar) and execute it
asynchronously by performing the following steps:


Lesson 3: Implementing an Asynchronous Operation | 26
1. Adding the IsPriceValidated field (which you’ll add to the database during system preparation) to the
RSSVRepairItem DAC, and updating the TypeScript file of the Services and Prices (RS203000) form. The
system will display this field as the Price Validated column in the table on the Repair Items tab.
2. Defining the logic that validates the repair item prices in the ValidatePrices method, implementing the
ValidateItemPrices action to run the ValidatePrices method asynchronously, and creating the
associated Validate Prices button on the table toolbar.
3. Testing the Validate Prices button and the underlying action.
System Preparation
Before you begin performing the steps of this activity, do the following:
1. In SQL Server Management Studio, execute the T230_AddColumn_RSSVRepairItem.sql script.
The script is provided in the Customization\T230\SourceFiles\DBScripts folder,
which you’ve downloaded from Acumatica GitHub.
2. To update the customization project, do the following in the Customization Project Editor opened for the
PhoneRepairShop project:
a. In the navigation pane, click Database Scripts.
b. On the More menu of the Database Scripts page, which opens, click Reload from Database.
Step 1: Updating the Services and Prices (RS203000) Form to Display the Price Validated Column
and the Validate Prices Button
In this step, you’ll add the IsPriceValidated field to the RSSVRepairItem DAC. You’ll also update the
corresponding TypeScript file to show the Price Validated column in the table and the Validate Prices button on
the table toolbar of the Repair Items tab. Do the following:
1. Add the following code to the RSSVRepairItem.cs file aer the BasePrice field definition.
        #region IsPriceValidated
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Price Validated", Enabled = false)]
        public virtual bool? IsPriceValidated { get; set; }
        public abstract class isPriceValidated :
            PX.Data.BQL.BqlBool.Field<isPriceValidated> { }
        #endregion
Note that the field is disabled in the UI because it will be updated by an external service. The value is set to
false by default.
2. Open the RS203000.ts file in the Modern UI Files node of the Customization Project Editor, and do the
following:
a. Add the following code in the RSSVRepairItem view class aer the BasePrice field.
 IsPriceValidated: PXFieldState;
This code has added the Price Validated column—which is represented by the IsPriceValidated
DAC field—to the table on the Repair Items tab.
b. Specify the PXActionState property with the name of the ValidateItemPrices action (which
you’ll declare in Step 3) in the RSSVRepairItem view class, as shown in the following code.


Lesson 3: Implementing an Asynchronous Operation | 27
    ValidateItemPrices: PXActionState;
The code above adds the Validate Prices button to the table toolbar of the Repair Items tab.
c. Add PXActionState to the list of imports if it has not been added yet.
3. Save your changes.
Step 2: Defining the Logic Used to Validate Prices
Next, you’ll define the method in which the repair items prices are validated, and then you’ll call this method in the
LongOperationManager.StartOperation method.
To define the method in which the repair items prices are validated, do the following:
1. Add the ValidatePrices static method to the RSSVRepairPriceMaint graph. The
ValidatePrices method validates the repair items prices for the selected record on the Services and
Prices (RS203000) form.
        private static void ValidatePrices(RSSVRepairPrice repairPriceItem)
        {
            // Create an instance of the RSSVRepairPriceMaint graph
            // and set the Current property of its RepairPrices view.
            var priceMaint = PXGraph.CreateInstance<RSSVRepairPriceMaint>();
            priceMaint.RepairPrices.Current = priceMaint.RepairPrices.
             Search<RSSVRepairPrice.serviceID, RSSVRepairPrice.deviceID>
             (repairPriceItem.ServiceID, repairPriceItem.DeviceID);
            // Set a delay to mimic connecting to an external service to validate the 
            // repair item prices.
            // In a real world scenario, you would connect to an actual external 
            // service and make an API request to validate the prices for 
            // the repair items.
            Thread.Sleep(3000);
            // Update the Price Validated field for each repair item on 
            // the Repair Items tab:
            // Here we are assuming that the validation was successful from the
            // external service and are setting IsPriceValidated to true for 
            // each repair item.     
            foreach (RSSVRepairItem item in priceMaint.RepairItems.Select())
            {
                // Set IsPriceValidated to true for each repair item.
                item.IsPriceValidated = true;
                // Update the cache with the above change for each repair item.
                priceMaint.RepairItems.Update(item);
            }
            // Trigger the Save action to save the changes stored in the cache 
            // to the database.
            priceMaint.Actions.PressSave();
        }
2. Make sure the following using directives have been added to the RSSVRepairPriceMaint.cs file.
using System.Collections;
using System.Collections.Generic;
using System.Threading;


Lesson 3: Implementing an Asynchronous Operation | 28
Step 3: Defining the ValidateItemPrices Action
Now you’ll define the ValidateItemPrices action, which:
•
Defines the underlying action for the Validate Prices button on the table toolbar of the Repair Items tab
•
Invokes the LongOperationManager.StartOperation method, which executes the
ValidatePrices method asynchronously.
Add the following code to the RSSVRepairPriceMaint graph.
        #region Actions
        public PXAction<RSSVRepairPrice> ValidateItemPrices = null!;
        [PXButton(DisplayOnMainToolbar = false, CommitChanges = true)]
        [PXUIField(DisplayName = "Validate Prices", Enabled = true)]
        protected virtual IEnumerable validateItemPrices(PXAdapter adapter)
        {
            // Populate a local list variable.
            List<RSSVRepairPrice> list = new List<RSSVRepairPrice>();
            foreach (RSSVRepairPrice repairItemPrice in adapter.Get<RSSVRepairPrice>())
            {
                list.Add(repairItemPrice);
            }
            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
            var repairPriceItem = RepairPrices.Current;
            // Execute the ValidatePrices method asynchronously by
            // using LongOperationManager.StartOperation
             LongOperationManager.StartOperation(cancellationToken =>
             {
                ValidatePrices(repairPriceItem);
             });
            // Return the local list variable.
            return list;
        }
        #endregion
To perform a background operation, an action method needs to have a parameter of the PXAdapter
type and return IEnumerable.
Step 4: Testing the Validate Prices Button and the Associated Action
To test the Validate Prices button and the underlying action, do the following:
1. Rebuild the PhoneRepairShop_Code project in Visual Studio.
2. In the Customization Project Editor, publish the customization project.
3. In Acumatica ERP, open the Services and Prices (RS203000) form.
4. Select the BATTERYREPLACE service for the NOKIA3310 device.
Notice that the Price Validated check box is cleared for any repair item on the Repair Items tab and that
the Validate Prices button is available on the table toolbar.
5. On the table toolbar, click Validate Prices.


Lesson 3: Implementing an Asynchronous Operation | 29
A notification appears indicating the status of the processing, as shown below.
Figure: Validation of the prices of repair items
When the process is complete, the Price Validated check box for each repair item is selected on the Repair
Items tab, as shown below.
Figure: Update of the Price Validated column


Appendix: Initial Configuration | 30
Appendix: Initial Configuration
If for some reason, you cannot complete the instructions in Initial Configuration, you can create an Acumatica ERP
instance and manually publish the needed customization project, as described in this topic.
Step 1: Deploying the Needed Acumatica ERP Instance for the Training Course
You deploy an Acumatica ERP instance and configure it as follows:
1. To deploy a new application instance, open the Acumatica ERP Configuration wizard, and do the following:
a. On the Database Configuration page, type the name of the database: SmartFix_T230.
b. On the Tenant Setup page, create a tenant with the T100 dataset inserted by specifying the following
settings:
•
Tenant Name: Company
•
New: Selected
•
Insert Data: T100
•
Parent Tenant ID: 1
•
Visible: Selected
c. On the Instance Configuration page, in the Local Path of the Instance box, select a folder that’s outside
of the C:\Program Files (x86), C:\Program Files, and C:\Users folder. We recommend
that you store the website folder outside of these folders to avoid an issue with permission to work in
these folders when you perform customization of the website.
The system creates a new Acumatica ERP instance, adds a new tenant, and loads the selected data to it.
2. Sign in to the new tenant by using the following credentials:
•
Username: admin
•
Password: setup
Change the password when the system prompts you to do so.
3. In the top right corner of the Acumatica ERP screen, click the username and then My Profile. The User
Profile (SM203010) form opens. On the General Info tab, under Personal Settings, select YOGIFON in the
Default Branch box; then click Save on the form toolbar.
In subsequent sign-ins to this account, you’ll be signed in to this branch.
4. Optional: Add the Customization Projects (SM204505), Site Map (SM200520), and Generic Inquiry (SM208000)
forms to your favorites. For details about how to add a form to your favorites, see The Acumatica ERP UI:
Favorites.
Step 2: Publishing the Required Customization Project
Load the customization project with the results of the T220 Data Entry and Setup Forms training course and publish
this project as follows:
1. On the Customization Projects (SM204505) form, create a project with the name PhoneRepairShop, and open
it.
2. In the menu of the Customization Project Editor, click Source Control > Open Project from Folder.
3. In the dialog box that opens, specify the path to the Customization\T220\PhoneRepairShop folder,
which you have downloaded from Acumatica GitHub, and click OK.
4. Bind the customization project to the source code of the extension library as follows:


Appendix: Initial Configuration | 31
a. Copy the Customization\T220\PhoneRepairShop_Code folder to the App_Data\Projects
folder of the website.
By default, the system uses the App_Data\Projects folder of the website as the parent
folder for the solution projects of extension libraries.
If the website folder is outside of the C:\Program Files (x86), C:\Program
Files, and C:\Users folders, we recommend that you use the App_Data\Projects
folder for the project of the extension library.
If the website folder is in the C:\Program Files (x86), C:\Program Files, or C:
\Users folder, we recommend that you store the project outside of these folders to avoid
an issue with permission to work in these folders. In this case, you need to update the links
to the website and library references in the project.
b. Open the solution, and build the PhoneRepairShop_Code project.
c. Reload the Customization Project Editor.
d. In the menu of the Customization Project Editor, click Extension Library > Bind to Existing.
e. In the dialog box that opens, specify the path to the App_Data\Projects
\PhoneRepairShop_Code folder, and click OK.
5. On the menu of the Customization Project Editor, click Publish > Publish Current Project.
The Modified Files Detected dialog box opens before publication because you have rebuilt
the extension library in the PhoneRepairShop_Code Visual Studio project. The Bin
\PhoneRepairShop_Code.dll file has been modified and you need to update it in the
project before the publication.
The published customization project contains all changes to the Acumatica ERP website and database that have
been performed in the T200 Maintenance Forms, T210 Customized Forms and Master-Details Relationships, and T220
Data Entry and Setup Forms training courses. This project also contains the customization plug-ins that fill in the
tables created in the T200 Maintenance Forms, T210 Customized Forms and Master-Details Relationships, and T220
Data Entry and Setup Forms training courses with the custom data entered in these training courses. For details
about the customization plug-ins, see To Add a Customization Plug-In to a Project. (The creation of customization
plug-ins is outside of the scope of this course.)
