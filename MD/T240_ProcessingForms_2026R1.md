Developer Course
Customization
T240 Processing Forms
2026 R1
Revision: 4/15/2026


Contents | 2
Contents
Copyright...............................................................................................................................................4
How to Use This Course.......................................................................................................................... 5
Company Story and Customization Description........................................................................................ 7
Initial Configuration..............................................................................................................................10
To Deploy an Instance for the Training Course..............................................................................................10
Part 1: Processing Form (Assign Work Orders)........................................................................................ 12
Lesson 1.1: Creating a Simple Processing Form............................................................................................ 12
Processing Form: General Information................................................................................................. 12
Processing Form: UI Guidelines.............................................................................................................14
Processing Form: A Form with Only a Grid........................................................................................... 15
Activity 1.1.1: To Create a Simple Processing Form .............................................................................17
Lesson 1.2: Implementing the Processing Operation....................................................................................21
Processing Operations: General Information........................................................................................21
Processing Operations: Specifying a Workflow Action.........................................................................22
Processing Operations: Specifying a Processing Delegate...................................................................23
Activity 1.2.1: To Implement a Processing Operation by Using the Workflow ....................................25
Activity 1.2.2: To Implement a Processing Operation by Using a Delegate ........................................ 28
Lesson 1.3: Adding Filtering Parameters to the Processing Form.................................................................31
Filtering Parameters: General Information........................................................................................... 31
Activity 1.3.1: To Add a Filter for a Processing Form............................................................................ 33
Part 2: Update of Data with a Custom Accumulator Attribute................................................................... 42
Lesson 2.1: Implementing a Custom PXAccumulator Attribute.................................................................... 42
PXAccumulator: General Information....................................................................................................42
PXAccumulator: Implementation of a Custom PXAccumulator Attribute........................................... 43
Activity 2.1.1: To Implement a Custom Accumulator Attribute............................................................44
Lesson 2.2: Modifying the Processing Form to Use the Field Updated by PXAccumulator .........................47
Activity 2.2.1: To Replace Field Attributes in CacheAttached...............................................................47
Activity 2.2.2: To Fetch Calculated Data from a Non-Scalar Source (in RowSelecting).......................49
Activity 2.2.3: To Modify the Processing Form to Use the Field Updated by PXAccumulator............. 50
Part 3: Redirection to a Report at the End of Processing.......................................................................... 55
Lesson 3.1: Adding Redirection to a Report at the End of Processing..........................................................55
Activity 3.1.1: To Add Redirection to a Report at the End of Processing............................................. 55
Additional Materials..............................................................................................................................60
Appendix A: Initial Configuration....................................................................................................................60


Contents | 3
Appendix B: Processing Dialog Box................................................................................................................ 61
Appendix C: Parallel Processing......................................................................................................................62
Appendix D: Asynchronous Execution............................................................................................................62
Appendix E: Use of Event Handlers................................................................................................................ 62


Copyright | 4
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
Last Updated: 04/15/2026


How to Use This Course | 5
How to Use This Course
The T240 Processing Forms training course teaches you how you can create processing forms by using Acumatica
Framework and the customization tools of Acumatica ERP. A processing form is a form on which users can invoke
an operation on multiple selected records at once.
This course is intended for application developers who are starting to learn how to customize Acumatica ERP.
The course is based on a set of examples that demonstrate the general approach to customizing Acumatica ERP. It
is designed to give you ideas about how to develop your own embedded applications through the customization
tools. As you go through the course, you'll continue the development of the customization for the cell phone repair
shop, which was performed in the previous training courses of the T series (which we recommend that you take
before completing the current course).
Aer you complete all the lessons of the course, you'll be familiar with the programming techniques used to define
Acumatica ERP processing forms.
We recommend that you complete the examples in the presented order because some examples use
the results of previous ones.
What the Course Prerequisites Are
To complete this course, you should be familiar with the basic concepts of Acumatica Framework and Acumatica
Customization Platform. Before you begin this course, we recommend that you complete the following training
courses:
•
T200 Maintenance Forms
•
T210 Customized Forms and Master-Details Relationships
•
T220 Data Entry and Setup Forms
•
T230 Actions
•
T270 Workflow API
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


How to Use This Course | 6
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
What Is in a Part
The first part of the course explains how to create two types of processing forms: a form without filtering
parameters, and a form with filtering parameters.
The second part of the course shows how to implement the update of the frequently edited fields (by using a
custom PXAccumulator attribute) and use these fields on a processing form.
The third part of the course shows the implementation of redirection to a report at the end of processing.
Each part of the course consists of lessons you should complete.
What Is in a Lesson
Each lesson focuses on a particular development scenario that you can implement by using Acumatica ERP
customization tools and the Acumatica Framework. Each lesson consists of a brief description of the scenario and
an example of its implementation.
Where the Source Code Is
You can find the source code of the customization described in this course and code snippets for the course in the
Customization\T240 folder of the Help-and-Training-Examples repository in Acumatica GitHub.
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


Company Story and Customization Description | 7
Company Story and Customization Description
In this course, you'll continue the development to support the cell phone repair shop of the Smart Fix company;
you began this development while completing the previous training courses of the T series.
You have loaded and published the customization project with the results of these courses as
described in Initial Configuration.
In the previous training courses of the T series, you have created the following forms:
•
The Repair Services (RS201000) custom maintenance form, which the Smart Fix company uses to manage
the lists of repair services that the company provides
•
The Serviced Devices (RS202000) custom maintenance form, which the Smart Fix company uses to manage
the lists of devices that can be serviced
•
The Services and Prices (RS203000) custom maintenance form, which provides users with the ability to
define and maintain the price for each repair service
•
The Repair Work Orders (RS301000) custom data entry form, which is used to create and manage work
orders for repairs
•
The Repair Work Order Preferences (RS101000) custom setup form, which an administrative user uses to
specify the company's preferences for the repair work orders
In the previous training courses of the T series, you have also customized the Stock Items (IN202500) form to mark
particular stock items as repair items—that is, items that are used for the repair services.
In this course, you'll create the Assign Work Orders (RS501000) custom processing form, which users will use to
assign multiple repair work orders at the same time. You'll implement the functionality of the form in stages:
1. First, you'll implement this form as a simple processing form without any filtering parameters for user
selection.
2. Then you'll add a filter to the form so that only the records that satisfy the filtering parameters are displayed
in the table.
3. Also, you'll implement the selection of the default assignee, which depends on the number of already
assigned work orders for the employees. You'll use a custom PXAccumulator attribute to update the
number of assigned orders in the database for each employee.
4. Finally, you'll implement redirection to a report at the end of the processing.
Assign Work Orders Form
The following screenshot shows how the Assign Work Orders (RS501000) form will look at the end of the course.


Company Story and Customization Description | 8
Figure: Assign Work Orders form
The form will contain the following elements:
•
Two processing buttons on the toolbar: Assign and Assign All, which a user will use to assign only the
selected work orders (that is, those for which the user has selected the unlabeled check boxes) or all of the
listed work orders, respectively, in the table.
•
The filtering UI elements in the Selection area, which a user can use to filter the list of repair work orders by
the priority, the number of days the work order is not assigned, or the service that should be provided.
•
The table that displays the list of work orders that have the Ready for Assignment status and meet the
other filtering criteria specified. Each row of the table lists a work order along with additional information
about it, such as the number of days the order has been unassigned, the assignee to whom the order will
be assigned, and the number of orders that this assignee is currently working on. A user can change the
assignee for any work order in the table.
Database Tables and DACs Used for the Form
This form will use the following custom tables, which will be added to the application database in To Deploy an
Instance for the Training Course:
•
RSSVWorkOrder: The data of this table will be displayed in the table on the form.
•
RSSVEmployeeWorkOrderQty: The data of this table will be used to display the number of assigned
work orders of an employee in the table on the form.
The filtering elements in the Selection area will use the RSSVWorkOrderToAssignFilter DAC, which contains
only unbound fields. Therefore, no table corresponds to this DAC in the database.
In Lesson 2.2: Modifying the Processing Form to Use the Field Updated by PXAccumulator, you'll add the
DefaultAssignee, AssignTo, and NbrOfAssignedOrders fields to the RSSVWorkOrder DAC (see the
diagram below). The values of DefaultAssignee and NbrOfAssignedOrders are calculated based on the
values in the RSSVEmployeeWorkOrderQty table, which holds the numbers of repair work orders assigned to
employees.
The RSSVEmployeeWorkOrderQty table is linked to the EPEmployee table by UserID (You'll add
the RSSVEmployeeWorkOrderQty table to the application database and the corresponding DAC to the
customization code in Activity 2.1.1: To Implement a Custom Accumulator Attribute).


Company Story and Customization Description | 9


Initial Configuration | 10
Initial Configuration
You need to perform prerequisite actions before you start to complete the course.
To Deploy an Instance for the Training Course
The following activity will walk you through the process of preparing and deploying an Acumatica ERP instance that
you can use to perform the steps in the lessons of this training course.
Story
To perform customization tasks and complete the activities described in the lessons of this training course, you
need to deploy an instance of Acumatica ERP with the PhoneRepairShop customization project published and then
create custom database tables.
Process Overview
In this activity, you'll prepare the environment and install tools that will help you to perform customization tasks.
You'll then deploy an instance of Acumatica ERP with the PhoneRepairShop customization project published and
the dataset from the T270 Workflow API course. Finally, you'll create custom database tables.
Step 1: Preparing the Environment
If you have completed any of the training courses of the T series and are using the same environment
for the current course, you can skip this step.
Before you begin deploying the needed Acumatica ERP instance, do the following:
1. Make sure that the environment you’re going to use conforms to the System Requirements for the Acumatica
ERP Installation.
2. Make sure that the Web Server (IIS) features listed in Configuration of IIS Web Server Features are turned on.
3. Install the Acuminator extension for Visual Studio.
4. Install Acumatica ERP. On the Main Soware Configuration page of the Acumatica ERP Setup wizard, select
the Install Acumatica ERP and Install Debugger Tools check boxes.
If you’ve already installed Acumatica ERP without the debugger tools, you should uninstall
it and install it again with the Install Debugger Tools check box selected. The reinstallation
of Acumatica ERP doesn’t aﬀect existing Acumatica ERP instances. You can also install the
Acumatica ERP Tools separately. For details, see Acumatica ERP Installation On-Premises: To
Install the Acumatica ERP Tools (Optional).
Step 2: Deploying the Instance
To perform customization tasks, you need to deploy an instance of Acumatica ERP for the T240 Processing Forms
training course on the instance.
You deploy an Acumatica ERP instance and configure it as follows:
1. Open the Acumatica ERP Configuration wizard, and do the following:


Initial Configuration | 11
a. Click Deploy a New Acumatica ERP Instance for T-Series Developer Courses.
b. On the Instance Configuration page, do the following:
a. In the Training Course box, select T240 Processing Forms.
b. In the Local Path to the Instance box, select a folder that’s outside of the C:\Program Files
(x86), C:\Program Files, and C:\Users folders. (We recommend that you store the website
folder outside of these folders to avoid an issue with permission to work in these folders when you
customize the website.)
c. On the Database Configuration page, make sure the name of the database is SmartFix_T240.
d. On the Website Configuration page, make sure the Install Node.js and Use Modern UI as Default check
boxes are selected.
The system creates a new Acumatica ERP instance, adds a new tenant, loads the data to it, and publishes
the customization project that is needed for activities of this training course.
The system also installs Node.js and adds the NodeJs:NodeJsPath key in the appSettings section of
the Web.config file of the instance.
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
If for some reason you cannot complete instructions in this step, you can create an Acumatica ERP
instance and manually publish the needed customization project, as described in Appendix A: Initial
Configuration.


Part 1: Processing Form (Assign Work Orders) | 12
Part 1: Processing Form (Assign Work Orders)
The Smart Fix company needs to have a custom Acumatica ERP form that the managers of the company will use
to assign repair work orders to particular employees. For this purpose, in this part of the course, you'll create
the Assign Work Orders (RS501000) processing form, which is described in Company Story and Customization
Description.
Lesson 1.1: Creating a Simple Processing Form
In this lesson, you'll create a simple processing form that displays the records to be processed and does not have
any filtering parameters. You'll create the Assign Work Orders (RS501000) custom processing form, which you'll
modify for expanded functionality in future lessons.
Processing Form: General Information
On a processing form, a user can perform an operation on multiple selected records at once.
Applicable Scenarios
You implement a processing form if you need to provide the ability for the user to invoke an operation on multiple
records at once.
Processing Forms
Processing forms look similar to inquiry forms. A processing form usually has the following components:
•
A table (which is also referred to as a grid) that displays the list of records retrieved by the processing data
view. The table includes:
•
A column with an unlabeled check box, which gives the user the ability to select one record or multiple
records in the grid for processing.
•
Additional columns that contain key settings of each listed record, including its ID or number.
•
Optional: A redirection button or link that can be clicked to open the data entry form for any selected
record.
•
Optional: Table filters (also known as quick filters) that are displayed above the table.
•
A form toolbar that includes the Process, Process All, and Cancel buttons.
•
Optional: An area that provides selection criteria (for narrowing the records that are listed and may be
processed) or configuration settings—or both—for the processing method.
The screenshot below shows an example of a processing form with a Selection area and a grid.


Part 1: Processing Form (Assign Work Orders) | 13
Figure: A processing form
Naming Conventions for Processing Forms
Processing forms have IDs that start with a two-letter abbreviation (indicating the functional area of the form)
followed by 50 (indicating a processing form), such as RS501000.
The names of the graphs that work with processing forms have the Process suﬀix. For instance,
RSSVAssignProcess will be the name of the graph for the Assign Work Orders (RS501000) form.
For more details about these naming conventions, see Form and Report Numbering and Graph Naming.
Definition of the Processing Graph and Data View
To configure the graph that works with the processing form, you do the following:
•
You define the data view for the processing form.
To do this, you use the SelectFrom<Table>.ProcessingView class. This class is derived from the
PXProcessingBase<Table> class, which is a base class for the data views of processing forms. You
can also use one of the types that use the traditional BQL style of data queries, such as PXProcessing or
PXProcessingJoin
To ensure the history of a processing operation is saved correctly, the main DAC of the
processing view must contain the NoteID field. This field must have the PXNote attribute
declared on it.
•
You add the Cancel action to the processing graph.
You do this by using the PXCancel class. If the processing form does not have a filter, you use the main DAC
of the processing data view as the type parameter, as shown in the following code.
// Definition of the Cancel button for processing without filtering
public class SalesOrderProcess : PXGraph<SalesOrderProcess>
{
    public SelectFrom<SalesOrder>.ProcessingView SalesOrders;
    // Main DAC of the processing data view
    public PXCancel<SalesOrder> Cancel;
}
•
Optional: You replace the names of the default buttons in the graph constructor.


Part 1: Processing Form (Assign Work Orders) | 14
By default, any form that has a data view of a type derived from PXProcessingBase<Table> has
the Process and Process All buttons on the form toolbar. To override the button captions, you use the
SetProcessCaption() and SetProcessAllCaption() methods, as the following code shows.
public RSSVAssignProcess()
{
    WorkOrders.SetProcessCaption("Assign");
    WorkOrders.SetProcessAllCaption("Assign All");
}
You also need to specify the processing action. For details, see Processing Operations: General Information.
Selected Data Field and Column
You must add the unbound Selected data field of the Boolean type to the DAC that provides the records to
process for the processing form and then add the column for this field to the form. If a user doesn’t want to process
all listed records, the user will select the check box in this column for each record to be processed. You define
the Selected data field as unbound by using the PXBool type attribute. (Unlike the PXDBBool attribute, the
PXBool attribute does not have DB in its name. DB indicates a bound data type.)
Selected is the default name for the data field for this check box; you can define the data field
with any name and override the default Selected name in the graph constructor with the
SetSelected() method of the PXProcessing class.
You make all columns in the grid (except for the column that corresponds to the Selected field) unavailable for
editing by specifying the Processing preset for the grid. For the Selected field, you set the allowCheckAll
property to true. This lets users select all records on the current page of the table for processing by selecting the
check box in the column header.
Related Links
•
Form Types
•
Form and Report Numbering
Processing Form: UI Guidelines
In this topic, you can learn the UI guidelines for the processing forms.
Template, Label Sizes, and Other Layout Settings
By default, you should use the following guidelines while designing a processing form:
•
For the Selection area, you use the 17-17-14 or 17-14-17 template. For more details about templates,
see Form Layout: Predefined Templates.
•
For the Selection area, you use the default label sizes.
•
For the table area, you use the Processing preset. For details about presets, see Form Layout: Grid
Presets.
•
No Activities, Files, and Notes buttons in the form title bar should be displayed.
•
No Files and Notes buttons in the table should be displayed.
For particular forms, the Notes and Files buttons can be required. For example, on the Process
Export Scenarios (SM207035) form, this is the only way a user can download an exported file.


Part 1: Processing Form (Assign Work Orders) | 15
•
Sections in the Selection area can have captions if the captions make sense.
Recommendations for Organizing the Layout
The following table shows recommendations for organizing the layout of a processing form.
Correct
Incorrect
Put all commands on a single toolbar.
Do not separate commands into two toolbars.
 
 
Use a single field tag for an element with the Date
Range label and two date and time controls for the se-
lection of the start date and the end date.
This approach applies only to processing forms.
Do not use two separate fields in a fieldset for the
Start Date and End Date boxes on processing forms.
 
 
UX and Functional Guidelines
The form design should be tailored for screens with a resolution of 1280 x 720.
The number of a processing form should start with 50. For details, see Form and Report Numbering.
Processing Form: A Form with Only a Grid
The following topic describes how to configure a processing form that contains only a table and does not contain
the Selection area or table filters (also called quick filters). (In the Classic UI, these forms were based on the
GridView template.) The following screenshot shows an example of a form with this layout.


Part 1: Processing Form (Assign Work Orders) | 16
Figure: A processing form without the Selection area or table filters
View Definition in TypeScript
For a processing form with only a table (and no Selection area or table filters), you need to use the following in the
TypeScript file of the form:
•
The createCollection method to define a property for the data view to display a table.
•
A class that extends PXView with the full list of fields for the table.
•
The Processing preset for the table. For details about presets, see Form Layout: Grid Presets.
The following code shows an example of this implementation of a processing form.
import {
    PXView, PXFieldState, commitChanges, graphInfo, PXScreen, createCollection
} from "client-controls";
 
@graphInfo({
    graphType: 'PX.Objects.CR.CRActivitySetupMaint',
    primaryView: 'ActivityTypes'
})
export class CR102000 extends PXScreen {
    ActivityTypes = createCollection(ActivityTypes);
}
 
@gridConfig({
    preset: GridPreset.Processing,
    quickFilterFields: ['ClassID', 'Type', 'Description']
})
export class ActivityTypes extends PXView {
    ClassID: PXFieldState
    Type: PXFieldState;
    Description: PXFieldState;
    Active: PXFieldState;
    IsDefault: PXFieldState;


Part 1: Processing Form (Assign Work Orders) | 17
    Application: PXFieldState<PXFieldOptions.CommitChanges>;
    ImageUrl: PXFieldState;
    PrivateByDefault: PXFieldState<PXFieldOptions.CommitChanges>;
    RequireTimeByDefault: PXFieldState<PXFieldOptions.CommitChanges>;
    Incoming: PXFieldState;
    Outgoing: PXFieldState;
}
Layout in HTML
For a processing form with only a table (and no Selection area or table filters), you define the layout by adding one
qp-grid control to the HTML code of the form, as shown in the following example.
<template>
    <qp-grid id="grid" view.bind="ActivityTypes">
    </qp-grid>
</template>
Activity 1.1.1: To Create a Simple Processing Form
The following activity will walk you through the process of creating the UI of a simple processing form that doesn’t
have any filtering parameters defined.
Story
The Smart Fix company needs to have a custom Acumatica ERP form that the managers of the company will
use to assign repair work orders to particular employees. For this purpose, you’ll create the Assign Work Orders
(RS501000) processing form.
This form will use the RSSVWorkOrder custom table, whose data will be displayed in the table on the form.
Process Overview
In this activity, you'll create a form template, add the unbound Selected data field to the DAC that will be used
for the table on the form, implement the screen class and view class for the form, and define the layout of the form
in the HTML file of the form.
Step 1: Creating the Form (Self-Guided Exercise)
In this step, you'll create the Assign Work Orders (RS501000) form on your own. Although this is a self-guided
exercise, this step provides details and suggestions you can use as you create the form. The creation of a form is
described in detail in the T200 Maintenance Forms training course.
If you are using the Customization Project Editor to complete the self-guided exercise, you can perform the
following general instructions:
1. In the PhoneRepairShop customization project, create the form and graph as follows:
a. On the toolbar of the Screens page of the Customization Project Editor, click Create New Screen.
b. In the Create New Screen dialog box, which opens, specify the following values:
•
Screen ID: RS.50.10.00
•
Graph Name: RSSVAssignProcess
•
Graph Namespace: PhoneRepairShop


Part 1: Processing Form (Assign Work Orders) | 18
•
Page Title: Assign Work Orders
•
Template: Grid (GridView)
•
Create Modern UI Files: Selected
c. Move the generated RSSVAssignProcess graph to the extension library.
2. Make sure that the RSSVWorkOrder DAC is defined in the PhoneRepairShop_Code Visual Studio
project.
3. Build the project in Visual Studio.
4. Update the customization project with a new version of PhoneRepairShop_Code.dll, and publish the
customization project.
5. Include a link to the Assign Work Orders form in the Processes category of the Phone Repair Shop
workspace.
6. In the Customization Project Editor, do the following:
•
Verify that the access rights for the Assign Work Orders form were automatically added on the Access
Rights page
•
Update the SiteMapNode item for the Assign Work Orders form
Step 2: Adding the Unbound Selected Data Field
Do the following:
1. In the RSSVWorkOrder DAC, add the unbound Selected data field, as shown in the following code.
        #region Selected
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        [PXBool]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        #endregion
2. Rebuild the project.
Step 3: Defining the Data View
In this step, you'll define the data view in the RSSVAssignProcess graph, which works with the Assign Work
Orders (RS501000) form. Do the following:
1. In the RSSVAssignProcess.cs file, add the following using directive.
using PX.Data.BQL.Fluent;
2. In the RSSVAssignProcess graph, use the following code to define the WorkOrders data view, which
provides the data records to be processed on the form.
        public 
            SelectFrom<RSSVWorkOrder>.
            // Inside the Where condition, use a fluent BQL statement 
            // that selects only the repair work orders with 
            // the Ready for Assignment status. 
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderEntry_Workflow.States.readyForAssignment>>.
            ProcessingView WorkOrders = null!;


Part 1: Processing Form (Assign Work Orders) | 19
3. Rebuild the project.
Step 4: Defining the Buttons for the Form Toolbar
In this step, you'll define the toolbar buttons for the form. Do the following:
1. In the RSSVAssignProcess graph, define the Cancel action for the toolbar (shown below).
        public PXCancel<RSSVWorkOrder> Cancel = null!;
Remove the existing Save and Cancel actions that were auto-generated when you created the graph in
Step 1.
2. In the RSSVAssignProcess graph, change the default names of the processing buttons in the
constructor of the graph as follows.
        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
        }
3. Rebuild the project.
Step 5: Defining the Screen Class of the Form
In the RS501000.ts file, define the view of the Assign Work Orders form by adding a screen class and a property
for the data view of the form. Do the following:
1. Open the RS501000.ts file.
You can open a TypeScript file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)
2. In the RS501000.ts file, make sure the following import directives are included.
import {
 PXScreen, createCollection, graphInfo,
 viewInfo,
} from "client-controls";
3. In the RS501000 screen class, modify the graphInfo decorator, and specify the graph and the primary
view of the form in the decorator properties, as the following code shows.
@graphInfo({
 graphType: "PhoneRepairShop.RSSVAssignProcess",
 primaryView: "WorkOrders"
})
export class RS501000 extends PXScreen {}
4. Define the property for the data view of the form, as the following code shows. For the data view that’s used
to display a table, initialize the property with the createCollection method. The method takes as the
input parameter an instance of the view class, which you'll define in the next step.


Part 1: Processing Form (Assign Work Orders) | 20
The names of the data view properties should be the same as those in the graph. For example,
if the WorkOrders view is declared in the RSSVAssignProcess graph, the property with
the same name should be declared in the RS501000 screen class.
export class RS501000 extends PXScreen {
 @viewInfo({containerName: "Work Orders to Assign"})
 WorkOrders = createCollection(RSSVWorkOrder);
}
In the viewInfo decorator, you’ve specified the name of the container for the table.
Step 6: Defining the View Class of the Form
In the TypeScript file of the form, define the RSSVWorkOrder view class for the table on the Assign Work Orders
form. Proceed as follows:
1. In the RS501000.ts file, add gridConfig, columnConfig, and GridPreset to the list of import
directives.
2. Define the RSSVWorkOrder view class as follows.
export class RSSVWorkOrder extends PXView { 
 @columnConfig({ allowCheckAll: true })
 Selected: PXFieldState; 
 @columnConfig({ hideViewLink: true })
 OrderNbr: PXFieldState;
 Description: PXFieldState;
 @columnConfig({ hideViewLink: true })
 ServiceID: PXFieldState;
 
 @columnConfig({ hideViewLink: true })
 DeviceID: PXFieldState;
 Priority: PXFieldState;
 
 @columnConfig({ hideViewLink: true})
 Assignee: PXFieldState;
}
For the Selected field, in the columnConfig decorator, you’ve specified that a user can select all
records on the page by clicking the check box in the column header.
For the OrderNbr, ServiceID, DeviceID, and Assignee fields in the columnConfig decorator, you
have specified that the selector link should not be displayed.
3. Add the gridConfig decorator to the RSSVWorkOrder view class, as the following code shows. In
the gridConfig decorator, you must specify the preset property. Because the table is used on the
processing form, you’ll use the Processing preset. For details about presets, see Form Layout: Grid Presets.
@gridConfig({
 preset: GridPreset.Processing,
 autoAdjustColumns: true
})
export class RSSVWorkOrder extends PXView { 
... 


Part 1: Processing Form (Assign Work Orders) | 21
}
In the gridConfig decorator, you’ve also specified that the table width should be adjusted to the screen
width. Otherwise, some of the columns could be too narrow to display values.
4. Save your changes.
Step 7: Creating the Layout of the Form
In this step, you’ll define the layout of the Assign Work Orders (RS501000) processing form.
1. Open the RS501000.html file.
You can open an HTML file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)
2. To define the table showing repair work orders, do the following in the RS501000.html file:
a. Within the template tag, modify the qp-grid tag and bind it to the WorkOrders view, as the
following code shows.
  <qp-grid id="grid-WorkOrders" view.bind="WorkOrders"> </qp-grid>
b. Save your changes.
3. If you used the development folder to modify the TypeScript and HTML files in the preceding Steps 5 to 7,
you need to update these files in the customization project. You do this by using the Detect Modified Files
button on the Modern UI Files page.
4. Publish the customization project.
5. Optional: In the RS501000.ts file, remove the MasterView =
createSingle(MasterViewClass); and DetailsView =
createCollection(DetailsViewClass); properties from the screen class. Also, remove the
MasterViewClass and DetailsViewClass classes. These properties and classes are part of the
boilerplate code that was generated by the system.
Publish the customization project again.
6. Open the Assign Work Orders (RS501000) form and review its layout.
Lesson 1.2: Implementing the Processing Operation
In this lesson, you'll implement a processing operation for the Assign Work Orders (RS501000) processing form and
test the form. You'll try two implementation approaches.
Processing Operations: General Information
On a processing form, a user can invoke an operation to be performed on multiple selected records at once. For
instance, a processing operation can be a procedure that modifies the status of documents.


Part 1: Processing Form (Assign Work Orders) | 22
Applicable Scenarios
You implement a processing operation if you need to create a processing form.
Specifying the Processing Operation
A processing operation is defined as a method that is invoked when a user clicks a processing button on the form
toolbar of a processing form. You can specify the processing operation in one of the following ways:
•
If no workflow is implemented for the records that you are going to process on the form, you specify the
processing delegate by using one of the SetProcessDelegate methods. For details, see Processing
Operations: Specifying a Processing Delegate.
•
If a workflow is implemented for the records that you are going to process on the form, you invoke one
of the SetProcessWorkflowAction methods. For more information, see Processing Operations:
Specifying a Workflow Action.
To ensure the history of a processing operation is saved correctly, the main DAC of the processing
view must contain the NoteID field. This field must have the PXNote attribute declared on it.
Processing Operations: Specifying a Workflow Action
If a workflow is implemented for the records that you are going to process on a processing form, you need to
specify a workflow action to define a processing operation for the form.
Specifying the Workflow Action
To specify a workflow action for processing, you invoke one of the SetProcessWorkflowAction methods of
the data view that has a type derived from PXProcessingBase<Table>.
You can specify the workflow action to be used for processing in the RowSelected event handler of the main DAC
of the primary data view.
We recommend that you not call the SetProcessWorkflowAction method in the graph
constructor because this could cause incorrect initialization of the workflow.
In the SetProcessWorkflowAction method, you specify the processing action, as shown in the following
example. You can also pass the values that are specified in the filter on the processing form to the method.
protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
{
    WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
        graph => graph.Assign);
}
Action Definition in the Workflow
You modify the action definition in the workflow so that the action can be used on the processing form as follows:
•
You call the MassProcessingScreen<>() method with the processing graph as the type parameter.
•
You call the InBatchMode() method if the workflow action works with the list of records.
See an example below.


Part 1: Processing Form (Assign Work Orders) | 23
actions.Add(graph => graph.Assign, action => action
    .WithCategory(processingCategory)
    .MassProcessingScreen<RSSVAssignProcess>()
    .InBatchMode());
Processing Operations: Specifying a Processing Delegate
If no workflow is implemented for the records that you are going to process on a processing form, you need to
specify a processing delegate to define a processing operation for the form.
Specifying the Processing Delegate
You specify the processing delegate by using one of the SetProcessDelegate methods of the data view that
has a type derived from PXProcessingBase<Table>.
You can specify the processing delegate in the graph constructor or the RowSelected event handler of the main
DAC of the primary data view.
If a user can select a method to process records on a processing form, you must use the
RowSelected event handler to specify the selected method as the processing delegate.
Implementing the Processing Delegate
The processing delegate, which is passed into a SetProcessDelegate method, can have one of the following
delegate types:
•
delegate void ProcessItemDelegate<Graph>(Graph graph, Table item)
You can use the specified graph and item objects within the processing method. When the processing of
records is initiated, Acumatica Framework creates a graph instance of the specified type and invokes the
delegate for each data record that should be processed. A single graph instance is used for the processing
of all records. Therefore, you have to clear the graph state by calling the Clear() method before the
invocation of the processing method within the delegate.
•
delegate void ProcessListDelegate(List<Table> list, 
    CancellationToken ct);
You can work with the list of processed records of the specified List<Table> type. You can reorder the
records in the list before processing, as well as check dependencies between records during processing.
To make sure the processing result for each record is correctly passed to the UI, inside the processing
delegate, you use the PXProcessing<>.ProcessRecords method. With this method, you do not need
to use the methods of the PXProcessing class to display messages during processing or throw an error
as a result of processing. The default messages and error handling are used. However, you can override the
default behavior in this method.
The processing delegate can have a second parameter of the CancellationToken type; you can use this
parameter to initiate the cooperative cancellation by calling the ThrowIfCancellationRequested
method.
Example: A Non-Static Processing Method and Its Invocation in the Processing Delegate
In this example, a non-static processing method is defined in the data entry graph. The method works with a single
record (see the following code).


Part 1: Processing Form (Assign Work Orders) | 24
// The data entry graph
public class SalesOrderEntry : PXGraph<SalesOrderEntry, SalesOrder>
{
    ...
    // A non-static method that works with a single record
    public void ApproveOrder(SalesOrder order, bool isMassProcess = false)
    {
         // Process the record here
    }
}
You invoke this method inside the processing delegate as follows.
// The processing graph
public class SalesOrderProcess : PXGraph<SalesOrderProcess>
{
    public PXProcessing<SalesOrder> SalesOrders = null!;
    ...
    public SalesOrderProcess()
    {
        // Set the processing delegate for a data view of 
        // the PXProcessing or derived type
        SalesOrders.SetProcessDelegate<SalesOrderEntry>(
            delegate(graph, order) => 
         {
            // Because a single graph instance is used 
            // for the processing of all records, 
            // you have to clear the graph state 
            // before the invocation of the processing method.
            graph.Clear();
            graph.ApproveOrder(order, true);
        });
    }
}
Example: A Static Processing Method and Its Invocation in the Processing Delegate
When you use a static processing method, you have to manually create a new graph object once and reuse it
throughout the whole static method (see the following code).
// The processing graph
public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
{
    // A static processing method that works with the list of records
    public static void AssignOrders(List<RSSVWorkOrder> list,
        bool isMassProcess = false)
    {
        // Create a new graph object only once.
        var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
                
        // Process the records. The processing method uses the error handling
        // and progress tracking functionality of the PXProcessing class.
        PXProcessing<RSSVWorkOrder>.ProcessRecords(list, isMassProcess,
            workOrder =>
        {
            workOrderEntry.Clear();


Part 1: Processing Form (Assign Work Orders) | 25
            workOrderEntry.WorkOrders.Current = workOrder;
            workOrderEntry.Assign.Press();
        });
    }
}
You invoke this static processing method inside the processing delegate as follows.
// The processing graph
public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
{
    public SelectFrom<RSSVWorkOrder>.ProcessingView WorkOrders = null!;
    public RSSVAssignProcess()
    {
        WorkOrders.SetProcessDelegate(list =>
                AssignOrders(list, true));
    }
}
Activity 1.2.1: To Implement a Processing Operation by Using the Workflow
The following activity will walk you through the process of implementing a processing operation by using the
workflow. Usually you use this approach if a workflow is implemented for the records that you are going to process
on a processing form.
Story
You’ve created the UI of the Assign Work Orders (RS501000) processing form. Now you need to implement the
processing operation for this form. A user can assign a repair work order on the Repair Work Orders (RS301000)
form, where a workflow is implemented for the records. Because repair work order records have a workflow
implemented, you’ve decided to implement the processing operation for the Assign Work Orders (RS501000)
processing form by using this workflow.
Process Overview
In this activity, you'll define the processing operation by using the workflow. You’ll also test the form.
Step 1: Specifying a Workflow Action for Processing
In this step, you'll specify the Assign action of the Repair Work Orders (RS301000) form to be used for processing
on the Assign Work Orders (RS501000) form. You'll also adjust the workflow of the Repair Work Orders (RS301000)
form. Do the following:
1. In the RSSVAssignProcess graph, define the following RowSelected event handler.
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
                graph => graph.Assign);
        }


Part 1: Processing Form (Assign Work Orders) | 26
2. In the RSSVWorkOrderEntry_Workflow class, in the lambda expression for the WithActions
method, adjust the Assign action definition so that it can be used on the Assign Work Orders (RS501000)
processing form as follows.
     actions.Add(graph => graph.Assign, action => action
         .WithCategory(processingCategory)
         .WithForm(formAssign)
         .WithFieldAssignments(fields => {
             fields.Add<RSSVWorkOrder.assignee>(field =>
                 field.SetFromFormField(formAssign, "Assignee"));
         })
         .MassProcessingScreen<RSSVAssignProcess>()
         .InBatchMode());
3. Rebuild the project.
Step 2: Specifying the Default Value for the Workflow Action
The Assign action of the Repair Work Orders (RS301000) form assigns a repair work order to the employee
selected in a dialog box displayed for the action. Because a user cannot select the assignee for each record during
processing on the processing form, you need to assign the default value for the assignee in the dialog box. The
default value will be the assignee specified in the Assignee box on the Repair Work Orders (RS301000) form.
The Assignee box will obtain its default value from the default employee specified on the Repair Work Order
Preferences (RS101000) form. A user can change the assignee in the Assignee box.
In this step, you'll configure the RSSVAssignProcess graph, which works with the Assign Work Orders
(RS501000) form, and adjust the workflow of the form. Do the following:
1. In the RSSVWorkOrder DAC, add the PXDefault attribute to the Assignee field. The default value
of the Assignee field is the value of the DefaultEmployee field of the RSSVSetup DAC, which is
specified on the Repair Work Order Preferences (RS101000) form.
        #region Assignee
        [Owner(DisplayName = "Assignee")]
        [PXDefault(typeof(RSSVSetup.defaultEmployee))]
        public virtual int? Assignee { get; set; }
        public abstract class assignee : PX.Data.BQL.BqlInt.Field<assignee> { }
        #endregion
2. In the RSSVWorkOrderEntry_Workflow class, specify the default value for the field in the dialog box
for the Assign action as follows. The DefaultValueFromSchemaField method specifies that the
default value is the value of the Assignee field of the RSSVWorkOrder DAC.
            var formAssign = context.Forms.Create("FormAssign", form =>
                form.Prompt("Assign").WithFields(fields =>
                {
                    fields.Add("Assignee", field => field
                       .WithSchemaOf<RSSVWorkOrder.assignee>()
                       .DefaultValueFromSchemaField()
                       .IsRequired()
                       .Prompt("Assignee"));
                }));
3. Rebuild the project.


Part 1: Processing Form (Assign Work Orders) | 27
Step 3: Testing the Processing Form
In this step, you'll test the Assign Work Orders (RS501000) form. Do the following:
1. On the Repair Work Orders (RS301000) form, do the following:
a. Open the 000001 repair work order. Specify Beauvoir, Layla as the assignee and click Remove Hold.
b. Open the 000002 repair work order. Specify Baker, Maxwell as the assignee and click Remove Hold.
2. Open the Assign Work Orders (RS501000) form. Select the check box in the first column for the 000001 work
order and click Assign on the form toolbar. The Assign dialog box opens (shown below).
Figure: The Assign dialog box
Leave the check box in the dialog box unselected, which indicates that the default value of the Assignee
field should be used to process the record. In the previous step you have already specified that this
field should use the value specified in the Assignee field of the RSSVWorkOrder DAC by calling the
.DefaultValueFromSchemaField() method.
Click Ok.
3. The Processing dialog box is displayed, which shows the progress and then the result of the operation (see
below). Close the dialog box.


Part 1: Processing Form (Assign Work Orders) | 28
Figure: The Processing dialog box
4. On the Repair Work Orders form, make sure that the 000001 work order now has the Assigned status and
that the assignee is Beauvoir, Layla, as shown below.
Figure: Assigned work order
Activity 1.2.2: To Implement a Processing Operation by Using a Delegate
The following activity will walk you through the process of implementing a processing operation by using a
delegate. Usually you use this approach if no workflow is implemented for the records that you are going to process
on a processing form.
Story
You’ve created the UI of the Assign Work Orders (RS501000) processing form. Now you need to implement the
processing operation for this form. A user can assign a repair work order on the Repair Work Orders (RS301000)


Part 1: Processing Form (Assign Work Orders) | 29
form, where a workflow is implemented for the records. You'll reuse the workflow action from the Repair Work
Orders form in the processing delegate.
Process Overview
In this activity, you'll define the processing operation and specify the processing delegate for the form. You’ll also
test the form.
Step 1: Defining the Processing Operation
In this step, you'll define the processing operation for the Assign Work Orders (RS501000) form as follows:
1. In the RSSVAssignProcess graph, define the AssignOrders() static method as follows.
        public static void AssignOrders(List<RSSVWorkOrder> list,
            bool isMassProcess = false)
        {
            var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
                
            // The processing method uses the error handling
            // and progress tracking functionality of the PXProcessing class.
            PXProcessing<RSSVWorkOrder>.ProcessRecords(list, isMassProcess,
                workOrder =>
                {
                    workOrderEntry.Clear();
                    workOrderEntry.WorkOrders.Current = workOrder;
                    // If the assignee is not specified,
                    // specify the default employee.
                    if (workOrder.Assignee == null)
                    {
                        // Retrieve the record with the default setting
                        RSSVSetup setupRecord =
                            workOrderEntry.AutoNumSetup.Current;
                        workOrder.Assignee = setupRecord.DefaultEmployee;
                    }
                    // Assign the work order in the cache.
                    workOrderEntry.Assign.Press();
                });
        }
2. Remove the PXDefault attribute from the Assignee field of the RSSVWorkOrder DAC. You do
not need to default the value of this field using the DAC because it is now done in the code of the
AssignOrders() method that you defined in the preceding instruction. This also allows you to leave the
Assignee box empty when you create a repair work order.
3. Rebuild the project.
Step 2: Specifying the Processing Delegate
In this step, you'll specify the processing delegate in the constructor of the RSSVAssignProcess graph. Do the
following:
1. In the RSSVAssignProcess graph, make sure you have defined the Cancel action for the toolbar and
the WorkOrders data view, which provides the data records to be processed on the form.
        public PXCancel<RSSVWorkOrder> Cancel = null!;
        public 


Part 1: Processing Form (Assign Work Orders) | 30
            SelectFrom<RSSVWorkOrder>.
            // Inside the Where condition, use a fluent BQL statement 
            // that selects only the repair work orders with 
            // the Ready for Assignment status. 
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderEntry_Workflow.States.readyForAssignment>>.
            ProcessingView WorkOrders = null!;
2. In the RSSVAssignProcess graph, define the constructor of the graph as follows.
        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate(list =>
                AssignOrders(list, true));
        }
3. Remove the RowSelected event handler.
4. Rebuild the project.
Step 3: Testing the Processing Form
In this step, you'll test the modified Assign Work Orders (RS501000) form. Do the following:
1. Create a repair work order with the following settings on the Repair Work Orders (RS301000) form:
•
Customer ID: C000000001
•
Service: Battery Replacement
•
Device: Nokia 3310
•
Description: Battery replacement, Nokia 3310
2. Leave the Assignee box empty.
3. Click Remove Hold.
4. On the Assign Work Orders form, make sure that two work orders are displayed on the form. Click Assign All
on the form toolbar. The Processing dialog box shows that two records have been processed (see below).


Part 1: Processing Form (Assign Work Orders) | 31
Figure: Assigned work orders
5. Make sure that for the 000002 work order, the assignee is Baker, Maxwell, which has been specified in the
work order. Notice that for the other work order, the assignee is Becher, Joseph—the default assignee
specified on the Repair Work Order Preferences (RS101000) form.
Lesson 1.3: Adding Filtering Parameters to the Processing Form
In this lesson, you'll modify the Assign Work Orders (RS501000) processing form so that it has filtering parameters,
which a user can use to filter the repair work orders in the table on the form. You'll also define the Assignee column
of the table to be editable, so that a user of the form can use this column to select an assignee for any listed repair
work order.
Filtering Parameters: General Information
When a user specifies selection criteria on an inquiry or processing form, the table displays the data narrowed by
the specified criteria. This gives the user the ability to view the most relevant data. On a processing form, they can
then process all of the listed records or only those they select.
For both of these types of forms, you can define filtering parameters to give users the ability to filter the data listed
in the table. You do this by specifying the filtering parameters for the elements to be used to narrow the data.
Learning Objectives
In this chapter, you'll learn how to add filtering parameters to a form.
Applicable Scenarios
You add filtering parameters in the following cases:
•
For a processing form, you need to provide the ability for the user to filter the records before processing
some or all of them.


Part 1: Processing Form (Assign Work Orders) | 32
•
For an inquiry form, you want to give users the ability to view narrowed data to meet their current
information needs, and reusable filters aren’t suﬀicient to provide the needed functionality.
DAC with Filtering Parameters
The data access class (DAC) you’ll define for filtering parameters should:
•
Contain the fields that correspond to the filtering parameters
•
Contain only unbound fields because you won’t retrieve the parameters’ values from the database
•
Not contain any key fields because the DAC works with only one data record, which represents the current
filtering parameters.
You usually assign the PXHidden attribute to the filter DAC because you don’t need this DAC to be used in generic
inquiries and reports.
To be able to use the filtering parameters in a BQL query, you need to make sure that the fields defined in this DAC
are added to the grid view DAC of the form.
Filter Data View
You’ll use generic PXFilter type of data view to provide filtering parameters for user selection on an Acumatica
ERP form, such as an inquiry form or a processing form. This data view:
•
Always creates a single data record with the current values of the filtering parameters; it never retrieves this
data record or saves it to the database.
•
Is used to specify values that are used by the application logic or other data views and that never should be
stored anywhere except the current user session.
The data view object for the filtering parameters is defined in a graph, as the following code shows.
public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
{
    //RSSVWorkOrderToAssignFilter is a DAC with filtering parameters
    public PXFilter<RSSVWorkOrderToAssignFilter> Filter;
    ...
}
You must not use the PXFilter data view type with a DAC that has at least one key field defined—
that is, a DAC that contains fields with the IsKey=true parameter in the type attribute.
Other Graph Members
To display the filtered data, the graph must contain the data view that selects the records that meet the criteria
specified by the filtering parameters.
To clear the filtering parameters on the form, you define the Cancel action for the filter DAC; see the following
code example.
public PXFilter<RSSVWorkOrderToAssignFilter> Filter;
// Adds the form toolbar button that clears the filtering parameters
public PXCancel<RSSVWorkOrderToAssignFilter> Cancel;
You must also override the IsDirty property of the graph to make the IsDirty property always return false.
This disables the dialog box that confirms that a user wants to leave the form. This dialog box appears when a
user attempts to close the form if there are unsaved changes in the cache objects for the form. A false value in the
IsDirty property of the graph means that there are no unsaved changes on the form and that the dialog box


Part 1: Processing Form (Assign Work Orders) | 33
never appears. This dialog box isn't needed on processing and inquiry forms, which aren’t intended for data entry
or editing.
You can also use the PXUIFieldAttribute.SetEnabled<>() method in the graph constructor to enable
editing for particular data fields.
Changes to the UI Files
Filter data fields are usually displayed on a form. To immediately refresh data records as soon a user updates a
filtering parameter, you need to enable callback for the input control that displays the filtering parameter on the
form. You use the PXFieldOptions.CommitChanges option for a field in TypeScript to enable callback.
For the UI of a processing form with filtering parameters, you should follow the guidelines in Processing Form:
A Form with a Selection Area and a Grid. For an inquiry form, you should follow the same guidelines. The only
diﬀerence is that you need to use the Inquiry preset for the table on the inquiry form.
Implementation Summary
To add a filter to a form, you generally complete the following steps:
1. You define the DAC that provides the filtering parameters.
2. You modify the DAC that provides records for filtering by adding the fields that correspond to the filtering
parameters.
3. In the graph, you define the following members:
•
The Cancel action.
•
The data view of the PXFilter type, which provides data for the filter.
•
The data view that retrieves filtered records.
4. In the graph, you modify the following members:
•
The graph constructor: To enable editing of particular columns in the table with the filtered results
•
The IsDirty property: To disable the dialog box that confirms that a user wants to leave the form
5. In the TypeScript file, you do the following:
•
Add the view class with the fields that correspond to the filtering parameters. For each field, you specify
the CommitChanges property.
•
Add the view property in the screen class. The view property is initialized with the createSingle
method, which takes as the input parameter an instance of the view class.
6. In the HTML file, you add a qp-template element to display the elements of the Selection area.
Activity 1.3.1: To Add a Filter for a Processing Form
The following activity will walk you through the process of adding a filter for a processing form.
Story
The Smart Fix company needed a custom Acumatica ERP form where the managers of the company will assign
repair work orders to particular employees. Suppose that for this purpose, you’ve already implemented the custom
Assign Work Orders (RS501000) processing form. Now you need to add filtering parameters to this form so that the
managers can narrow the range of listed repair work orders before processing.
The Selection area of the form should contain the following UI elements:


Part 1: Processing Form (Assign Work Orders) | 34
•
Priority: If a user selects a priority in this box, the form’s table displays only the repair work orders with this
priority. If no priority is selected, repair work orders with all priority values can be displayed in the table.
•
Minimum Number of Days Unassigned: If a user types a number in this box, the table displays only the
repair work orders that have been unassigned for the specified number of days or longer.
•
Service: If a user selects a service in this box, the table displays only the repair work orders in which this
service is selected.
You need to create filtering parameters for these UI elements.
In addition, you’ll add the Number of Days Unassigned column to the table on the form. This value should not
be stored in the database; you should instead use the PXDBCalced attribute to calculate the value from the date
when the repair work order was created.
You also need to define the Assignee column of the table to be editable so that a user of the form can use this
column to select an assignee for any listed repair work order.
Process Overview
You'll add filtering parameters to the Assign Work Orders (RS501000) processing form by performing the following
steps:
1. Extending the RSSVWorkOrder DAC with the new TimeWithoutAction field
2. Defining the filter DAC
3. Defining the data views for the form
4. Adjusting the TypeScript and HTML files of the form
Finally, you'll test the filter.
Step 1: Extending the DAC with a New Field (Using PXDBCalced)
In this step, you’ll add the TimeWithoutAction field, which holds the number of days that have passed from
the date when the repair work order was created. In the RSSVWorkOrder.cs file, add the new field as follows:
1. In the RSSVWorkOrder class, define the TimeWithoutAction field, as shown in the following code.
        #region TimeWithoutAction
        [PXInt]
        [PXDBCalced(
            typeof(RSSVWorkOrder.dateCreated.Diff<Now>.Days),
            typeof(int))]
        [PXUIField(DisplayName = "Number of Days Unassigned")]
        public virtual int? TimeWithoutAction { get; set; }
        public abstract class timeWithoutAction :
        PX.Data.BQL.BqlInt.Field<timeWithoutAction>
        { }
        #endregion
To calculate the value of the TimeWithoutAction field, you’ve used the PXDBCalced attribute. The
field’s value is calculated during the retrieval of each RSSVWorkOrder record from the database. It’s
calculated as the diﬀerence between the value of the RSSVWorkOrder.DateCreated field and the
current date, for which you’ve used the Now BQL constant. For more information on the PXDBCalced
attribute, see Ad Hoc SQL for Fields.
2. Build the project.


Part 1: Processing Form (Assign Work Orders) | 35
Step 2: Defining the Filter DAC
In this step, you'll define the RSSVWorkOrderToAssignFilter DAC, which will be used to display filtering
parameters on the Assign Work Orders (RS501000) form. The DAC will contain three fields (ServiceID,
TimeWithoutAction, and Priority) that correspond to the filtering parameters. To define the filter DAC, do
the following:
1. In the RSSVAssignProcess graph, define the RSSVWorkOrderToAssignFilter data access class as
follows.
        [PXHidden]
        public class RSSVWorkOrderToAssignFilter : PXBqlTable, IBqlTable
        {
            #region Priority
            [PXString(1, IsFixed = true)]
            [PXUIField(DisplayName = "Priority")]
            [PXStringList(
            new string[]
            {
                WorkOrderPriorityConstants.High,
                WorkOrderPriorityConstants.Medium,
                WorkOrderPriorityConstants.Low
            },
            new string[]
            {
                Messages.High,
                Messages.Medium,
                Messages.Low
            })]
            public virtual string? Priority { get; set; }
            public abstract class priority :
            PX.Data.BQL.BqlString.Field<priority>
            { }
            #endregion
            #region TimeWithoutAction
            [PXInt]
            [PXUnboundDefault(0)]
            [PXUIField(DisplayName = "Minimum Number of Days Unassigned")]
            public virtual int? TimeWithoutAction { get; set; }
            public abstract class timeWithoutAction :
            PX.Data.BQL.BqlInt.Field<timeWithoutAction>
            { }
            #endregion
            #region ServiceID
            [PXInt()]
            [PXUIField(DisplayName = "Service")]
            [PXSelector(typeof(Search<RSSVRepairService.serviceID>),
                typeof(RSSVRepairService.serviceCD),
                typeof(RSSVRepairService.description),
                SubstituteKey = typeof(RSSVRepairService.serviceCD),
                DescriptionField = typeof(RSSVRepairService.description))]
            public virtual int? ServiceID { get; set; }
            public abstract class serviceID :
            PX.Data.BQL.BqlInt.Field<serviceID>
            { }


Part 1: Processing Form (Assign Work Orders) | 36
            #endregion
        }
2. Build the project.
Step 3: Defining the Data Views (with PXFilter and ProcessingView.FilteredBy)
In this step, you’ll prepare the graph members that provide data for the form. Do the following:
1. In the RSSVAssignProcess graph, define the Filter data view of the PXFilter type (as shown
below), which provides the filtering parameters for the processing form.
        public PXFilter<RSSVWorkOrderToAssignFilter> Filter = null!;
2. Replace the definition of the Cancel action so that the action uses the filter DAC.
        public PXCancel<RSSVWorkOrderToAssignFilter> Cancel = null!;
3. Replace the definition of the WorkOrders data view with the following data view of the
ProcessingView.FilteredBy<Table> type, which selects the repair work orders that match the
values of the filtering parameters.
        public 
            SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.status.IsEqual<
                RSSVWorkOrderEntry_Workflow.States.readyForAssignment>.
                And<RSSVWorkOrder.timeWithoutAction.IsGreaterEqual<
                    RSSVWorkOrderToAssignFilter.timeWithoutAction.
                        FromCurrent>.
                And<RSSVWorkOrder.priority.IsEqual<
                    RSSVWorkOrderToAssignFilter.priority.FromCurrent>.
                    Or<RSSVWorkOrderToAssignFilter.priority.FromCurrent.
                        IsNull>>.
                And<RSSVWorkOrder.serviceID.IsEqual<
                    RSSVWorkOrderToAssignFilter.serviceID.FromCurrent>.
                    Or<RSSVWorkOrderToAssignFilter.serviceID.FromCurrent.
                        IsNull>>>>.
           OrderBy<RSSVWorkOrder.timeWithoutAction.Desc,
               RSSVWorkOrder.priority.Desc>.
           ProcessingView.
           FilteredBy<RSSVWorkOrderToAssignFilter> WorkOrders = null!;
4. In the graph constructor, enable editing for the Assignee data field, as shown in the following code.
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignee>(
                WorkOrders.Cache, null, true);
Because you specified the Processing preset for the table in the TypeScript code (in Activity 1.1.1: To
Create a Simple Processing Form), the columns of the table were not defined as being editable. In the graph
constructor, you've made the values in the Assignee column of the table editable. You’ve enabled the
editing of the column in the graph constructor (instead of in RowSelected event handler) because the
column’s UI presentation logic doesn’t depend on the particular values of the data record.
5. Override the IsDirty property of the graph, as the following code shows.
        public override bool IsDirty => false;


Part 1: Processing Form (Assign Work Orders) | 37
You have overridden the IsDirty property of the graph to make it always return false. You have multiple
elements on the form that a user can modify, such as the filtering parameters on the form, the Assignee
column, and the column with the unlabeled check box. That’s why you need to override the IsDirty
property to omit the dialog box that confirms that the user wants to leave the form when it has been edited.
6. Rebuild the project.
Step 4: Adjusting the TypeScript and HTML Code
In this step, you’ll adjust the TypeScript and HTML files of the Assign Work Orders (RS501000) form to display the
filter and the data to be available for processing. Adjust the RS501000.ts and RS501000.html files as follows:
You can perform the following instructions on the Modern UI Files page of the Customization Project
Editor or edit the TypeScript and HTML files of the form in the development folder of your instance
by using an external code editor, such as Visual Studio Code. For details on working with the Modern
UI Files page or editing the code files in the development folder, see the T200 Maintenance Forms
training course. The instructions below are presented in general terms to fit both methods.
1. For the screen class in RS501000.ts, change the value of the primaryView property in the
graphInfo decorator to Filter.
2. Define the property for the data view of the Selection area of the form, as the following code shows. To
initialize the data view of the Selection area of the form, use the createSingle method. It takes as the
input parameter an instance of the view class, which you’ll define in the next step.
 @viewInfo({containerName: "Filter Parameters"})
 Filter = createSingle(RSSVWorkOrderToAssignFilter);
In the viewInfo decorator, you’ve specified the name of the container for the Selection area.
3. You need to define a view class for the data view of the Selection area of the Assign Work Orders form, which
is Filter.
Proceed as follows:
a. In the RS501000.ts file, add createSingle to the list of import directives.
b. Define the RSSVWorkOrderToAssignFilter class as follows.
export class RSSVWorkOrderToAssignFilter extends PXView {}
c. In the view class, specify the properties for all data fields of the data view that should be displayed in the
UI, as shown below. You use the name of the data field as the property name.
export class RSSVWorkOrderToAssignFilter extends PXView {
 Priority: PXFieldState<PXFieldOptions.CommitChanges>;
 TimeWithoutAction: PXFieldState<PXFieldOptions.CommitChanges>;
 ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
}
All fields of the view should be defined so that changes are committed to the server; therefore, you’ve
used the PXFieldOptions.CommitChanges option for the property type.
4. In the RS501000.html file, define the layout of the Selection area by adding the qp-template tag with
the 17-17-14 template within the template tag as follows:
a. For the first two slots, define a fieldset, as shown in the following code. To leave the third slot empty, do
not specify any tags for it.
  <qp-template
    id="form-Filter"


Part 1: Processing Form (Assign Work Orders) | 38
    name="17-17-14"
    class="equal-height"
  >
    <qp-fieldset
      id="fsColumnA-Filter"
      slot="A"
      view.bind="Filter"
      class="label-size-xm"
    >
                                    </qp-fieldset>
    <qp-fieldset id="fsColumnB-Filter" slot="B" view.bind="Filter">
                                    </qp-fieldset>
  </qp-template>
Each fieldset has been bound to the same Filter view.
For details about the qp-template tag and slots, see Form Layout: Predefined Templates.
b. In each fieldset, add the field tags for the fields that should be displayed in the corresponding fieldset,
as the following code shows.
    <qp-fieldset
      id="fsColumnA-Filter"
      slot="A"
      view.bind="Filter"
      class="label-size-xm"
    >
      <field name="Priority"></field>
      <field name="TimeWithoutAction"></field>
    </qp-fieldset>
    <qp-fieldset id="fsColumnB-Filter" slot="B" view.bind="Filter">
      <field name="ServiceID"></field>
    </qp-fieldset>
In the first fieldset, you’ve also specified the width of the labels by using the label-size-xm class. For more
details about CSS classes, see Form Layout: CSS Classes.
c. Save your changes.
5. In the RS501000.ts file, add the TimeWithoutAction field to the RSSVWorkOrder view class, as
shown below, so that the corresponding column is displayed in the table of the Assign Work Orders form.
Also, specify that the changes in the Assignee field should be committed to the server by adding the
PXFieldOptions.CommitChanges option for the property type.
export class RSSVWorkOrder extends PXView {
    ...
                         Assignee: PXFieldState<PXFieldOptions.CommitChanges>;
                         TimeWithoutAction: PXFieldState;
}
6. Save your changes.
7. If you used the development folder to modify the TypeScript and HTML files in the preceding instructions,
you need to update these files in the customization project. You do this by using the Detect Modified Files
button on the Modern UI Files page.
8. Publish the customization project.


Part 1: Processing Form (Assign Work Orders) | 39
Step 5: Testing the Filter
In this step, you'll test the filtering parameters you have implemented for the Assign Work Orders (RS501000) form.
Do the following:
1. On the Repair Work Orders (RS301000) form, create three repair work orders with the settings specified in
the following table. Save each order and then click Remove Hold.
 
Work Order 1
Work Order 2
Work Order 3
Customer ID
C000000001
C000000002
C000000001
Service
Battery Replacement
Screen Repair
Battery Replacement
Device
Nokia 3310
Samsung Galaxy S4
Motorola RAZR V3
Assignee
Beauvoir, Layla
Empty
Baker, Maxwell
Priority
High
Medium
Medium
Description
Test order
Test order
Test order
The created work orders have the Ready for Assignment status.
2. On the Assign Work Orders form, test the filtering parameters as follows:
a. Make sure that the three work orders you have created are displayed on the form.
b. In the Priority box in the Selection area, select High. Make sure one of the created work orders is
displayed in the table, as shown below.
Figure: The work order with a priority of High
c. Clear the filter by clicking Cancel on the form toolbar.
d. In the Minimum Number of Days Unassigned box, type 1. No work orders are displayed in the table (as
long as you haven’t created work orders outside of the instructions of the guide).
e. Change the value in the Minimum Number of Days Unassigned box to 0. Three work orders are
displayed.
f.
In the Service box, select Battery Replacement. Two of the created work orders are displayed in the table.
g. In the Priority box, select Medium. Only one of the created work orders remains in the table.
h. On the form toolbar, click Assign All. The processing dialog box indicates that the work order has been
processed. Make sure it has the Assigned status and is assigned to Baker, Maxwell (the employee you’ve
selected during creation), as shown below.


Part 1: Processing Form (Assign Work Orders) | 40
Figure: The assigned work order
3. Test the Assignee column on the Assign Work Orders form as follows:
a. Clear all the boxes in the Selection area. Two of the created repair work orders are displayed.
b. For a work order with the default assignee (which is Becher, Joseph), in the Assignee column, select
Beauvoir, Layla.
c. On the form toolbar, click Assign All. The processing dialog box shows that two repair work orders have
been processed. Make sure that Beauvoir, Layla is the assignee of both orders, as shown below.


Part 1: Processing Form (Assign Work Orders) | 41
Figure: Two assigned work orders


Part 2: Update of Data with a Custom Accumulator Attribute | 42
Part 2: Update of Data with a Custom Accumulator
Attribute
The functionality of the Assign Work Orders (RS501000) form that has been implemented so far is not enough for
the Smart Fix company. In accordance with the specifications of the managers of the company, the repair work
orders that are processed on the Assign Work Orders form should be automatically assigned to the employee with
the smallest number of repair work orders assigned. If there are multiple employees with the smallest number
of repair work orders assigned, the work orders will be assigned to the first of these employees selected from the
database.
To bring the form closer to these specifications, in this part of the guide, you'll do the following:
•
Modify the Assign Work Orders form so that it contains information about the number of work orders
assigned to potential assignees listed in the table
•
Implement a custom PXAccumulator attribute that will count the number of repair work orders assigned
to each employee and update this number in the database during processing on the Assign Work Orders
form
Aer you complete the lessons of this part, you'll be able to test the updated functionality of the form and the way
the custom PXAccumulator attribute works.
Lesson 2.1: Implementing a Custom PXAccumulator Attribute
In this lesson, you'll learn how to implement a custom attribute derived from the PXAccumulator attribute.
PXAccumulator: General Information
An accumulator attribute is the PXAccumulator attribute or any attribute derived from it. The use of
accumulator attributes is a specific Acumatica Framework technique for fields that are updated frequently (and
oen concurrently by multiple users). An accumulator attribute changes the SQL query that is executed when data
is updated in the database.
Learning Objectives
In this chapter, you'll learn how to do the following:
•
Implement a custom attribute derived from the PXAccumulator attribute.
•
Specify the values of the fields updated by a PXAccumulator attribute.
Applicable Scenarios
You can use an accumulator attribute in either of the following cases:
•
To update a field or multiple fields of a data record without checking for the version of the data record in the
database. (In an ordinary update, the framework generates the SQL statement that checks the time stamp
column, if this column exists in the table.)
•
To define a specific update policy for a column—for instance, to calculate the sum of values in a column on
every update. You can also specify restrictions for a column that will be checked by the database during
update.


Part 2: Update of Data with a Custom Accumulator Attribute | 43
DAC for Accumulated Values
Database access classes (DACs) that are used exclusively for storing accumulated values usually do not contain
audit, time stamp, or NoteID fields. The base PXAccumulatorAttribute class (on which custom attributes
are based) is capable of handling fields of the DateTime type only if they are decorated with one of the following
attributes:
•
PXDBLastModifiedDateTimeAttribute
•
PXDBLastChangeDateTimeAttribute
•
PXDBLastModifiedByScreenIDAttribute
•
PXDBLastModifiedByIDAttribute
Base PXAccumulator Attribute
The base PXAccumulator attribute has two parameters in the constructor. This attribute applies the
Summarize policy to the specified decimal or double fields (that is, on each update, the new value is added to the
database value) and the Initialize policy to other fields. To override this policy or to set restrictions, you derive
a custom attribute class from PXAccumulator and override the PrepareInsert() method in the custom
attribute class.
Use of a PXAccumulator Attribute
You can add an accumulator attribute directly to a DAC that is updated only from code and not through the UI.
If you have a DAC that users can edit through the UI, you cannot assign a PXAccumulator attribute directly to
this DAC. Instead, you should derive a new DAC from the original one and assign the accumulator attribute to this
derived DAC, so that the derived DAC and the original DAC implement the following alternative ways of updating
the related table:
•
All data fields are updated through the original DAC when a record is edited through the UI.
•
The data fields specified in the accumulator attribute are updated through the derived DAC according to the
updating policies defined in the accumulator attribute when a record is edited through the code.
PXAccumulator: Implementation of a Custom PXAccumulator Attribute
When you define a custom accumulator attribute, you typically implement the members that are described in the
following sections.
•
For reference information on methods and properties that you can use in the
PXAccumulator attribute, see PXAccumulatorAttribute Class.
•
For an example of implementation of a custom accumulator attribute, see Activity 2.1.1: To
Implement a Custom Accumulator Attribute.
Attribute Constructor
By setting the value of the _SingleRecord field in the constructor to true, you specify that the system should use
single-record update mode. In this mode, the attribute updates the data record independently from the existing
data records and does not add any restrictions to future data records. In single-record update mode, the framework
generates a specific SQL statement that updates an independent record. By default, single-record mode is not
used.


Part 2: Update of Data with a Custom Accumulator Attribute | 44
PrepareInsert() Method
In the overridden PrepareInsert() method, you first have to invoke the base PrepareInsert() method
to initialize the collection of columns. If the base PrepareInsert() method returns true, the collection of
columns is initialized. Then in the overridden method, you can set restrictions and update policies for specific
columns. For details about policies, see the description of the PXDataFieldAssign.AssignBehavior
enumeration.
In the PrepareInsert() method, the columns are represented by an object of the
PXAccumulatorCollection class. To update a value or to set a restriction for a column, you invoke the
needed generic method of the columns collection. You can use the following methods in single-record mode (that
is, when _SingleRecord = true is specified in the attribute constructor):
•
columns.Update(): Sets the update policy for the field.
•
columns.Restrict(): Sets the value restriction for the column. The restriction triggers
the PXLockViolationException exception, which you should handle in the overridden
PersistInserted() method of the attribute.
PersistInserted() Method
If you set any restrictions, you have to override the PersistInserted() method. For details, see
PXAccumulator: Implementation of an Update with Restrictions.
Activity 2.1.1: To Implement a Custom Accumulator Attribute
The following activity will walk you through the process of implementing a custom accumulator attribute.
Story
In the Smart Fix company, the number of work orders assigned to each employee changes frequently. Users can
assign work orders on both the Repair Work Orders (RS301000) form and the Assign Work Orders (RS501000) form.
Multiple users may assign repair work orders to the same employee at the same time. These concurrent updates of
a record trigger this error: Another process has updated the record.
To avoid this error, you’ll implement a custom attribute derived from the PXAccumulator attribute. The
accumulator attribute modifies the SQL query and guarantees that concurrent updates of each record are handled
smoothly. In this attribute, you’ll calculate the number of repair work orders assigned to each employee and
enforce a maximum of 10.
Process Overview
You'll create a custom accumulator attribute to calculate the numbers of assigned work orders for each employee
during the assignment or completion of work orders.
In the custom attribute, you’ll define the following:
•
The constructor, in which you'll specify the update mode for the records
•
The PrepareInsert() method, in which you’ll define the updating policy for the field and specify the
restriction for the values of this field
You'll also assign to the DAC the custom accumulator attribute that stores the field to be updated by the
accumulator attribute.


Part 2: Update of Data with a Custom Accumulator Attribute | 45
System Preparation
Before you begin implementing a custom PXAccumulator attribute, do the following:
1. Create the database table and include the creation script in the customization project as follows:
a. In SQL Server Management Studio, execute the T240_DatabaseTables.sql script to create the
RSSVEmployeeWorkOrderQty database table.
b. On the Database Scripts page of the Customization Project Editor, for the added table, do the following:
a. On the page toolbar, click Add Custom Table Schema.
b. In the dialog box that opens, select the RSSVEmployeeWorkOrderQty table and click OK.
c. Publish the project.
For details on designing database tables for Acumatica ERP, see Designing the Database
Structure and DACs.
2. In the system, indicate the completion of all repair work orders that have the Assigned status on the Repair
Work Orders (RS301000) form. Do the following for each of the repair work orders that has the Assigned
status:
a. Open the repair work order.
b. Click Complete on the form toolbar.
The RSSVEmployeeWorkOrderQty table will hold the number of repair work orders assigned to each
employee. This table currently contains no data because you haven’t yet implemented the logic to update
the data in the table. However, if any repair work orders are assigned to employees, you need to complete
them. As a result, none of the employees have repair work orders assigned and the table’s data correctly
reflects the current state of the system.
Step 1: Creating a DAC (Self-Guided Exercise)
During system preparation, you’ve created the RSSVEmployeeWorkOrderQty database table, whose
NbrOfAssignedOrders column will be updated by the custom accumulator attribute. In this step, you’ll create
a data access class for this table.
For details on creating a DAC, see the T200 Maintenance Forms training course.
As you create the RSSVEmployeeWorkOrderQty DAC, you’ll perform the following general actions:
1. Create the RSSVEmployeeWorkOrderQty data access class and define its single system
field: LastModifiedDateTime. (For more information about the definition of the
LastModifiedDateTime system field, see Audit Fields in the documentation.)
2. For the DAC, specify the PXHidden attribute, which indicates that the DAC won’t be used for reports or
generic inquiries.
3. In the DAC, define the UserID and NbrOfAssignedOrders fields and their attributes as follows:
•
Mark the UserID field as the key field, as shown in the following code.
        #region UserID
        [PXDBInt(IsKey = true)]
        public virtual int? UserID { get; set; }
        public abstract class userID : PX.Data.BQL.BqlInt.Field<userID> { }
        #endregion


Part 2: Update of Data with a Custom Accumulator Attribute | 46
•
Don’t specify any display names for the fields because they won’t be displayed in the UI.
Step 2: Implementing the Accumulator Attribute
In this step, you’ll create the custom RSSVEmployeeWorkOrderQtyAccumulator accumulator attribute for
the RSSVEmployeeWorkOrderQty DAC. For each employee, this attribute will compute the total of the number
of assigned work orders and save the value in the RSSVEmployeeWorkOrderQty.NbrOfAssignedOrders
field.
The attribute will be derived from the PXAccumulator system attribute. The base attribute can also be
configured to calculate the values in the RSSVEmployeeWorkOrderQty.NbrOfAssignedOrders field.
You'll instead use the custom attribute because you need to specify a custom restriction for the number of work
orders assigned to an employee—a maximum of 10.
To implement the custom accumulator attribute, do the following:
1. In the Messages.cs file, add the following constant with the message that’s displayed when the
restriction specified in the accumulator attribute is violated.
        public const string ExceedingMaximumNumberOfAssignedWorkOrders =
            @"Updating the number of assigned work orders for the employee 
            will lead to exceeding of the maximum number of assigned work orders, 
            which is 10.";
2. In the RSSVEmployeeWorkOrderQty.cs file, define the
RSSVEmployeeWorkOrderQtyAccumulator attribute as follows.
    public class RSSVEmployeeWorkOrderQtyAccumulator :
        PXAccumulatorAttribute
    {
        //Specify the single-record mode of update in the constructor.
        public RSSVEmployeeWorkOrderQtyAccumulator()
        {
            _SingleRecord = true;
        }
        //Override the PrepareInsert method.
        protected override bool PrepareInsert(PXCache sender, object row,
            PXAccumulatorCollection columns)
        {
            if (!base.PrepareInsert(sender, row, columns)) return false;
            RSSVEmployeeWorkOrderQty newQty = (RSSVEmployeeWorkOrderQty)row;
            if (newQty.NbrOfAssignedOrders != null)
            {
                // Add the restriction for the value of 
                // RSSVEmployeeWorkOrderQty.NbrOfAssignedOrders.
                columns.AppendException(
                    Messages.ExceedingMaximumNumberOfAssignedWorkOrders,
                new PXAccumulatorRestriction<
                    RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders>(
                    PXComp.LE, 10));
            }
            // Update NbrOfAssignedOrders by using Summarize.
            columns.Update<RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders>(
                newQty.NbrOfAssignedOrders,
                PXDataFieldAssign.AssignBehavior.Summarize);
            return true;
        }


Part 2: Update of Data with a Custom Accumulator Attribute | 47
    }
3. Add the RSSVEmployeeWorkOrderQtyAccumulator attribute to the
RSSVEmployeeWorkOrderQty class, as shown below.
    [PXHidden]
    [RSSVEmployeeWorkOrderQtyAccumulator]
    public class RSSVEmployeeWorkOrderQty : PXBqlTable, IBqlTable
    {        ...
    }
You have added the custom attribute directly to the RSSVEmployeeWorkOrderQty DAC because this
class is updated only from code and not through the UI.
4. Build the project.
Lesson 2.2: Modifying the Processing Form to Use the Field Updated by
PXAccumulator
In this lesson, you'll modify the Assign Work Orders (RS501000) form so that if no assignee is specified for a repair
work order on the Repair Work Orders (RS301000) form, the system detects the default assignee for this repair work
order as the employee that has the fewest work orders assigned.
You'll also modify the Assign and Complete actions on the Repair Work Orders (RS301000) form. When a user
clicks Assign, the number of assigned work orders for the corresponding assignee will be increased. When a user
clicks Complete, the number of assigned work orders for the assignee will be decreased. These calculations will be
performed by the custom accumulator attribute, which you have implemented in the previous lesson.
Activity 2.2.1: To Replace Field Attributes in CacheAttached
The following activity will walk you through the process of replacing field attributes in CacheAttached event
handlers.
Story
The RSSVWorkOrder DAC holds the data of the Repair Work Orders (RS301000) data entry form. The data of this
DAC is also displayed on the Assign Work Orders (RS501000) processing form. (You have developed both of these
forms for the Smart Fix company.) You need to add the following fields to this DAC:
•
DefaultAssignee: The employee that has the fewest assigned repair work orders
•
AssignTo: The employee the repair work order will be assigned to
On the Assign Work Orders form, you need to calculate the values of these fields by using attributes. You do not
need these calculations for the data entry form.
Process Overview
In this activity, you'll add attributes that calculate the values of the DefaultAssignee and AssignedTo fields
of the RSSVWorkOrder DAC. Because you need these calculations only for the Assign Work Orders (RS501000)
form, you'll add these attributes by using the CacheAttached event handler.


Part 2: Update of Data with a Custom Accumulator Attribute | 48
Step 1: Extending the DAC with New Fields
Add the new fields to the RSSVWorkOrder DAC as follows:
1. In the RSSVWorkOrder class, define the DefaultAssignee field, as shown in the following code.
        #region DefaultAssignee
        [PXInt]
        [PXUIField(DisplayName = "Default Assignee")]
        public virtual int? DefaultAssignee { get; set; }
        public abstract class defaultAssignee :
            PX.Data.BQL.BqlInt.Field<defaultAssignee>
        { }
        #endregion
2. Define the AssignTo field, as shown below.
        #region AssignTo
        [PXInt]
        [PXUIField(DisplayName = "Assign To")]
        public virtual int? AssignTo { get; set; }
        public abstract class assignTo : PX.Data.BQL.BqlInt.Field<assignTo> { }
        #endregion
3. Build the project.
Step 2: Replacing the Attributes
In this step, you'll add the attributes to the RSSVWorkOrder DAC fields by using the CacheAttached
event handlers of these fields in the RSSVAssignProcess graph. These attributes will be used for the
RSSVWorkOrder DAC fields only on the Assign Work Orders (RS501000) form. Instead of completely replacing
attributes, you'll add the needed attributes to the fields by including the PXMergeAttributes attribute in the
list of assigned attributes.
To implement the calculation of field values for the RSSVAssignProcess graph, do the following:
1. In the RSSVAssignProcess.cs file, add the PX.TM using directives.
2. To add the PXDBScalar attribute to the DefaultAssignee field, add the following event handler to the
RSSVAssignProcess graph.
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [Owner(IsDBField = false, DisplayName = "Default Assignee")]
        [PXDBScalar(typeof(SelectFrom<OwnerAttribute.Owner>.
            LeftJoin<RSSVEmployeeWorkOrderQty>.
            On<OwnerAttribute.Owner.contactID.IsEqual<
                RSSVEmployeeWorkOrderQty.userID>>.
            Where<OwnerAttribute.Owner.acctCD.IsNotNull>.
            OrderBy<RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders.Asc,
                RSSVEmployeeWorkOrderQty.lastModifiedDateTime.Asc>.
            SearchFor<OwnerAttribute.Owner.contactID>))]
                protected virtual void _(
            Events.CacheAttached<RSSVWorkOrder.defaultAssignee> e)
        { }
For the system to calculate the value of the DefaultAssignee field, you have used the PXDBScalar
attribute. The PXDBScalar attribute selects the first record that matches the query specified in the


Part 2: Update of Data with a Custom Accumulator Attribute | 49
attribute. In the query, you have selected records in ascending order by the number of assigned work
orders.
To display the employee name instead of its ID (which is an integer) and display the selector for the
column if it is editable, you have assigned the Owner attribute to the DefaultAssignee field. Since
the DefaultAssignee field does not exist in the database, in the Owner attribute, you have specified
IsDBField = false.
3. To add the PXUnboundDefault attribute to the AssignedTo field, add the following event handler to
the RSSVAssignProcess graph.
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [Owner(IsDBField = false, DisplayName = "Assign To")]
        [PXUnboundDefault(typeof(RSSVWorkOrder.assignee.When<
            RSSVWorkOrder.assignee.IsNotNull>.
            Else<RSSVWorkOrder.defaultAssignee>))]
        protected virtual void _(
            Events.CacheAttached<RSSVWorkOrder.assignTo> e)
        { }
The system sets the value of the AssignedTo field to the employee selected for the work order on the
Repair Work Orders (RS301000) form (if the value is not null) or to the default assignee specified in the
DefaultAssignee field (if the value selected on the Repair Work Orders form is null). You have defined
this behavior by using the PXUnboundDefault attribute.
4. Build the project.
Related Links
•
CacheAttached: General Information
•
Ad Hoc SQL for Fields
Activity 2.2.2: To Fetch Calculated Data from a Non-Scalar Source (in RowSelecting)
The following activity will walk you through the process of fetching calculated data from a non-scalar source by
using the RowSelecting event handler.
Story
Suppose that you need to fetch the values for the Number of Assigned Work Orders column, which is
displayed on the Assign Work Orders (RS501000) form. (You have developed this form for the Smart Fix
company.) You need to write a fluent BQL query that fetches the number of assigned work orders from the
RSSVEmployeeWorkOrderQty DAC for the employee selected in the AssignTo field of the RSSVWorkOrder
DAC.
Process Overview
To fetch the needed values, you'll use the RowSelecting event handler. In the event handler, you'll retrieve the
number of assigned work orders for the employee selected in the AssignTo field of the RSSVWorkOrder DAC.
Step 1: Extending the RSSVWorkOrder DAC
Add the new field to the RSSVWorkOrder DAC as follows:
1. Define the NbrOfAssignedOrders field, as the following code shows.
        #region NbrOfAssignedOrders


Part 2: Update of Data with a Custom Accumulator Attribute | 50
        [PXInt]
        [PXUIField(DisplayName = "Number of Assigned Work Orders")]
        public virtual int? NbrOfAssignedOrders { get; set; }
        public abstract class nbrOfAssignedOrders :
            PX.Data.BQL.BqlInt.Field<nbrOfAssignedOrders>
        { }
        #endregion
2. Build the project.
Step 2: Fetching Values for the NbrOfAssignedOrders Field
Modify the RSSVAssignProcess graph as follows:
1. In the RSSVAssignProcess.cs file, add the PX.Data.BQL using directive.
2. In the graph, define the following RowSelecting event handler.
        protected virtual void _(Events.RowSelecting<RSSVWorkOrder> e)
        {
            using (new PXConnectionScope())
            {
                if (e.Row == null) return;
                    RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                        SelectFrom<RSSVEmployeeWorkOrderQty>.
                        Where<RSSVEmployeeWorkOrderQty.userID.IsEqual<@P.AsInt>>.
                        View.Select(this, e.Row.AssignTo);
                if (employeeNbrOfOrders != null)
                {
                    e.Row.NbrOfAssignedOrders = 
                      employeeNbrOfOrders.NbrOfAssignedOrders.GetValueOrDefault();
                }
                else
                {
                    e.Row.NbrOfAssignedOrders = 0;
                }
            }
        }
3. Build the project.
Activity 2.2.3: To Modify the Processing Form to Use the Field Updated by
PXAccumulator
The following activity will walk you through the modification of the processing form to use the field updated by the
custom PXAccumulator attribute.
Story
The Assign Work Orders (RS501000) processing form was developed for the Smart Fix company, and now its
managers would like to make the automatic assignment of work orders on this form more eﬀicient:


Part 2: Update of Data with a Custom Accumulator Attribute | 51
•
Repair work orders should be automatically assigned to the employee with the fewest repair work orders
assigned.
•
If multiple employees have the smallest number of repair work orders assigned, the work orders will be
assigned to the first of these employees to be selected from the database.
For the Assign Work Orders form, you’ve already implemented the custom PXAccumulator attribute, which
counts the number of repair work orders assigned to each employee and updates this number in the database
during processing. Now you need to modify the form so that it contains information about the number of work
orders assigned to the potential assignees who are listed in the table.
In the table on the Assign Work Orders form, you need to have the following columns related to the employees and
the number of assigned work orders:
•
Assignee: The assignee selected on the Repair Work Orders (RS301000) form for the work order. The column
is empty if no value has been selected on the Repair Work Orders form. You’ll add this column temporarily
for testing purposes; it won’t be editable.
•
Default Assignee: The default assignee, which is calculated as the employee with the lowest number of
assigned work orders. To make sure that the values in the Assign To column are calculated correctly during
testing, you’ll display the Default Assignee column, and it won’t be editable. (You'll delete this column aer
testing.)
•
Assign To: The employee to which the repair work order will be assigned during processing. By default,
the system inserts the value from the Assignee column for this work order if it isn’t null. If it’s null, the
system inserts the value copied from the Default Assignee column. A user can override the default value in
this column.
•
Number of Assigned Work Orders: An uneditable column showing the number of work orders assigned to
the assignee that’s specified in the Assignee column.
Process Overview
You'll modify the constructor of the RSSVAssignProcess graph to make the Assign To column editable.
In the RSSVWorkOrderEntry graph, you'll also modify the implementation of the AssignOrders() method
and add the complete() action handler so that 1 is added to or subtracted from the number of assigned work
orders. The value specified for the number of assigned work orders in the AssignOrders() method and the
complete() action handler will be added to the value stored in the database by the custom PXAccumulator
attribute.
You’ll then modify the TypeScript code of the Assign Work Orders (RS501000) form and test the changes on the
form.
Step 1: Enabling the Editing of the Field
Edit the RSSVAssignProcess graph as follows:
1. In the constructor of the RSSVAssignProcess graph, replace the Assignee field with the AssignTo
field of the RSSVWorkOrder DAC. The resulting code of the constructor is shown in the following code.
        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate(list =>
                AssignOrders(list, true));
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignTo>(
                WorkOrders.Cache, null, true);
        }


Part 2: Update of Data with a Custom Accumulator Attribute | 52
2. Build the project.
Step 2: Modifying the Assignment and Completion Operations
In this step, you'll modify the AssignOrders() static method of the RSSVAssignProcess graph and add
the assign() and complete() action handlers of the RSSVWorkOrderEntry graph so that they change
the number of assigned work orders for each employee who’s assigned a repair work order or who completed a
repair work order. You’ll assign 1 or -1 (depending on whether the work order is assigned or completed) to the
RSSVWorkOrder.NbrOfAssignedOrders field; the custom accumulator attribute will add this value to the
value stored in the database.
Do the following to modify the AssignOrders() method and add the assign() and complete() action
handlers:
1. In the RSSVWorkOrderEntry graph, define the data view for the calculation of the number of assigned
work orders per employee, as shown in the following code.
        //The view for the calculation of the number of assigned work orders 
        //per employee
        public SelectFrom<RSSVEmployeeWorkOrderQty>.View Quantity = null!;
2. In the RSSVWorkOrderEntry graph, replace the definition of the Assign action with the following code.
        public PXAction<RSSVWorkOrder> Assign = null!;
        [PXButton]
        [PXUIField(DisplayName = "Assign", Enabled = false)]
        protected virtual IEnumerable assign(PXAdapter adapter)
        {
            // Get the current order from the cache
            RSSVWorkOrder row = WorkOrders.Current;
            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.UserID = row.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = 1;
            Quantity.Insert(employeeNbrOfOrders);
            // Trigger the Save action to save changes in the database
            Actions.PressSave();
            return adapter.Get();
        }
3. In the RSSVWorkOrderEntry graph, replace the definition of the Complete action with the following
code.
        public PXAction<RSSVWorkOrder> Complete = null!;
        [PXButton]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual IEnumerable complete(PXAdapter adapter)
        {
            // Get the current order from the cache
            RSSVWorkOrder row = WorkOrders.Current;
            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.UserID = row.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = -1;
            Quantity.Insert(employeeNbrOfOrders);
            // Trigger the Save action to save changes in the database


Part 2: Update of Data with a Custom Accumulator Attribute | 53
            Actions.PressSave();
            return adapter.Get();
        }
4. In the AssignOrders() method of the RSSVAssignProcess graph, add the following line in the
beginning of the lambda expression in the ProcessRecords method.
                    workOrder.Assignee = workOrder.AssignTo;
5. Rebuild the project.
Step 3: Adjusting the TypeScript Code (Self-Guided Exercise)
Perform the following general steps on your own:
1. In the RS501000.ts file, add the Default Assignee, Assign To, and Number of Assigned Work Orders
columns to the table on the Assign Work Orders (RS501000) form. Hide the selector links for the Default
Assignee and Assign To columns.
2. Remove PXFieldOptions.CommitChanges for the Assignee column.
3. For the Assign To column, set the PXFieldOptions.CommitChanges option for the property type.
4. Publish the customization project.
Step 4: Testing the Form and the Attribute
In this step, you’ll test the Assign Work Orders (RS501000) form and the custom accumulator attribute. Do the
following:
1. On the Repair Work Orders (RS301000) form, create three repair work orders with the settings specified in
the following table. Save each of them and click Remove Hold on the form toolbar.
 
Work Order 1
Work Order 2
Work Order 3
Customer ID
C000000001
C000000002
C000000001
Service
Battery Replacement
Screen Repair
Battery Replacement
Device
Nokia 3310
Samsung Galaxy S4
Motorola RAZR V3
Assignee
Andrews, Michael
Empty
Beauvoir, Layla
Description
Test order
Test order
Test order
Notice that the created work orders have the Ready for Assignment status.
2. On the Assign Work Orders (RS501000) form, make sure that you see the three repair work orders that you
have created. Also, be sure that the Assignee, Default Assignee, and Assign To columns are filled in for
these orders.
For the first work order, the Assign To setting is Andrews, Michael, which is also specified in the Assignee
column. (You specified this employee on the Repair Work Orders form.)
For the second work order with no value in the Assignee column, the Assign To setting is Baker, Maxwell—
which is also specified in the Default Assignee column. The database currently doesn’t have information
about the number of repair work orders assigned to the employee. Therefore, it has selected the employee
with the first UserID (the key field) in the database.


Part 2: Update of Data with a Custom Accumulator Attribute | 54
For the third work order, the Assign To setting is Beauvoir, Layla—which is also specified in the Assignee
column.
Figure: The assignees on the Assign Work Orders form
3. For the third work order, change the employee in the Assign To column to Becher, Joseph.
4. On the form toolbar, click Assign All. The work orders should be processed successfully.
5. In the Processing dialog box, make sure that the processed repair work orders have the assignees specified
as follows:
•
First work order: Andrews, Michael
•
Second work order: Baker, Maxwell
•
Third work order: Becher, Joseph
6. Review the records in the RSSVEmployeeWorkOrderQty table by using Microso SQL Server
Management Studio. The table contains three records (one for each employee to which repair work orders
have been assigned during this testing). The value in the NbrOfAssignedOrders column is 1 for each
row.
7. On the Repair Work Orders (RS301000) form, open one of the processed work orders. Click Complete on the
form toolbar.
8. In SQL Server Management Studio, review the records in the RSSVEmployeeWorkOrderQty table. Now
for the row representing the completed order, the value of NbrOfAssignedOrders is 0.
Step 5: Removing Unnecessary Columns from the Form (Self-Guided Exercise)
You should now remove the Assignee and Default Assignee columns from the table on the Assign Work Orders
(RS501000) form on your own.
You can remove the columns by editing the TypeScript code of the form on the Modern UI Files page of the
Customization Project Editor or by editing the code in Visual Studio Code or another code editor.


Part 3: Redirection to a Report at the End of Processing | 55
Part 3: Redirection to a Report at the End of Processing
In this part of the course, you'll modify the processing operation of the Assign Work Orders form so that it displays a
report at the end of the operation.
Lesson 3.1: Adding Redirection to a Report at the End of Processing
In this lesson, you'll modify the Assign Work Orders (RS501000) form so that it displays a report at the end of
processing. The report will list the repair work orders assigned during the assignment operation and the assignees
they were assigned to.
Activity 3.1.1: To Add Redirection to a Report at the End of Processing
The following activity will walk you through the process of implementing redirection to a report at the end of
processing.
Story
For better usability of the custom Assign Work Orders (RS501000) processing form, the managers of the Smart Fix
company have requested that the form be modified so that at the end of the processing, the system displays a
report that lists all processed work orders and shows the employees to which they have been assigned during the
processing. The report has already been developed.
Process Overview
You'll add the RS601000.rpx report file to the PhoneRepairShop customization project. You'll then modify the
processing operation of the Assign Work Orders (RS501000) form so that it displays the report at the end of the
operation. you'll also test the updated functionality of the form.
The creation of reports with Acumatica Report Designer is outside of the scope of this activity. To
learn more about the creation of reports, see Acumatica Report Designer Guide.
Step 1: Including RS601000.rpx in the Customization Project
In this step, you'll add the RS601000.rpx report file to the customization project. You must include the report file
in the customization project so that the report is available in each Acumatica ERP instance to which you publish the
PhoneRepairShop customization project.
To include the report file in the customization project, do the following:
1. Copy the RS601000.rpx file to the ReportsCustomized folder of your Acumatica ERP instance for the
training course. The system uses this folder to search for custom and customized Acumatica ERP reports.
2. In the Customization Project Editor, open the PhoneRepairShop customization project.
3. On the Files page, add the ReportsCustomized\RS601000.rpx file, and save your changes.
For details on adding files to the customization project, see To Add a Custom File to a Project in
the documentation.
4. Publish the customization project.


Part 3: Redirection to a Report at the End of Processing | 56
5. On the Site Map (SM200520) form of Acumatica ERP, add a new row with the following settings, and save
your changes:
•
Screen ID: RS.60.10.00
•
Title: Assigned Work Orders
•
UI: Classic
•
URL: ~/frames/reportlauncher.aspx?id=RS601000.rpx
•
Graph Type: Empty
•
Workspaces: Empty
The report is not supposed to be used directly from the UI of Acumatica ERP; therefore, you do not
include it in any workspace.
•
Category: Empty
•
Is Substitute: Empty
6. On the Access Rights by Screen (SM201020) form, provide Granted access rights for the Assigned Work
Orders (RS601000) report form for the Administrator and Customizer user roles. You can find the Assigned
Work Orders item that corresponds to the report by expanding the Hidden node in the le pane.
7. In the Customization Project Editor (with it opened for the PhoneRepairShop customization project), on the
Site Map page, add the site map item for the Assigned Work Orders report.
8. On the Access Rights page, include access rights for the new report in the customization project.
9. Publish the customization project.
Step 2: Testing the Report and Switching its UI
In Acumatica ERP, make sure that the report is displayed correctly by doing the following:
1. Open the Assigned Work Orders (RS601000) report form.
Because the report has no workspace specified, you cannot find it in the UI by typing its name
in the Search box or browsing the main menu. The only way to open the report is to type the
ID of the report as the value of the ScreenId parameter of the URL in the address line of the
browser, as shown in the following screenshot.
Figure: Report ID in the URL
2. On the report form toolbar, click Run Report. The report is displayed, as shown in the following screenshot.
Because no filtering is specified in the report settings, the report displays all the repair work orders that
exist in the application database. When the system redirects the user to this report from the Assign Work
Orders (RS501000) form, it will filter the list to show only the orders that have been assigned during the
processing.


Part 3: Redirection to a Report at the End of Processing | 57
Figure: Assigned Work Orders report
3. Switch the report to the Modern UI by clicking Tools > Switch to Modern UI on the report form's title bar.
Step 3: Implementing Redirection to a Report
In this step, you'll implement redirection to the Assigned Work Orders (RS601000) report at the end of the
AssignOrders() method. The report will display the repair work orders that have been assigned during the
processing operation the user invoked on the form. To implement the redirection, do the following:
1. In the Messages.cs file, add the following constant, which specifies the name of the webpage that will
display the report.
        public const string ReportRS601000Title = "Assigned Work Orders";
2. In the RSSVAssignProcess.cs file, modify the AssignOrders() method, as follows:
a. In the beginning of the method, add the following lines.
            PXReportResultset assignedOrders =
                new PXReportResultset(typeof(RSSVWorkOrder));
b. In the end of the lambda expression in the ProcessRecords method, add the following code.
                    // Add to the result set the order
                    // that has been successfully assigned.
                    if (workOrder.Status == WorkOrderStatusConstants.Assigned)
                    {
                        assignedOrders.Add(workOrder);
                    }
c. In the end of the AssignOrders() method, add the following code.
            if (assignedOrders.GetRowCount() > 0 && isMassProcess)
            {
                throw new PXReportRequiredException(assignedOrders, "RS601000",
                                                    Messages.ReportRS601000Title);
            }


Part 3: Redirection to a Report at the End of Processing | 58
To redirect the user to the report, you have thrown the PXReportRequiredException exception.
Once an exception is thrown, it interrupts the current context and propagates up the call stack until it is
handled by Acumatica Framework, which performs the redirection. You do not need to implement the
handling of the exceptions that are used for redirection.
The Assigned Work Orders report has no filtering parameters. You have passed the data to be displayed
in the report (that is, the repair work orders that have been assigned) in the parameters of the
PXReportRequiredException constructor.
3. Build the project.
4. Update the customization project to include the latest version of the extension library in the customization
project and publish it.
Step 4: Testing the Redirection to the Report
In this step, you'll test the redirection to the Assigned Work Orders (RS601000) report, which should occur at the
end of the assignment operation that is invoked on the Assign Work Orders (RS501000) form. To test the redirection
to the report, do the following:
1. On the Repair Work Orders (RS301000) form, create three repair work orders with the settings specified in
the following table. Once you have entered the settings of each order, save the order and then click Remove
Hold.
 
First Work Order
Second Work Order
Third Work Order
Customer ID
C000000001
C000000002
C000000001
Service
Battery Replacement
Screen Repair
Battery Replacement
Device
Nokia 3310
Samsung Galaxy S4
Motorola RAZR V3
Assignee
Andrews, Michael
Empty
Beauvoir, Layla
Description
Test order
Test order
Test order
Notice that each of the created work orders has the Ready for Assignment status.
2. On the Assign Work Orders (RS501000) form, make sure that three repair work orders are listed.
3. On the form toolbar, click Assign All. At the end of the processing, the Assigned Work Orders (RS601000)
report is displayed. Make sure that it shows the three work orders that you entered and that all three have
been assigned, as shown in the following screenshot.


Part 3: Redirection to a Report at the End of Processing | 59
Figure: Assigned Work Orders report
Related Links
•
Redirection to Webpages: General Information


Additional Materials | 60
Additional Materials
In this chapter, you can find additional information that is related to the training course.
Appendix A: Initial Configuration
If for some reason you cannot complete the instructions in Step 2: Deploying the Instance, you can create an
Acumatica ERP instance and manually publish the needed customization project, as described in this topic.
Step 1: Deploying the Needed Acumatica ERP Instance for the Training Course
You deploy an Acumatica ERP instance and configure it as follows:
1. To deploy a new application instance, open the Acumatica ERP Configuration wizard, and do the following:
a. On the Database Configuration page, type the name of the database: SmartFix_T240.
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
You load the customization project with the results of the T270 Workflow API training course and publish this project
as follows:
1. On the Customization Projects (SM204505) form, create a project with the name PhoneRepairShop, and
open it.


Additional Materials | 61
2. In the menu of the Customization Project Editor, click Source Control > Open Project from Folder.
3. In the dialog box that opens, specify the path to the WorkflowDevelopment
\T270\PhoneRepairShop folder, which you have downloaded from Acumatica GitHub, and click OK.
4. Bind the customization project to the source code of the extension library as follows:
a. Copy the WorkflowDevelopment\T270\PhoneRepairShop_Code folder to the App_Data
\Projects folder of the website.
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
been performed in the previous training courses of the T series. This project also contains the customization plug-
in, which fills in the tables created in these training courses with the custom data entered in these training courses.
For details about the customization plug-ins, see To Add a Customization Plug-In to a Project. (The creation of
customization plug-ins is outside of the scope of this course.)
Appendix B: Processing Dialog Box
In Lesson 1.1: Creating a Simple Processing Form, you have defined the Assign Work Orders (RS501000) processing
form. This form displays the Processing dialog box during the assignment operation. Making changes to this dialog
box, such as adding a custom button to this dialog box, is outside of the scope of this course but may be useful to
some readers.
Adding a Button to the Processing Dialog Box
When a processing operation is started, all elements of the processing form become unavailable. If you need to
make a button from the processing form available during processing, you have to add this button to the processing
dialog box, as described in Processing Form: Processing Dialog Box.


Additional Materials | 62
Hiding the Processing Dialog Box
You can turn oﬀ the displaying of the processing dialog box and instead display the progress and the result of the
processing on the form toolbar. For details, see Processing Form: Processing Dialog Box.
Appendix C: Parallel Processing
Suppose that you need to implement processing of items on a custom processing form. On this form, users process
large lists of items and all of these items can be processed independently. To speed up processing of these items,
you can implement parallel processing with Acumatica Framework. If you turn on the parallel processing, the list of
records that should be processed is split into batches and is processed in multiple threads.
Acumatica Framework has its own threading subsystem. We do not recommend that you mix it with
the default one from .Net.
For details about parallel processing, see PXParallelProcessingOptions Class.
Appendix D: Asynchronous Execution
Each time a user action on a form triggers a request (a round trip), the system creates a new graph instance to
process that request. Aer the request is processed, the graph instance must be cleared from the memory of the
Acumatica ERP server. If your code performs a long-running operation—such as processing a document, processing
large volumes of data, or executing an action that needs to call an external API—you should run it asynchronously
in a separate thread.
For more details about how to start asynchronous execution and how to work with data during asynchronous
execution, see Asynchronous Operations: General Information and Asynchronous Operations: How They are Handled
in Acumatica ERP.
Appendix E: Use of Event Handlers
This topic lists the scenarios in which particular event handlers have been used in this course.
Event
Scenario
Examples in the Guide
CacheAttached
Replacing the attributes of a DAC field
Activity 2.2.1: To Replace Field Attributes in
CacheAttached
RowSelecting
Defining the logic for fetching calculated
field values
Activity 2.2.2: To Fetch Calculated Data from
a Non-Scalar Source (in RowSelecting)
RowSelected
Specifying the workflow action to be used
for the processing
Activity 1.2.1: To Implement a Processing
Operation by Using the Workflow
