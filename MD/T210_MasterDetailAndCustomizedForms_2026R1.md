Developer Course
Customization
T210 Customized Forms and
Master-Details Relationships
2026 R1
Revision: 4/16/2026


Contents | 2
Contents
Copyright...............................................................................................................................................4
How to Use This Course.......................................................................................................................... 5
Company Story and Customization Description........................................................................................ 7
Initial Configuration............................................................................................................................... 9
Step 1: Preparing the Environment.................................................................................................................. 9
Step 2: Preparing the Needed Acumatica ERP Instance for the Training Course........................................... 9
Step 3: Creating the Database Tables.............................................................................................................10
Part 1: Custom Fields (Stock Items Form)............................................................................................... 11
Lesson 1.1: Adding Custom Fields.................................................................................................................. 11
Step 1.1.1: Creating a Custom Column and Field with the Project Editor...........................................12
Step 1.1.2: Creating a Control for the Custom Field............................................................................. 16
Step 1.1.3: Viewing the Content of the Customization Project............................................................ 19
Step 1.1.4: Creating a Custom Column with the Project Editor and a Custom Field with Visual
Studio...................................................................................................................................................... 21
Step 1.1.5: Creating a Control for the Custom Field—Self-Guided Exercise........................................ 23
Step 1.1.6: Making the Custom Field Conditionally Available (with RowSelected).............................24
Step 1.1.7: Creating Repair Items.......................................................................................................... 27
Lesson Summary.................................................................................................................................... 28
Additional Information: DAC Extensions............................................................................................... 29
Lesson 1.2: Configuring the UI—Self-Guided Exercise...................................................................................29
Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form)......................................31
Lesson 2.1: Defining a Master-Detail Relationship.........................................................................................31
Step 2.1.1: Creating the Form—Self-Guided Exercise...........................................................................33
Step 2.1.2: Defining the Master-Detail Relationship Between Data (with PXParent and
PXDBDefault)...........................................................................................................................................40
Step 2.1.3: Numbering Detail Records (with PXLineNbr)......................................................................42
Step 2.1.4: Creating Controls on the Form............................................................................................43
Step 2.1.5: Testing the Tab.....................................................................................................................45
Lesson Summary.................................................................................................................................... 45
Additional Information: Relationships Between DACs.........................................................................46
Lesson 2.2: Defining the Business Logic.........................................................................................................46
Step 2.2.1: Restricting the Values of a Field (with PXRestrictor).......................................................... 47
Step 2.2.2: Updating Fields of the Same Record on Update of a Field (with FieldUpdated and
FieldDefaulting)...................................................................................................................................... 49
Step 2.2.3: Updating a Field of Another Record on Update of a Field (with RowUpdated).................51


Contents | 3
Step 2.2.4: Updating Fields on Update of Another Field—Self-Guided Exercise................................. 53
Lesson Summary.................................................................................................................................... 56
Additional Information: Application Localization.................................................................................57
Additional Information: Data Querying.................................................................................................58
Additional Information: PXCache Objects.............................................................................................58
Part 3: Custom Tab (Stock Items Form)...................................................................................................59
Lesson 3.1: Adding a New Tab........................................................................................................................ 59
Step 3.1.1: Creating a DAC—Self-Guided Exercise................................................................................ 60
Step 3.1.2: Creating a Data View............................................................................................................60
Step 3.1.3: Creating the Tab Item, Grid, and Columns......................................................................... 61
Step 3.1.4: Hiding the Tab from the Form (with RowSelected)............................................................64
Step 3.1.5: Using the New Tab...............................................................................................................65
Lesson Summary.................................................................................................................................... 65
Part 4: Calculations and Insertion of a Default Record (Services and Prices Form)...................................... 67
Lesson 4.1: Calculating Field Values...............................................................................................................67
Step 4.1.1: Adding the Labor Tab—Self-Guided Exercise..................................................................... 68
Step 4.1.2: Calculating Field Values (with PXFormula).........................................................................70
Lesson Summary.................................................................................................................................... 73
Lesson 4.2: Inserting a Default Detail Record................................................................................................ 73
Step 4.2.1: Adding the Warranty Tab—Self-Guided Exercise................................................................74
Step 4.2.2: Inserting a Default Detail Record (with RowInserted)........................................................76
Step 4.2.3: Adding UI Representation Logic (with RowSelected and RowDeleting)............................78
Lesson Summary.................................................................................................................................... 80
Additional Information..........................................................................................................................81
Appendix A: Initial Configuration....................................................................................................................81
Appendix B: Use of Event Handlers................................................................................................................82
Appendix C: Troubleshooting the Customization Project Editor.................................................................. 83
Appendix C: Troubleshooting the Customization Project Editor................................................................ 84


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
Last Updated: 04/16/2026


How to Use This Course | 5
How to Use This Course
The T210 Customized Forms and Master-Details Relationships course shows how to customize existing Acumatica
ERP forms and define the master-detail relationship. In this course, you will learn how to add custom controls
and tabs to an existing Acumatica ERP form. You will also learn how to implement the UI logic, calculations, and
insertion of a default detail record by using Acumatica Framework.
The course is intended for application developers who are starting to learn how to customize Acumatica ERP.
The course is based on a set of examples that demonstrate the general approach to customizing Acumatica ERP. It
is designed to give you ideas about how to develop your own embedded applications by using the customization
tools. As you go through the course, you will continue development of the customization for the cell phone
repair shop, which was started in the T200 Maintenance Forms training course. We recommend completing T200
Maintenance Forms before you complete this one; if you have not completed that course, a prerequisite step will
explain how to publish the customization project that contains the results of T200 Maintenance Forms.
Aer you complete all the lessons of the course, you will be familiar with the basic programming techniques for the
customization of existing Acumatica ERP forms and for the implementation of the business logic of an Acumatica
ERP form.
We recommend that you complete the examples in the presented order because some examples use
the results of previous ones.
What the Course Prerequisites Are
To complete this course, you should be familiar with the basic concepts of Acumatica Framework. We strongly
recommend that you complete the T200 Maintenance Forms training course before you begin this course.
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
•
Writing and debugging complex SQL queries (WHERE clauses, aggregates, and subqueries)
•
Understanding the database structure (primary keys, data types, and denormalization)
•
The following experience with IIS:


How to Use This Course | 6
•
Configuring and deploying ASP.NET websites
•
Configuring and securing IIS
What Is in a Part
Each part of the course is dedicated to implementation of a particular scenario of customization of an existing
Acumatica ERP form or to implementation of a particular business logic on example of a custom form.
Each part of the course consists of lessons you should complete.
What Is in a Lesson
Each lesson focuses on a particular development scenario that you can implement by using Acumatica ERP
customization tools and the Acumatica Framework. Each lesson consists of a brief description of the scenario and
an example of its implementation.
The lesson may also include Additional Information topics, which are outside of the scope of this course but may be
useful to some readers.
Each lesson ends with a Lesson Summary topic, which summarizes the development techniques used during the
implementation of the scenario.
Where the Source Code Is
You can find the source code of the customization described in this course and code snippets for the course in the
Customization\T210 folder of the Help-and-Training-Examples repository in Acumatica GitHub.
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
In this course, you will continue the development for the cell phone repair shop of the Smart Fix company that you
have started in the T200 Maintenance Forms training course.
If you have not completed this training course, you will load and publish the customization project
with the results of this course in Initial Configuration.
In the T200 Maintenance Forms training course, you’ve created two simple maintenance forms for the Smart Fix
company:
•
Repair Services (RS201000), which contains a list of the company's repair services
•
Serviced Devices (RS202000), which lists the devices that can be serviced
In this course, you will create another maintenance form, Services and Prices (RS203000), which gives users the
ability to define and maintain the price for each provided repair service. You will also customize the Stock Items
(IN202500) form of Acumatica ERP to mark particular stock items as repair items—that is, the items that are
supplied to the customer (such as replacement screens and batteries) as part of the repair services.
Services and Prices Form
The Services and Prices (RS203000) form, which you will create in this course, will look as shown in the following
screenshot.
Figure: Services and Prices form
The form will contain the following tabs:
•
Repair Items: Will show the list of repair items (stock items) necessary for the repair service and device
selected in the Summary area of the form.
•
Labor: Will represent the work (the list of non-stock items) that is performed for the repair service and
device selected in the Summary area of the form.
•
Warranty: Will include the list of contract templates for the repair service and device selected in the
Summary area of the form.
You will also create a substitute form of the inquiry type, which will serve as the entry point to the form.
This form displays the data entered on the Services and Prices form in a tabular format. The generic
inquiry will function as a substitute form because it will be brought up instead of the form when a user
clicks the form name in a workspace.


Company Story and Customization Description | 8
These forms will use the following custom tables, which you have added to the application database in Initial
Configuration:
•
RSSVRepairPrice: The data of this table is displayed in the Summary area of the form.
•
RSSVRepairItem: The data of this table is displayed on the Repair Items tab.
•
RSSVLabor: The data of this table is displayed on the Labor tab.
•
RSSVWarranty: The data of this table is displayed on the Warranty tab.
Customization of the Stock Items Form
The custom elements that you will add to the Stock Items (IN202500) form are shown in the following screenshot.
Figure: Stock Items form
The customization of the Stock Items form will include the following changes:
•
The Repair Item check box, which you will add to the Item Defaults section of the General tab, will be used
to define whether the selected stock item is a repair item.
•
The Repair Item Type box, which you will also add to the Item Defaults section of the General tab, will
hold the repair item type to which the repair item belongs, which is one of the following: Battery, Screen,
Screen Cover, Back Cover, or Motherboard.
•
You will add the Compatible Devices tab, which will be used to define and maintain a list of compatible
serviced devices for the selected repair item. This tab will appear on the form only if the Repair Item check
box is selected.
The customized Stock Items form will use the RSSVStockItemDevice custom table, which you have added
to the application database in Initial Configuration. The form also uses the custom UsrRepairItem and
UsrRepairItemType fields of the InventoryItem database table. You will add these custom fields in Part 1:
Custom Fields (Stock Items Form).


Initial Configuration | 9
Initial Configuration
You need to perform prerequisite actions before you start to complete the course.
If you have deployed an instance for the T200 Maintenance Forms course and have the customization project and
the source code for this course, you can perform only Step 3: Creating the Database Tables.
Step 1: Preparing the Environment
If you have completed the T200 Maintenance Forms training course and are using the same
environment for the current course, you can skip this step.
Prepare the environment for the training course as follows:
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
Step 2: Preparing the Needed Acumatica ERP Instance for the Training Course
If you have completed the T200 Maintenance Forms training course, instead of deploying a new
instance, you can use the Acumatica ERP instance that you have deployed for the T200 Maintenance
Forms training course.
You deploy an Acumatica ERP instance and configure it as follows:
1. Open the Acumatica ERP Configuration wizard, and do the following:
a. Click Deploy a New Acumatica ERP Instance for T-Series Developer Courses.
b. On the Instance Configuration page, do the following:
a. In the Training Course box, select T210 Customized Forms and Master-Details Relationships.
b. In the Local Path to the Instance box, select a folder that’s outside of the C:\Program Files
(x86), C:\Program Files, and C:\Users folders. (We recommend that you store the website
folder outside of these folders to avoid an issue with permission to work in these folders when you
customize the website.)


Initial Configuration | 10
c. On the Database Configuration page, make sure the name of the database is SmartFix_T210.
The system creates a new Acumatica ERP instance, adds a new tenant, loads the data to it, and publishes
the customization project that is needed for activities of this training course.
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
instance and manually publish the needed customization project as described in Appendix A: Initial
Configuration.
Step 3: Creating the Database Tables
Create the database tables that are necessary for the T210 Customized Forms and Master-Details Relationships
training course and include the scripts in the customization project as follows:
1. In SQL Server Management Studio, execute the T210_DatabaseTables.sql script to create the
database tables that are necessary for the T210 Customized Forms and Master-Details Relationships training
course. The script creates the following tables: RSSVRepairPrice, RSSVRepairItem, RSSVLabor,
RSSVWarranty, and RSSVStockItemDevice.
•
The script is provided in the Customization\T210\SourceFiles\DBScripts
folder, which you have downloaded from Acumatica GitHub.
•
If you use the Acumatica ERP instance that you have deployed for the T200 Maintenance
Forms training course, you need to change the name of the database in the script from
SmartFix_T210 to SmartFix_T200 before execution of the script.
2. On the Database Scripts page of the Customization Project Editor that is open for the PhoneRepairShop
customization project, for each added table, do the following:
a. On the page toolbar, click Add Custom Table Schema.
b. In the dialog box that opens, select the table and click OK.
3. Publish the project.
The design of database tables is outside the scope of this course. For details on designing database
tables for Acumatica ERP, see Designing the Database Structure and DACs.


Part 1: Custom Fields (Stock Items Form) | 11
Part 1: Custom Fields (Stock Items Form)
A repair item is an item, such as a battery, that is supplied to the customer as part of the repair service. In
Acumatica ERP, such items will be defined on the Stock Items (IN202500) form.
This form, however, was not designed to track whether the item is a repair item or what type of repair item it is.
Therefore, to make it possible for a user to define a stock item as a repair item, the Smart Fix company needs to
customize the Stock Items form. In this part of the course, you will add custom fields to this form.
Aer you complete the lessons of this part, you will be able to test the ability to create a repair item on the Stock
Items form.
Lesson 1.1: Adding Custom Fields
A manager of the Smart Fix company needs to specify that particular stock items on the Stock Items (IN202500)
form of Acumatica ERP are repair items and select the type of each repair item.
To implement this scenario, you need to change the UI of the Stock Items (IN202500) form and the database table
that stores data for this form. In this lesson, you will use two approaches to perform these changes:
•
Using the Customization Project Editor to create the custom database column, the data access class (DAC)
field, and the UI control
•
Using Visual Studio to add the DAC field and the Customization Project Editor to create the custom database
column and the UI control
UI Changes
In this lesson, you will add the following custom controls to the Stock Items (IN202500) form of Acumatica ERP:
•
Repair Item: A check box that indicates (if selected) that the stock item can be used during the provision of
the repair services of the Smart Fix company
•
Repair Item Type: A drop-down list box for the repair item type, which is one of the following:
•
Battery
•
Screen
•
Screen Cover
•
Back Cover
•
Motherboard
You will add these controls to the Item Defaults fieldset on the General tab of the form (see the following
screenshot).


Part 1: Custom Fields (Stock Items Form) | 12
Figure: Custom elements to be added to the Stock Items form
Database Changes
The General tab displays the stock item's general information, which is stored in the data record of the
IN.InventoryItem data access class. Hence, you will add the custom fields to this class. To be able to save the
repair item data to the database, you will add the database columns for the new values. The IN.InventoryItem
class records are stored in the InventoryItem database table; therefore, you will add columns for the new fields
to this table.
Lesson Objectives
As you complete this lesson, you will learn how to do the following:
•
Add a custom column to an Acumatica ERP database table
•
Add a custom field to an Acumatica ERP data access class
•
Add the control for the custom field to the form
Step 1.1.1: Creating a Custom Column and Field with the Project Editor
In this step, you will create a custom column in the InventoryItem database table and a custom field in the
IN.InventoryItem data access class for this column. This column and field will be used to store and edit the
value of the Repair Item check box. You will use the Customization Project Editor to add the column and field.


Part 1: Custom Fields (Stock Items Form) | 13
The approach described in this step is the easiest way to create both the column in the database and
the bound field in the corresponding data access class.
We recommend that you not write custom SQL scripts to add changes to the database schema. If
you add a custom SQL script, you must adhere to platform requirements that apply to custom SQL
scripts, such as the support of multitenancy and the support of SQL dialects of the target database
management systems. If you use the approach described in this topic, during the publication of the
customization project, the platform generates SQL statements to alter the existing table so that this
statement conforms to all platform requirements.
You will create an extension of the IN.InventoryItem DAC to hold custom fields (which is referred to as a DAC
extension or cache extension). The Acumatica Customization Platform creates an extension for every customized
DAC to hold custom fields and customized attributes. At runtime, during the first initialization of a base class, the
platform automatically finds the extension for the class and applies the customization by replacing the base class
with the merged result of the base class and the extension it found. For more information about DAC extensions,
see Changes in the Application Code (C#).
You will also move the generated code to the extension library and adjust it with Acuminator.
Creating the Custom Column and Field
To create the custom column and the custom field, perform the following steps:
1. Open the Stock Items (IN202500) form, and then open the Element Inspector for it as follows:
a. On the form title bar, click Settings (the Gear icon) > Inspect Element, as shown in the following
screenshot.
Figure: Settings menu
b. On the General tab, click the Item Defaults fieldset to open the Element Properties dialog box for the
fieldset, as shown in the following screenshot. In the dialog box, notice the following:
•
The qp-fieldset control is the type of UI container whose area you have clicked for inspection.
•
The InventoryItem data access class provides the data fields for the controls on the inspected
fieldset.
By clicking the link with the name of the DAC you can view details about this DAC in the
DAC Schema Browser.


Part 1: Custom Fields (Stock Items Form) | 14
•
The ItemSettings data view provides data for the container.
•
The InventoryItemMaint graph provides business logic for this form.
Figure: Element Properties dialog box
2. Open the Customization Projects (SM204505) form and click the PhoneRepairShop customization project.
3. To add a custom field for the Repair Item check box in the customization project, do the following:
a. In the navigation pane of the Customization Project Editor, click Data Access.
b. On the Data Access page, click Add New Record.
c. In the Select Existing Data Access Class dialog box, select the name of the DAC:
PX.Objects.IN.InventoryItem, and click Select to close the dialog box.
The Data Class: IN.InventoryItem->PX.Data.PXBqlTable page opens.
d. On the page toolbar, click Create New Field.
e. In the Create New Field dialog box, which opens, specify the following settings for the new field:
•
Field Name: RepairItem
As soon as you move the focus out of the Field Name box, the system adds the Usr
prefix to the field name, which provides a distinction between the original fields and the
new custom fields that you add to the class. Keep the prefix in the field name.
•
Display Name: Repair Item
•
Storage Type: DBTableColumn
•
Data Type: bool
f.
Click OK to close the dialog box.
g. Click Save on the page toolbar.
The system creates the specified extension to both the data access class and the database table. The
InventoryItem customization item is added to the Data Access page, and the database script is added
to the Database Scripts page of the Customization Project Editor. The platform saves the changes to
the customization project that is opened in the Project Editor. However, the changes have not yet been
applied to the application because the project has not been republished.
4. Move the data access class extension to the PhoneRepairShop_Code extension library:


Part 1: Custom Fields (Stock Items Form) | 15
a. In the navigation pane, click Data Access.
The Data Access page opens.
b. On the Data Access page, click the line with InventoryItem.
c. On the page toolbar, click Convert to Extension.
The InventoryItemExtensions Code item appears in the Code Editor page.
d. On the toolbar of the Code Editor, click Move to Ext. Library.
For details how to move a DAC to an extension library, see To Move a DAC Item to an Extension
Library.
5. In Visual Studio, adjust the DAC extension as follows:
a. Move the InventoryItemExtensions.cs file to the DAC folder and open the file.
Notice that Acuminator displays the PX1016 error and the PX1011 warning for the
InventoryItemExt class.
The PX1016 error indicates that the class does not implement the IsActive method, which
conditionally makes the extension active or inactive. For details, see To Enable a DAC Extension
Conditionally (IsActive). In this course, for simplicity, the extension will be always active and you will
suppress the error.
The PX1011 warning shows that the InventoryItemExt class can be marked with the sealed
modifier because multiple levels of inheritance are not supported for PXCacheExtension. You will
use the fix provided by Acuminator.
b. To suppress the PX1016 error, place the cursor on the InventoryItemExt class name and in the
Quick Actions menu select Suppress the PX1016 diagnostic with Acuminator > in a comment, as
shown in the screenshot below. Acuminator adds the suppression comment.
Figure: Suppression of the error in a comment
c. Place the cursor on the InventoryItemExt class name and in the Quick Actions menu select Mark
the type as sealed, as the following screenshot shows. Acuminator adds the sealed modifier.
Figure: Fix of the warning
d. Remove virtual from the UsrRepairItem property field.
e. Make sure the UsrRepairItem field has the attributes shown in the following code.
        [PXDBBool]


Part 1: Custom Fields (Stock Items Form) | 16
        [PXUIField(DisplayName="Repair Item")]
f.
Add the PXDefault attribute as shown in the following code. The check box that will correspond to the
field will be cleared by default and the value of the field will not be required.
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
g. Change the namespace in which the InventoryItemExt class is declared to PhoneRepairShop.
h. Build the project.
Related Links
•
To Add a Custom Data Field
•
To Move a DAC Item to an Extension Library
•
To Publish the Current Project
•
Changes in the Application Code (C#)
•
To Enable a DAC Extension Conditionally (IsActive)
Step 1.1.2: Creating a Control for the Custom Field
Now you will define a control for the custom field that you added to the PhoneRepairShop customization project in
the previous step.
For you to create a control for a field on a form in an application instance, both of the following conditions must be
met:
•
The field exists in the instance.
•
The field is available through a data view that refers to the data access class containing the field declaration.
Extensions in TypeScript and HTML
To add a control for a custom field, you need to create an extension of an existing Acumatica ERP form in TypeScript
and HTML.
To define an extension of an existing Acumatica ERP form in TypeScript, you use TypeScript mixins. For details
about mixins, see https://www.typescriptlang.org/docs/handbook/mixins.html#alternative-pattern.
In the TypeScript file of an extension, you define an interface that extends the screen class and a class with the
same name the interface has. In the extension class, you do any of the following:
•
Initialize new data views in the same way as you do in the screen class. For details about the screen class
and view classes, see Screen Class in TypeScript and View Classes in TypeScript.
•
Optional: In the parameter of the featureInstalled decorator, specify the feature for which the
extension should be available.
•
Optional: Define new actions in the same way as you do in the screen class. For details about action
definitions, see Action Definitions in TypeScript and Button: Configuration.
•
Optional: Adjust the TypeScript code of the original form.
An example is shown in the following code.
import { featureInstalled, FeaturesSet } from "client-controls";
import { GL401000 } from "src/screens/GL/GL401000/GL401000";
export interface GL401000_MultiCurrency extends GL401000 { }
@featureInstalled(FeaturesSet.Multicurrency)
export class GL401000_MultiCurrency {


Part 1: Custom Fields (Stock Items Form) | 17
}
In the HTML file of an extension, you can modify the layout of the screen, if necessary. The following code shows an
example of a modification.
<template>
    <field after="#columnSecond [name='SubCD']" name="ShowCuryDetail"></field>
</template>
In this example, the after attribute of the field tag shows that the ShowCuryDetail field should be placed
aer the tag with the SubCD name in the container with the columnSecond ID.
All tags that customize the original HTML code of an Acumatica ERP form must be located on the
highest level of the extension layout—that is, in the template tag of the highest level.
For more examples of layout adjustments, see UI Adjustments in HTML and TypeScript: HTML Examples.
In this lesson, you will be using the Modern UI Editor to generate the code of the extensions.
Step 1: Creating the TypeScript Extension
To create a TypeScript extension for the form, do the following:
1. Open the Modern UI Editor for the Stock Items (IN202500) form:
a. In the navigation pane of the Customization Project Editor, click Screens.
b. On the Screens page toolbar, click Customize Existing Screen.
c. In the Customize Existing Screen dialog box, specify the IN202500 form and click OK.
The line for the Stock Items (IN202500) appears on the Screens page.
d. In the navigation pane of the Customization Project Editor, open Screens >  IN202500 > Modern UI
Editor.
The IN202500 (Stock Items) page opens.
2. On the page toolbar, click Add Field.
3. In the Add Field dialog box, specify the following values:
•
Data View: ItemSettings (Inventory Item Settings)
•
DAC: PX.Objects.IN.InventoryItem
•
Field or Display Name: UsrRepairItem
4. In the table of the dialog box, select the UsrRepairItem field.
5. Click Save to Extension.
6. On the page toolbar, click Save.
The TypeScript extension for the IN202500 form is generated and added to the Modern UI Files page.
Step 2: Specifying the Location of the Control in HTML
To specify the location of the field and generate the HTML extension, do the following:
1. Open the Modern UI Editor for the Stock Items (IN202500) form.
2. In the element tree (Item 1 in the screenshot below), locate the ItemsSettings view and the
UsrRepairItem field.
You can do it by clicking the Customized filter at the top of the tree (Item 2).


Part 1: Custom Fields (Stock Items Form) | 18
Figure: The Modern UI Editor for the Stock Items form
3. On the HTML tab, locate the definition of the Item Settings section: It is defined in the qp-fieldset tag
bound to the ItemSettings view.
4. The Repair Item box should be located aer the Type box, so to specify the location of the custom box, put
the cursor aer the ItemType field (Item 4).
5. In the element tree, click the arrow button (Item 3) next to the UsrRepairItem node.
The new line with the field tag for the Repair Item box is generated aer the field tag for the Type box.
6. On the page toolbar, click Save.
The HTML extension for the IN202500 form is generated and added to the Modern UI Files page.
You can also review the generated HTML extension by clicking Preview HTML Extension on
the tab toolbar.
7. Publish the customization project.
Step 3: Apply and Check the Changes
To make sure, the box has been added correctly, refresh the Stock Items (IN202500) form in the browser. The Repair
Item check box should be located in the Item Defaults fieldset of the General tab, as shown in the following
screenshot.


Part 1: Custom Fields (Stock Items Form) | 19
Figure: The Repair Item check box
Related Links
•
To Add a Box for a Data Field
•
Changes in Webpages (ASPX)
Step 1.1.3: Viewing the Content of the Customization Project
In this lesson, you have defined the following items in the PhoneRepairShop customization project:
•
A custom column in the InventoryItem table of the database.
•
A custom field in the IN.InventoryItem data access class. You then have moved this definition to the
extension library.
•
A custom control on the Stock Items (IN202500) form.
In this step, you will view the XML content of these items.
Viewing the Content of the Customization Project
To view the content of the customization project for items you have created, click File > Edit Project Items on the
menu of the Customization Project Editor.
The Project Editor opens the Edit Project Items page, which displays the list of items of the customization project
and the work area to review and edit the XML code of the item selected in the list (see the following screenshot).


Part 1: Custom Fields (Stock Items Form) | 20
Figure: The list of the project items
Notice the following items in the PhoneRepairShop customization project, which are highlighted in the screenshot
above.
Item
Description
IN202500
This EditorUpdatedFields file contains information about TypeScript
changes that were made in the Modern UI Editor for the Stock Items
(IN202500) form. Based on these changes, the system generates the Type-
Script extensions from scratch overwriting the TypeScript extension file in
the project.
Bin\PhoneRe-
pairShop_Code.dll
This File item contains a relative path to the DLL file of the extension li-
brary.
screens\IN
\IN202500\exten-
sions\IN202500_PhoneRe-
pairShop_generated.html
This PerTenantFile item contains the location of the HTML extension of
the Stock Items (IN202500) form. The file according to this location will be
built and applied during publication.
screens\IN
\IN202500\exten-
sions\IN202500_PhoneRe-
pairShop_generated.ts
This PerTenantFile item contains the location of the TypeScipt extension
of the Stock Items (IN202500) form. The file according to this location will
be built and applied during publication.


Part 1: Custom Fields (Stock Items Form) | 21
Item
Description
InventoryItem
This Table item contains information about the custom column added to
the InventoryItem table for the bound custom field created in the ex-
tension of the InventoryItem DAC.
Related Links
•
Customization Project
Step 1.1.4: Creating a Custom Column with the Project Editor and a Custom Field
with Visual Studio
In this step, you will create a custom column in the InventoryItem database table and a custom field in the
IN.InventoryItem data access class for this column. This column will hold the value of the Repair Item Type
box of the Stock Items (IN202500) form, which corresponds to the field. You will use Visual Studio to add the DAC
field and the Customization Project Editor to add the database column.
We recommend that you not write custom SQL scripts to add changes to the database schema. If
you add a custom SQL script, you must adhere to platform requirements that apply to custom SQL
scripts, such as the support of multitenancy and the support of SQL dialects of the target database
management systems. If you use the approach described in this topic, during the publication of the
customization project, the platform generates SQL statements to alter the existing table so that this
statement conforms to all platform requirements.
You will define the UsrRepairItemType data field in the InventoryItemExt DAC extension. The fields in the
DAC extensions are defined in the same way as they are in DACs. For details about the definition of DACs, see Data
Access Classes.
You will define the Repair Item Type combo box as the input control for the UsrRepairItemType data field
by adding the PXStringList attribute to the field. The control will give the user the ability to select one of the
following repair item types: Battery, Screen, Screen Cover, Back Cover, or Motherboard.
Creating the Custom Column and Field
Do the following to create the custom column and field:
1. Add the database column as follows:
a. In the Customization Project Editor, open the PhoneRepairShop project.
b. On the le pane, click Database Scripts.
c. On the More menu of the Database Scripts page of the Customization Project Editor, click Add Custom
Column to Table.
The InventoryItem database script is already present on the page. So, alternatively, you can
click on the InventoryItem row, and in the Edit Table Columns dialog box which appears,
click Add > Add Custom Column To Table.
d. In the dialog box that opens, specify the following values:
•
Table: InventoryItem
•
Field Name: UsrRepairItemType
•
Data Type: string


Part 1: Custom Fields (Stock Items Form) | 22
•
Length: 2
e. Click OK to close the dialog box.
The Acumatica Customization Platform adds the column to the InventoryItem Table item in the
customization project.
f.
Publish the customization project.
2. In Visual Studio, in the Helper\Constants.cs file, define the RepairItemTypeConstants class (if
it has not been defined yet) as shown in the following code.
    //Constants for the repair item types
    public static class RepairItemTypeConstants
    {
        public const string Battery = "BT";
        public const string Screen = "SR";
        public const string ScreenCover = "SC";
        public const string BackCover = "BC";
        public const string Motherboard = "MB";
    }
3. In the Helper\Messages.cs file, add the constants for the repair item types (if they have not been
added yet) to the Messages class, as shown in the following code.
        //Repair item types
        public const string Battery = "Battery";
        public const string Screen = "Screen";
        public const string ScreenCover = "Screen Cover";
        public const string BackCover = "Back Cover";
        public const string Motherboard = "Motherboard";
4. In the InventoryItemExt class of the InventoryItemExtensions.cs file, add a custom field for
the Repair Item Type box, as the following code shows.
        #region UsrRepairItemType
        [PXDBString(2, IsFixed = true)]
        [PXStringList(
            new string[]
            {
                PhoneRepairShop.RepairItemTypeConstants.Battery,
                PhoneRepairShop.RepairItemTypeConstants.Screen,
                PhoneRepairShop.RepairItemTypeConstants.ScreenCover,
                PhoneRepairShop.RepairItemTypeConstants.BackCover,
                PhoneRepairShop.RepairItemTypeConstants.Motherboard
            },
            new string[]
            {
                PhoneRepairShop.Messages.Battery,
                PhoneRepairShop.Messages.Screen,
                PhoneRepairShop.Messages.ScreenCover,
                PhoneRepairShop.Messages.BackCover,
                PhoneRepairShop.Messages.Motherboard
            })]
        [PXUIField(DisplayName = "Repair Item Type")]
        public string? UsrRepairItemType { get; set; }
        public abstract class usrRepairItemType :
          PX.Data.BQL.BqlString.Field<usrRepairItemType>
        { }


Part 1: Custom Fields (Stock Items Form) | 23
        #endregion
In the first parameter of the PXStringList constructor, you specify the list of possible values for the field,
while in the second parameter, you specify the labels that correspond to the values and are displayed on
the form. Because the possible values for the field have a fixed two-character length, you have specified the
IsFixed = true and the length equal to 2 for the PXDBString attribute.
5. Build the project.
Related Links
•
To Add a Custom Column to an Existing Table
•
PXStringListAttribute Class
•
Data Access Classes
Step 1.1.5: Creating a Control for the Custom Field—Self-Guided Exercise
Now you will create a control on your own for the Repair Item Type custom field, which you added to the
PhoneRepairShop customization project in the previous step. The addition of a control for the field was described
earlier in this lesson.
Once you complete this step, the Stock Items (IN202500) form will look as shown in the following screenshot. The
InventoryItem database table contains the UsrRepairItemType column of the nvarchar(2) type.
Figure: The Repair Item Type box
Related Links
•
To Add a Box for a Data Field
•
Changes in Webpages (ASPX)


Part 1: Custom Fields (Stock Items Form) | 24
Step 1.1.6: Making the Custom Field Conditionally Available (with RowSelected)
In this step, you will learn how to work with a custom control that is available only conditionally. The Repair Item
Type box should be unavailable on the Stock Items (IN202500) form unless the Repair Item check box is selected.
Changes in the DAC
You will make the Repair Item Type box unavailable by default by setting the Enabled property of the
PXUIField attribute to false.
The user can edit a field value in the UI if the control for the field is available on the form. You can
make a control available or unavailable by specifying the Enabled parameter of the PXUIField
attribute in the data access class.
Changes in the Graph
You will add the RowSelected event handler to make the box become available when a user selects the Repair
Item check box. You will use the RowSelected event handler because it is intended for the implementation of UI
presentation logic. In the RowSelected event handler, you will do the following:
•
Access the UsrRepairItem extension field of the InventoryItem DAC by invoking the
GetExtension method on the cache. (For details on this method, see Access to a Custom Field in the
documentation.)
•
Use the PXUIFieldAttribute.SetEnabled<>() method to change the value of the Enabled
property of the PXUIField attribute of the UsrRepairItemType extension field.
You will use the Customization Project Editor to create the graph extension, and you will edit the business logic in
Visual Studio.
Changes in the TypeScript Code
To make the Repair Item Type box available if a user has selected the Repair Item check box and then the Repair
Item check box has lost input focus, you need to set the CommitChanges property of the UsrRepairItem field
to true in the view class declaration in TypeScript. You can do it in one of the following ways:
•
Use the PXFieldOptions.CommitChanges option for the property type, as shown below.
UsrRepairItem: PXFieldState<PXFieldOptions.CommitChanges>;
•
Set the CommitChanges property of the field to true by using the fieldInfo decorator, as shown below.
@fieldInfo({ commitChanges: true })
UsrRepairItem: PXFieldState;
If you do not set the CommitChanges property to true, then when a user selects the Repair Item check box, the
Repair Item Type box will become available only when the stock item record is saved or when the value of another
field with the CommitChanges property set to true is changed. For details about the CommitChanges option,
see Use of the CommitChanges Property of Boxes in the documentation.
Instructions for Adding the UI Presentation Logic
To add this presentation logic, perform the following steps:


Part 1: Custom Fields (Stock Items Form) | 25
1. In Visual Studio, in the InventoryItemExtensions.cs file, make the Repair Item Type
box unavailable by default by setting the Enabled property of the PXUIField attribute of the
UsrRepairItemType field to false, as the following code shows.
        [PXUIField(DisplayName = "Repair Item Type", Enabled = false)]
        public string? UsrRepairItemType { get; set; }
        public abstract class usrRepairItemType :
          PX.Data.BQL.BqlString.Field<usrRepairItemType>
        { }
2. Generate the graph extension as follows:
a. Learn the name of the graph for the Stock Items form by using the Element Inspector: The name is
InventoryItemMaint.
b. Open the Code page of the Customization Project Editor.
c. On the page toolbar, click Add New Record.
d. In the Create Code File dialog box, specify the following values:
•
File Template: Graph Extension
•
Base Graph: PX.Objects.IN.InventoryItemMaint
e. Click OK.
The Custom Code page opens with the InventoryItemMain_Extension class.
f.
On the page toolbar, click Move to Ext. Library.
3. In Visual Studio, adjust the graph extension as follows:
a. Change the namespace in which the InventoryItemMaint_Extension class is declared to
PhoneRepairShop.
b. In the InventoryItemMaint.cs file, use Acuminator to suppress the PX1016 error in a comment as
you have done for the DAC extension in the first step of this lesson.
c. Define the RowSelected event handler as follows.
        protected void _(Events.RowSelected<InventoryItem> e)
        {
            if (e.Row == null) return;
            InventoryItem item = e.Row;
            InventoryItemExt itemExt = PXCache<InventoryItem>.
                GetExtension<InventoryItemExt>(item);
            bool enableFields = itemExt != null &&
                itemExt.UsrRepairItem == true;
            //Make the Repair Item Type box available
            //when the Repair Item check box is selected.
            PXUIFieldAttribute.SetEnabled<InventoryItemExt.usrRepairItemType>(
                e.Cache, e.Row, enableFields);
        }
The code above makes the usrRepairItemType custom field available for editing if the value of
the UsrRepairItem field of the row in PXCache is true. Otherwise, it makes the custom field
unavailable.
d. Remove unnecessary using directives.


Part 1: Custom Fields (Stock Items Form) | 26
While Acumatica Customization Platform creates an extension for an original class of
Acumatica ERP, the platform inserts all the using directives from the original class to
the extension. Some using directives are unused in the customization code and can be
removed.
e. Build the project.
Instructions for Specifying CommitChanges in the Modern UI Editor
You can set the CommitChanges property to true in the fieldInfo decorator by using the Modern UI Editor. To
set the CommitChanges property to true for the UsrRepairItem field:
1. In the Modern UI Editor, select the UsrRepairItem field in the tree (Item 1 on the following screenshot).
2. On the View or Field Customization tab, click Add Decorators (Item 2).
Figure: The Add Decorators dialog box
3. In the Add Decorators dialog box, in the le table, select the fieldInfo decorator (Item 3).
4. In the Add Decorators dialog box, in the right table, select the commitChanges property (Item 4).
5. Click Add.
The fieldInfo decorator appears in the Custom Code box of the View or Field Customization tab.
6. On the page toolbar, click Save.
You can view the updated extension by clicking Edit TypeScript Extension and selecting the generated
extension.
7. Update the customization project with the changes you have made in this lesson, and publish the project.
Related Links
•
PXUIFieldAttribute Class
•
Use of the CommitChanges Property of Boxes


Part 1: Custom Fields (Stock Items Form) | 27
•
Access to a Custom Field
•
Configuration of the User Interface in Code
Step 1.1.7: Creating Repair Items
Now you will create the BAT3310, BAT3310EX, and BCOV3310 repair item records, which you will need later.
To do this, perform the following actions:
1. On the Stock Items (IN202500) form, click Add New Record on the form toolbar.
2. In the Summary area, specify the following settings:
•
Inventory ID: BAT3310
•
Description: Battery for Nokia 3310
3. Notice that the Repair Item Type box in the Item Defaults section of the General tab is unavailable
because the Repair Item check box is cleared.
4. In the Item Defaults section of the General tab, specify the following settings:
•
Item Class: Stock Item
•
Repair Item: Selected
Notice that once you selected the check box, the Repair Item Type box becomes available.
•
Repair Item Type: Battery
5. In the Warehouse Defaults section of the General tab, select MAIN in the Default Warehouse box.
6. In the Price Management section of the Price/Cost tab, enter 20 as the Default Price.
7. On the form toolbar, click Save to save the record in the database.
8. Repeat the previous instructions to create repair items with the following values.
UI Element (Location)
First Record
Second Record
Inventory ID (Summary area)
BAT3310EX
BCOV3310
Description (Summary area)
Extended Battery for
Nokia 3310
Back cover for Nokia
3310
Item Class (Item Defaults sec-
tion of the General tab)
Stock Item
Stock Item
Repair Item (Item Defaults sec-
tion of the General tab)
Selected
Selected
Repair Item Type (Item De-
faults section of the General
tab)
Battery
Back Cover
Default Warehouse (Warehouse
Defaults section of the General
tab)
MAIN
MAIN
Default Price (Price Manage-
ment section of the Price/Cost
tab)
30
10


Part 1: Custom Fields (Stock Items Form) | 28
To ensure that the InventoryItem table of the database contains the custom columns and the data entered, you
can review the table by using SQL Server Management Studio.
Lesson Summary
In this lesson, you have learned how to create a control so that you can display on a form a custom field bound
to the database. To implement this customization, you have learned how to add the necessary modifications to a
customization project and how to publish the project to apply the changes to the system.
As you have completed the lesson, you have added the following elements to the PhoneRepairShop customization
project:
•
Two column definitions in the InventoryItem table of the database.
•
Two custom field declarations in the extension of the IN.InventoryItem data access class (in the
PhoneRepairShop_Code extension library).
•
Two custom fields in a TypeScript extension and HTML extension for the Stock Items (IN202500) form.
•
One custom event handler, which you have added to the InventoryItemMaint graph. You have used
the RowSelected event handler to configure the UI presentation logic.
The following diagram shows the results of the lesson.


Part 1: Custom Fields (Stock Items Form) | 29
Additional Information: DAC Extensions
You can not only create custom fields but also customize existing Acumatica ERP fields and include your
customizations in DAC extensions of diﬀerent levels. These scenarios are outside of the scope of this course but
may be useful to some readers.
Customization of Field Attributes
In addition to adding custom fields in DAC extensions, you can customize existing fields by changing the attributes
assigned to the fields in Acumatica ERP DACs. For more information about the customization of field attributes, see
Customization of Field Attributes in DAC Extensions.
Diﬀerent Levels of DAC Extensions
The Acumatica Customization Platform supports multilevel extensions, which are required when you develop oﬀ-
the-shelf soware that is distributed in multiple editions. Precompiled extensions provide a measure of protection
for your source code and intellectual property.
You can use multilevel extensions to develop applications that extend the functionality of Acumatica ERP or other
soware based on Acumatica Framework in multiple markets (that is, specified categories of potential client
organizations). You may have a base extension that contains the solution common to all markets as well as multiple
market-specific extensions. Every market-specific solution is deployed along with the base extension. Moreover,
you can later customize deployed extensions for the end user by using DAC and graph extensions.
For additional details about multilevel extensions, see DAC Extensions and Graph Extensions.
Lesson 1.2: Configuring the UI—Self-Guided Exercise
Consultants and managers of the Smart Fix company would like that all Acumatica ERP forms that are necessary
for their work be available in one place, in the Phone Repair Shop workspace of Acumatica ERP. This workspace
was added as part of the T200 Maintenance Forms training course. (If you did not complete this course, the creation
of the workspace is part of the customization project that have been published in preparation to take the current
course.)
In this lesson, which you will complete on your own, you will add the Stock Items (IN202500) form to the Phone
Repair Shop workspace of Acumatica ERP. You will also include this change to the workspace in the customization
project.
Tips
As you add the Stock Items form to the Phone Repair Shop workspace of Acumatica ERP, use the following tips:
•
Use Menu Editing mode. For details about Menu Editing mode, see Menu Editing Mode.
•
Add a link to the Stock Items form to the Profiles category of the Phone Repair Shop workspace and make
the form be available in the quick menu of the workspace. For details about this process, see UI Navigation
Options: Workspaces and UI Navigation Options: To Configure a Workspace.
Result
As the result, the Phone Repair Shop workspace will look as shown in the following screenshot.


Part 1: Custom Fields (Stock Items Form) | 30
Figure: Stock Items form in the Phone Repair Shop workspace
The customization project will include the SiteMapNode item for the Stock Items form, as shown in the following
screenshot.
Figure: SiteMapNode item for the Stock Items form


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 31
Part 2: Master-Detail Relationship and Business Logic
(Services and Prices Form)
To simplify the creation of the work orders for repairs, the Smart Fix company needs to have a custom Acumatica
ERP form on which users can maintain the price of the selected repair service for the selected device. For this
purpose, you will create the Services and Prices (RS203000) custom maintenance form, which is described in
Company Story and Customization Description. In this part, you will create and configure the Summary area and the
Repair Items tab of the form, for which you will implement the master-detail relationship and basic business logic.
You will add other tabs in Part 4: Calculations and Insertion of a Default Record (Services and Prices Form).
Aer you complete the lessons of this part, you will be able to test the functionality of the Repair Items tab of the
Services and Prices form.
Lesson 2.1: Defining a Master-Detail Relationship
In this lesson, you will create the custom Services and Prices (RS203000) form, design its Repair Items tab, and
define the master-detail relationship between the records displayed in the Summary area of the form and the
records displayed on the Repair Items tab. For the particular service and device selected in the Summary area of
the form, the Repair Items tab will display the records of the stock items that are repair items.
A manager of the Smart Fix company will select a service and device in the Summary area of the form and will add
or remove repair items for this service and device on the Repair Items tab of the form. For each repair item on the
tab, the manager can select the type of the repair item, the stock item, whether the repair item is required, and
whether it should be used by default if multiple repair items of the same type can be used for the selected service
and device.
You will implement the business logic for the Summary area and the Repair Items tab of the form in
Lesson 2.2: Defining the Business Logic and Lesson 4.1: Calculating Field Values.
Description of Form Elements That Are Created in This Lesson
The Summary area of the form will contain the following elements:
•
Service: A box in which the user can select one of the services that has been entered on the custom Repair
Services (RS201000) form. You created this form in the T200 Maintenance Forms training course or published
a customization project that includes it while completing the prerequisites of the current course.
•
Device: A box in which the user can select one of the devices that has been entered on the custom Serviced
Devices (RS202000) form, which was also created in the T200 Maintenance Forms training course or included
in the customization project that you published before completing the current course.
•
Approximate Price: A read-only box that displays the price of the selected service for the selected device,
which is an approximate price for the corresponding work order. In this lesson, you will only specify the
default value for this field; the calculation of the field value will be defined in Lesson 4.1: Calculating Field
Values.
The Repair Items tab will contain the following columns:
•
Repair Item Type: A drop-down list box that contains the type of the repair item, which is one of the
following:
•
Battery
•
Screen
•
Screen Cover


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 32
•
Back Cover
•
Motherboard
•
Required: A check box that indicates (if selected) that this repair item type is required
•
Inventory ID: A box in which the user can select one of the stock items defined on the Stock Items
(IN202500) form
•
Description: A read-only box with the description of the stock item selected in the Inventory ID column
•
Price: The price of the repair item
•
Default: A check box that indicates (if selected) that this repair item should be used by default if multiple
repair items of the same repair item type can be used for the selected service and device
The following screenshot shows the form as it will look aer you complete this lesson.
Figure: Services and Prices form
Relationships Between Database Tables
The repair prices for particular services and devices (represented by the records of the RSSVRepairPrice
database table) and the stock items (represented by the records of the InventoryItem database table) will have
a many-to-many relationship: The same stock item can be included in the repair prices for multiple services and
devices, and a repair price for a particular service and device can include multiple stock items. The many-to-many
links between records will be stored in the separate RSSVRepairItem custom table (see the following diagram).
For the Services and Prices form, RSSVRepairItem records will be details for the RSSVRepairPrice class.
Once a repair price (master record) is deleted, the links to stock items (detail records) should also be deleted from
the database.
The following diagram illustrates the relationships between the database tables that will be used in this lesson.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 33
Lesson Objectives
In this lesson, you will learn how to do the following:
•
Define the master-detail relationship between data
•
Implement automatic numbering of detail records
Step 2.1.1: Creating the Form—Self-Guided Exercise
In this step, you will create the Services and Prices (RS203000) form on your own. Although this is a self-guided
exercise, you can use the details and suggestions in this topic as you create the form. The creation of a form is
described in detail in the T200 Maintenance Forms training course.
If you are using the Customization Project Editor to complete the self-guided exercise, you can perform the
following instructions:
1. Create the form and graph as follows:
a. On the toolbar of the Customized Screens page of the Customization Project Editor, click Create New
Screen.
b. In the Create New Screen dialog box, which opens, specify the following values:
•
Screen ID: RS.20.30.00
•
Graph Name: RSSVRepairPriceMaint
•
Graph Namespace: PhoneRepairShop
•
Page Title: Services and Prices
•
Template: FormTab
•
Create Modern UI Files: Selected
c. Move the generated RSSVRepairPriceMaint graph to the extension library.
2. Create and configure the DACs by doing the following:
a. On the Code Editor page, generate the RSSVRepairPrice and RSSVRepairItem DACs, and move
them to the extension library.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 34
b. In the Helper\Messages.cs file, in the Messages class, add the following constant for the user-
friendly DAC names.
        public const string RSSVRepairPrice = "Repair Price";
        public const string RSSVRepairItem = "Repair Item";
c. Configure the RSSVRepairPrice and RSSVRepairItem DACs in Visual Studio as follows:
•
RSSVRepairPrice: Specify the attributes as shown in the code fragments below:
•
For the DAC:
    [PXCacheName(Messages.RSSVRepairPrice)]
•
For the system fields:
        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime :
            PX.Data.BQL.BqlDateTime.Field<createdDateTime>
        { }
        #endregion
        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID :
            PX.Data.BQL.BqlGuid.Field<createdByID>
        { }
        #endregion
        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string? CreatedByScreenID { get; set; }
        public abstract class createdByScreenID :
            PX.Data.BQL.BqlString.Field<createdByScreenID>
        { }
        #endregion
        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime :
            PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime>
        { }
        #endregion
        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID :
            PX.Data.BQL.BqlGuid.Field<lastModifiedByID>
        { }
        #endregion
        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string? LastModifiedByScreenID { get; set; }


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 35
        public abstract class lastModifiedByScreenID :
            PX.Data.BQL.BqlString.Field<lastModifiedByScreenID>
        { }
        #endregion
        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[]? Tstamp { get; set; }
        public abstract class tstamp :
            PX.Data.BQL.BqlByteArray.Field<tstamp>
        { }
        #endregion
        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
For details about the definition of the attributes of the system fields, see Audit Fields, Concurrent
Update Control (TStamp), and Attachment of Additional Objects to Data Records (NoteID) in the
documentation.
•
For the ServiceID field:
        #region ServiceID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Service", Required = true)]
        [PXSelector(typeof(Search<RSSVRepairService.serviceID>),
            typeof(RSSVRepairService.serviceCD),
            typeof(RSSVRepairService.description),
            SubstituteKey = typeof(RSSVRepairService.serviceCD),
            DescriptionField = typeof(RSSVRepairService.description))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID :
            PX.Data.BQL.BqlInt.Field<serviceID>
        { }
        #endregion
•
For the DeviceID field:
        #region DeviceID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Device", Required = true)]
        [PXSelector(typeof(Search<RSSVDevice.deviceID>),
            typeof(RSSVDevice.deviceCD),
            typeof(RSSVDevice.description),
            SubstituteKey = typeof(RSSVDevice.deviceCD),
            DescriptionField = typeof(RSSVDevice.description))]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID :
            PX.Data.BQL.BqlInt.Field<deviceID>
        { }
        #endregion


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 36
The PXSelector attributes added to the ServiceID and DeviceID fields must
have the SubstituteKey property set to RSSVRepairService.serviceCD
and RSSVDevice.deviceCD, respectively. Whenever you need to create a
selector control based on a DAC that contains a CD key field and a numeric ID field
mapped to identity column, you must use the ID field in the BQL query for the
selector control and substitute the ID field with the CD field.
•
For the Price field:
        #region Price
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Approximate Price", Enabled = false)]
        public virtual Decimal? Price { get; set; }
        public abstract class price : PX.Data.BQL.BqlDecimal.Field<price> { }
        #endregion
You will define other fields of this DAC in the remaining steps of this lesson.
•
RSSVRepairItem: Specify the system attributes and other attributes as shown in the code
fragments below:
•
For the DAC:
    [PXCacheName(Messages.RSSVRepairItem)]
•
For the system fields add the same attribute as for the RSSVRepairPrice DAC
•
For the InventoryID field:
        #region InventoryID
        [Inventory]
        [PXDefault]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID :
            PX.Data.BQL.BqlInt.Field<inventoryID>
        { }
        #endregion
You use the Inventory attribute defined in the PX.Objects.IN namespace in the Acumatica
ERP code. This attribute, which is defined from the PXDimensionSelector attribute, defines
a selector control for the inventory ID. This selector control displays only the inventory items for
which the current user has access rights and that do not have the Inactive or Marked for Deletion
status.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 37
You can find the attribute that can be used for a selector field that retrieves the data
from an Acumatica ERP database table by investigating the source code of a similar
selector field in the application. For example, if you want to find an attribute that
can be used with the inventory ID selector field in a document detail, you can do the
following:
a. On the Shipments (SO302000) form, click Customization > Inspect Element and
click the header of the Inventory ID column on the Details tab.
b. In the Element Properties dialog box, notice the data field name
(InventoryID) and click Actions > View Data Class Source.
c. In the Source Code Browser, find the needed attribute of the InventoryID field,
which is shown in the following screenshot.
Figure: InventoryID attribute
•
For the BasePrice field:
        #region BasePrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Price")]
        public virtual Decimal? BasePrice { get; set; }
        public abstract class basePrice :
            PX.Data.BQL.BqlDecimal.Field<basePrice>
        { }
        #endregion
•
For the Required field:
        #region Required
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Required")]
        public virtual bool? Required { get; set; }
        public abstract class required :
            PX.Data.BQL.BqlBool.Field<required>
        { }


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 38
        #endregion
•
For the IsDefault field:
        #region IsDefault
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Default")]
        public virtual bool? IsDefault { get; set; }
        public abstract class isDefault :
            PX.Data.BQL.BqlBool.Field<isDefault>
        { }
        #endregion
•
For the RepairItemType field
        #region RepairItemType
        [PXDBString(2, IsFixed = true)]
        [PXStringList(
            new string[]
            {
                RepairItemTypeConstants.Battery,
                RepairItemTypeConstants.Screen,
                RepairItemTypeConstants.ScreenCover,
                RepairItemTypeConstants.BackCover,
                RepairItemTypeConstants.Motherboard
            },
            new string[]
            {
                Messages.Battery,
                Messages.Screen,
                Messages.ScreenCover,
                Messages.BackCover,
                Messages.Motherboard
            })]
        [PXUIField(DisplayName = "Repair Item Type")]
        public virtual string? RepairItemType { get; set; }
        public abstract class repairItemType :
            PX.Data.BQL.BqlString.Field<repairItemType>
        { }
        #endregion
You will define other fields of this DAC in the remaining steps of this lesson.
3. Configure the RSSVRepairPriceMaint graph: Define a data view in the generated graph and make the
full list of standard system actions available by specifying the second generic type parameter in the base
PXGraph class, as the following code shows.
using System;
using PX.Data;
using PX.Data.BQL.Fluent;
namespace PhoneRepairShop
{
    public class RSSVRepairPriceMaint :
        PXGraph<RSSVRepairPriceMaint, RSSVRepairPrice>
    {


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 39
        #region Data Views
        public SelectFrom<RSSVRepairPrice>.View RepairPrices = null!;
    
        public PXFilter<MasterTable> MasterView;
        public PXFilter<DetailsTable> DetailsView;
        [Serializable]
        public class MasterTable : PXBqlTable, IBqlTable
        {
        }
        [Serializable]
        public class DetailsTable : PXBqlTable, IBqlTable
        {
        }
        #endregion
    }
}
The fluent BQL classes, which are used for the definition of the data view, are available in the
PX.Data.BQL.Fluent namespace.
4. Build the project in Visual Studio and publish the customization project.
5. Configure the frontend of the RS203000 screen:
a. On the Modern UI Files page of the Customization Project Editor, click Export to Development Folder to
export all Modern UI files to the development folder.
b. Open the RS203000.ts file located in the development and do the following:
a. In the graphInfo decorator, specify the RepairPrices view as the primary view.
b. In the RS203000 class, initialize the RepairPrices view, as shown below.
@viewInfo({containerName: "Repair Prices"})
RepairPrices = createSingle(RSSVRepairPrice);
c. Define the RSSVRepairPrice view class as shown below.
export class RSSVRepairPrice extends PXView {
  ServiceID: PXFieldState;
  DeviceID: PXFieldState;
  Price: PXFieldState;
}
c. In the RS203000.html file, define the Summary area by using the qp-fieldset tags inside the qp-
template tag, as shown below.
<qp-template id="form-RepairPrices" name="17-17-14">
  <qp-fieldset id="fsColumnA" view.bind="RepairPrices" slot="A">
    <field name="ServiceID"></field>
    <field name="DeviceID"></field>
  </qp-fieldset>
  <qp-fieldset id="fsColumnA" view.bind="RepairPrices" slot="B">
    <field name="Price"></field>
  </qp-fieldset>


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 40
</qp-template>
You can leave the qp-tabbar tag because you will use it in a later lesson.
d. On the Modern UI Files page, click Detect Modified Files. In the Modified Files Detected dialog box,
make sure the Selected check box is selected for the RS203000.ts and RS203000.html files, and
click Update Customization Project.
6. Publish the customization.
7. On the Access Rights by Screen (SM201020) form, make sure Delete access rights for the Services and Prices
(RS203000) form are provided for the Customizer user role. The Services and Prices form is located under
the Hidden node in the le pane of the Access Rights by Screen form. This is because the form is yet to be
assigned to any workspace.
8. Make sure access rights for the new form are included in the customization project.
9. Include a link to the form in the Configuration category of the Phone Repair Shop workspace, and make it
available in the quick menu.
10.As a substitute form, use the generic inquiry from the ServicesAndPrices.xml file, which is provided
with this course. For details about how to add a substitute form, see Lesson 2.4: Create a Substitute Form in
the T200 Maintenance Forms training course.
The generic inquiry is provided in the Customization\T210\SourceFiles
\ListAsEntryPoint folder, which you have downloaded from Acumatica GitHub.
11.Update the customization project as follows:
•
Add the Services and Prices generic inquiry to the customization project
•
Include access rights for the generic inquiry in the customization project
•
Update the SiteMapNode item for the Services and Prices form in the customization project
Step 2.1.2: Defining the Master-Detail Relationship Between Data (with PXParent
and PXDBDefault)
In this step, you will set up the master-detail relationship between the RSSVRepairPrice and
RSSVRepairItem DACs and define the detail data view, which selects the items that are used for a particular
repair service provided for a particular device.
Changes to the Detail DAC
To set up the master-detail relationship between the RSSVRepairPrice and RSSVRepairItem data access
classes, you will add the PXDBDefault and PXParent attributes to the key fields of the detail class, which is
RSSVRepairItem.
The PXDBDefault attribute specifies the value that is inserted into a field of the detail records from the field of
the current (at runtime) master data record.
The PXParent attribute defines the master-detail relationship between the data access classes. In particular,
the attribute enables cascading deletion of the detail records when the master data record is deleted. When a
price record is deleted, the corresponding detail records that match the specified query will also be deleted. The
PXParent attribute can be added to any data field of the detail class in the master-detail relationship. However,
we recommend that you add it to the declaration of the first foreign key.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 41
Changes to the Graph
You will use fluent BQL to define the detail data view.
To retrieve data from the database, Acumatica Framework converts the fluent BQL query to an SQL
command and executes this command. You can trace executed SQL commands on the Request
Profiler (SM205070) form. For more information, see To Validate a BQL Statement and To Measure the
Execution Time of a BQL Statement.
Instructions for Defining the Master-Detail Relationship
To define the master-detail relationship, do the following:
1. In the RSSVRepairItem DAC, define the ServiceID and DeviceID fields as follows:
a. Add the PXDBInt and PXDBDefault attributes to the ServiceID field, as shown below.
        #region ServiceID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.serviceID))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion
The PXDBDefault attribute inserts the default value into the key field of the detail class, which is the
key to the master record. For each new RSSVRepairPrice object, the framework inserts the ID of the
current service into the RSSVRepairItem.ServiceID data field. The field isn't intended for editing
in the UI; thus, it does not have the PXUIField attribute.
b. Add the PXParent attribute to the ServiceID field as follows.
        #region ServiceID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.serviceID))]
        [PXParent(typeof(SelectFrom<RSSVRepairPrice>.
            Where<RSSVRepairPrice.serviceID.
                IsEqual<RSSVRepairItem.serviceID.FromCurrent>.
                And<RSSVRepairPrice.deviceID.
                IsEqual<RSSVRepairItem.deviceID.FromCurrent>>>))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion
The PXParent attribute specifies the master-detail relationship between classes.
Make sure you have added the using PX.Data.BQL.Fluent; directive.
c. Add the PXDBInt and PXDBDefault attributes to the DeviceID field, as shown below.
        #region DeviceID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.deviceID))]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
        #endregion


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 42
You do not need to specify the PXParent attribute for the second key field.
2. In the RSSVRepairPriceMaint graph, define the detail data view, as the following code shows.
        public SelectFrom<RSSVRepairItem>.
            Where<RSSVRepairItem.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>.
            And<RSSVRepairItem.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>>>.View
            RepairItems = null!;
In the RepairItems data view, you pass the device ID and service ID to the query by using the
FromCurrent parameter of the fluent BQL statement. When the framework executes the statement,
it takes the value from the Current property of the PXCache object that holds RSSVRepairPrice
objects in this graph and retrieves records that match the query.
The detail data view must be declared aer the master data view.
3. Build the project.
Related Links
•
Master-Detail Relationship Between Data with PXDBDefault and PXParent
•
PXDBDefaultAttribute Class
•
PXParentAttribute Class
•
Creating Fluent BQL Queries
•
Selection of the Linked Data Through the Current Property
Step 2.1.3: Numbering Detail Records (with PXLineNbr)
Now you will implement the numbering of repair items by using the predefined PXLineNbr attribute. You will use
the line number as a key field of a repair item. (These numbers will be maintained internally and not made visible
on the form.) You need one more key (besides the service ID and device ID) for the repair items and cannot use as a
key the inventory ID because users can add multiple repair items with the same service ID, device ID, and inventory
ID to the Repair Items tab of the Services and Prices (RS203000) form.
The PXLineNbr attribute uses the value of the specified field from the parent data record to keep the last
assigned number, increments this value, and assigns the incremented value to the LineNbr field of the new detail
data records. The PXLineNbr attribute does not work without the PXParent attribute being assigned to a field
of the same DAC.
Numbering Detail Records
Do the following to implement the numbering of detail records:
1. Add the attributes shown in the following code to the LineNbr field of the RSSVRepairItem DAC.
        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(RSSVRepairPrice.repairItemLineCntr))]
        [PXUIField(DisplayName = "Line Nbr.", Visible = false)]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 43
You set the Visible property of the PXUIField attribute to false so that the field is not displayed in
the UI by default.
2. To set the starting number, for the RepairItemLineCntr field of the RSSVRepairPrice DAC, specify
the following PXDefault attribute, and add the PXDBInt attribute. (See the following code.)
        #region RepairItemLineCntr
        [PXDBInt()]
        [PXDefault(0)]
        public virtual Int32? RepairItemLineCntr { get; set; }
        public abstract class repairItemLineCntr :
            PX.Data.BQL.BqlInt.Field<repairItemLineCntr>
        { }
        #endregion
In this code, you specify 0 as the default value, so the first LineNbr value assigned will be 1.
3. Build the project.
Related Links
•
PXLineNbrAttribute Class
Step 2.1.4: Creating Controls on the Form
In this step, you will define the grid that displays detail lines on the Services and Prices (RS203000) form.
Definition in TypeScript
In TypeScript, configure the form as follows:
1. Define the view class with the following fields:
•
RepairItemType
•
Required
•
InventoryID
•
InventoryID_description
In a column, you can display a value of a data field that is displayed in the lookup table of another field
(in this example, the description of the inventory ID). To do this, you specify the name of the selector
field and the name of the field from the lookup table separated by one underscore character, such as
InventoryID_description.
•
BasePrice
•
IsDefault
In the RS203000.ts file, the code of the view class should look as follows.
export class RSSVRepairItem extends PXView {
    RepairItemType : PXFieldState;
    Required : PXFieldState;
    InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
    InventoryID_description : PXFieldState;
    BasePrice : PXFieldState;
    IsDefault : PXFieldState;
}
2. Specify the following properties for the grid in the gridConfig decorator:


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 44
•
preset: GridPreset.Details
You set the preset property for a grid to assign a style and a set of toolbar buttons to the container. The
Details value is used to render a table with detail lines in a master-detail data entry form. The table has
a toolbar that holds the default actions, such as Refresh, Add, Remove, Fit to Screen, and Export to
Excel; it can also hold custom actions. The table has no caption and paging is allowed. For other possible
values of the preset property, see Form Layout: Grid Presets in the documentation.
•
initNewRow: true
Normally, when a user clicks the Add Row button on a grid toolbar, the new empty row is created only on
the web page and the data is not sent to the application server. It means that the C# code with all event
handlers is not performed. For the data to be sent to the server, you need to specify InitNewRow =
True. Also, by specifying this property, you prevent the system from creating an extra row automatically
when a user clicks the Save button.
The code is shown below.
@gridConfig({
  preset: GridPreset.Details,
  initNewRow: true
})
export class RSSVRepairItem extends PXView { ... }
3. Initialize the RepairItems view in the RS203000 class, as shown below.
 @viewInfo({containerName: "Repair Items"})
    RepairItems = createCollection(RSSVRepairItem);
Defining Tabs
In the Modern UI, tabs are defined by a parent qp-tabbar tag and nested qp-tab tags. For each tab, you need to
define a qp-tab control and specify its id and caption. For more details, see Tab.
Definition in HTML
In the RS203000.html file, do the following:
1. In the qp-tabbar control, specify the proper ID: tabsPrices.
2. In the first qp-tab tag, specify the proper ID (tabRepairItems) and the caption (Repair Items).
3. In the first qp-tab tag, add the qp-grid tag and bind it to the RepairItems view.
4. Remove the second qp-tab tag.
The resulting HTML code should look as shown below.
<template>
     
<qp-template id="RepairPrices" name="1-1-1"> 
 <qp-fieldset id="fsColumnA-RepairPrices" view.bind="RepairPrices" slot="A" >
  <field name="ServiceID" ></field>
     <field name="DeviceID" ></field>
 </qp-fieldset>
 <qp-fieldset id="fsColumnB-RepairPrices" view.bind="RepairPrices" slot="B" >
  <field name="Price" ></field>
 </qp-fieldset>
</qp-template> 
<qp-tabbar id="tabsPrices">
 <qp-tab id="tabRepairItems" caption="Repair Items"> 


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 45
         <qp-grid id="gridRepairItems" view.bind="RepairItems"></qp-grid>
    </qp-tab>
</qp-tabbar>
 
</template>
Finally, update the RS203000.ts and RS203000.html files in the customization project and publish the
project.
Step 2.1.5: Testing the Tab
In this step, on the Services and Prices (RS203000) form, you will test the master-detail relationship that you have
developed.
Do the following:
1. Open the Services and Prices form, and add a new record with the following values in the Summary area:
•
Service: Battery Replacement
•
Device: Nokia 3310
2. On the Repair Items tab, add rows with the following settings.
Repair Item Type
Required
Inventory ID
Price
Default
Battery
Cleared
BAT3310
20
Cleared
Back Cover
Cleared
BCOV3310
10
Cleared
You need to specify the price because the Price column in the database requires a not null
value. You will define automatic change of the Price box value in Lesson 2.2: Defining the
Business Logic.
3. On the form toolbar, click Save.
4. In SQL Server Management Studio, make sure the data you specified has been added to the
RSSVRepairPrice and RSSVRepairItem tables.
5. On the Services and Prices form, delete the record you have created.
6. In SQL Server Management Studio, make sure the data has been removed from the RSSVRepairPrice
and RSSVRepairItem tables.
Lesson Summary
In this lesson, you have learned how to set up a master-detail relationship between data.
To create a master-detail form, you have completed the following actions:
1. Set up the master-detail relationship between data access classes as follows:
•
Added the PXDBDefault attribute to the key data fields of the detail DAC that are the keys to the
master record. The PXDBDefault attribute provides the default value for the key field of the detail DAC.
•
Added the PXParent attribute to the first foreign key data field of the detail DAC. The PXParent
attribute enables cascading deletion of detail records on deletion of the master record.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 46
2. Defined two data views that select the master-detail data for the form. You have used the FromCurrent
parameter in the detail data view type to select records for a particular master record.
3. Defined and configured the UI control that display the data on the form as follows:
•
Defined a view class for the Summary area and a view class for the table
•
Defined a screen class and initialized two view classes in it
•
Defined the Summary area with the qp-template tag and two nested qp-fieldset tags
•
Defined the qp-tab control and the qp-grid control for the detail lines
•
Specified the master data view for the two fieldsets in the Summary area (the view.bind property of
the qp-fieldset tag)
•
Specified the detail data view for the grid (the view.bind property of the qp-grid tag)
In the lesson, you have also implemented the numbering of detail data records by using the PXLineNbr attribute.
Additional Information: Relationships Between DACs
In this lesson, you have defined the relationship between DACs with the PXParent attribute. To define
relationships between DACs, you can also use other approaches, which are outside of the scope of this course but
may be useful to some readers.
Relationship Between Data with PrimaryKeyOf and ForeignKeyOf
In the code of an Acumatica Framework-based application, you can define the relationship between two tables as
follows:
•
To define a primary key of a table, for the set of key fields of the data access class (DAC) that corresponds to
the table, you set the IsKey property of the data type attribute to true.
•
To define a foreign key of a table, in the DAC that corresponds to the table, you mark the field that
contains the foreign key with one of the following attributes: PXForeignReference, PXSelector, or
PXParent.
Another way to define a relationship between two tables is to use the PrimaryKeyOf and ForeignKeyOf
classes that are specially designed for the definition of primary and foreign keys.
For details about these classes, see Relationship Between Data with PrimaryKeyOf and ForeignKeyOf in the
documentation.
Lesson 2.2: Defining the Business Logic
In this lesson, you will define the business logic of the Services and Prices (RS203000) form. You will implement the
logic to meet the following requirements for the Repair Items tab:
•
For a particular row, if a value is selected in the Repair Item Type column, the Inventory ID column will
display only the stock items that are repair items and have the selected repair item type. If no value is
selected in the Repair Item Type column, the Inventory ID column will display all stock items that are
repair items.
•
For a particular row, if a value is selected in the Inventory ID column, the values in the Repair Item Type
and Price columns will be changed to the repair item type and base price (respectively) of the selected stock
item as specified on the Stock Items (IN202500) form.
•
If the Default check box is selected for a repair item listed in the grid, this check box must be cleared for all
other repair items of the same repair item type.
•
For all repair items of the same repair item type, the state of the Required check box must be identical.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 47
•
If a value is selected in the Repair Item Type column for any row, the system should set the state of the
Required check box for this row to the state of the Required check box specified in other rows that have the
same repair item type as the selected one.
Lesson Objectives
As you complete this lesson, you will learn how to do the following:
•
Restrict the possible values of a field by using the PXRestrictor attribute
•
Mark localizable messages in code
•
Update the fields of the same data record on update of a field of this record
•
Update the fields of other records on update of a field
•
Learn one of the possible ways to retrieve a data record from the database in code by using the static
PXSelectorAttribute.Select<>() method
Step 2.2.1: Restricting the Values of a Field (with PXRestrictor)
In this step, you will define the logic of the selector control in the Inventory ID column of the Repair Items tab of
the Services and Prices (RS203000) form. You will restrict the values available in the selector control when a repair
item type is selected in the Repair Item Type column.
Changes in the DAC
To configure the restriction, you will use the PXRestrictor attribute for the InventoryID field of the
RSSVRepairItem DAC. In the first parameter of the PXRestrictor attribute constructor, you will specify the
restriction that limits the records displayed in the Inventory ID selector control. In the constructor, you will also
use an error message defined in a class with the PXLocalizable attribute specified to make the text available
for localization.
For the PXRestrictor attribute to work correctly, the PXRestrictor attribute must be used along with
a PXSelector or PXDimensionSelector attribute. In this example, the InventoryID field has the
Inventory attribute, which is derived from the PXDimensionSelector attribute.
Changes in the TypeScript Code
To update the data in the Inventory ID selector control based on the restriction configured in the PXRestrictor
attribute, you will set to True the following properties in TypeScript:
•
The CommitChanges property for the Repair Item Type column
•
The SyncPosition property of the RepairItems grid
If you want a selector control to react to the changes in the UI, you use the SyncPosition property. Otherwise a
user will have to manually click the Refresh button in the selector control to see the changed data.
With the SyncPosition property set to True, the selected row can be synchronized with the Current property
of the cache object. The Current property, which you will use in the PXRestrictor attribute, is set to each row
selected by the user in the grid.
Instructions for Restricting the Values of a Field
Proceed as follows:
1. In Visual Studio, add the using PX.Common; directive to the Messages.cs file and the using
PX.Data.BQL; directive to the RSSVRepairItem.cs file.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 48
2. Add the PXLocalizable attribute to the Messages class.
3. In the Messages class, include the constant that defines the error message to be used in the
PXRestrictor attribute, as the following code shows.
        public const string StockItemIncorrectRepairItemType =
            "This stock item has a repair item type that differs from {0}.";
4. In the RSSVRepairItem DAC, add the PXRestrictor attribute to the InventoryID field, as shown in
the following code.
        #region InventoryID
        [PXRestrictor(typeof(
            Where<InventoryItemExt.usrRepairItem.IsEqual<True>.
                And<Brackets<
                    RSSVRepairItem.repairItemType.FromCurrent.IsNull.
                        Or<InventoryItemExt.usrRepairItemType.
                            IsEqual<RSSVRepairItem.repairItemType.FromCurrent>>>>>),
            Messages.StockItemIncorrectRepairItemType,
            typeof(RSSVRepairItem.repairItemType))]
        [Inventory]
        [PXDefault]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID :
            PX.Data.BQL.BqlInt.Field<inventoryID>
        { }
        #endregion
5. Build the project.
6. In the RS203000.ts file, specify PXFieldOptions.CommitChanges option for the
RepairItemType field, as shown below.
    RepairItemType : PXFieldState<PXFieldOptions.CommitChanges>;
7. In the RS203000.ts, file, in the gridConfig decorator for the RSSVRepairItem class, specify
syncPosition: true, as shown below.
@gridConfig({
 preset: GridPreset.Details,
 syncPosition: true,
 initNewRow: true
})
export class RSSVRepairItem extends PXView { ... }
8. Update the Modern UI files in the customization project and publish the project.
Instructions for Testing the Restriction
On the Services and Prices (RS203000) form, do the following:
1. Create a new record and specify the Battery Replacement service and the Nokia 3310 device for it.
2. Add a new row on the Repair Items tab, and do not select anything in the Repair Item Type column. Click
the magnifier icon in the Inventory ID column. Notice the list of stock items displayed in the lookup table.
This list includes all active stock items that are repair items.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 49
3. In the Repair Item Type column, select Battery. Click the magnifier icon in the Inventory ID column. The list
of stock items contains only two stock items—the ones with the Battery repair item type—as shown in the
following screenshot.
Figure: Inventory ID selector control
4. Do not save the record.
Related Links
•
PXRestrictorAttribute Class
•
PXSelectorAttribute Class
•
PXDimensionSelectorAttribute Class
•
UI Localization: Application Messages in C#
•
Managing Segmented Keys
Step 2.2.2: Updating Fields of the Same Record on Update of a Field (with
FieldUpdated and FieldDefaulting)
In this step, you will add code that does the following when the RSSVRepairItem.InventoryID value is
changed: It copies the RSSVRepairItem.BasePrice and RSSVRepairItem.RepairItemType values
from the stock item record that has the ID equal to the new RSSVRepairItem.InventoryID value.
You will use the FieldUpdated event handler for the RSSVRepairItem.InventoryID field to update the
values of the following fields of the same record:
•
RSSVRepairItem.RepairItemType: Instead of directly assigning the value to this field, you will call
the SetValueExt<field> method to assign the value and invoke the FieldUpdated event handler
for this field.
•
RSSVRepairItem.BasePrice: You will trigger the FieldDefaulting event for this field
by using the SetDefaultExt<field> method of PXCache. You will assign the value of the
RSSVRepairItem.BasePrice field in the FieldDefaulting event handler.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 50
You will not assign the value of the RSSVRepairItem.BasePrice field in the
FieldUpdated event handler, because this field may depend on multiple fields of the same
record. For example, the price can depend on not only the item selected in the line but also the
discount specified for this line. In this example, the RSSVRepairItem.BasePrice field
depends only on the RSSVRepairItem.InventoryID value, but we recommend that you
use this approach for the fields that may depend on multiple fields of the same record.
In the FieldUpdated and FieldDefaulting handlers, you will use the
PXSelectorAttribute.Select<>() method to select the stock item record with the inventory ID that has
been selected in the updated field. The PXSelectorAttribute.Select<>() method uses the BQL query
from PXSelector on the specified field.
In the FieldDefaulting handler, you will use the PK.Find() method, which selects a record by using the
values of the key fields of the record, to retrieve the value of the base price of the stock item. For details about
definition of primary keys, see Relationship Between Data with PrimaryKeyOf and ForeignKeyOf.
Updating Fields of the Same Record
To update multiple fields of the same record, do the following:
1. In the RSSVRepairPriceMaint.cs file, add the PX.Objects.IN using directive.
2. Define the FieldUpdated event handler for the RSSVRepairItem.InventoryID field in the
RSSVRepairPriceMaint class as follows.
        //Update the price and repair item type when the inventory ID of
        //the repair item is updated.
        protected void _(Events.FieldUpdated<RSSVRepairItem,
            RSSVRepairItem.inventoryID> e)
        {
            RSSVRepairItem row = e.Row;
            if (row.InventoryID != null && row.RepairItemType == null)
            {
                //Use the PXSelector attribute to select the stock item.
                var item = PXSelectorAttribute.
                    Select<RSSVRepairItem.inventoryID>(e.Cache, row)
                    as InventoryItem;
                //Copy the repair item type from the stock item to the row.
                var itemExt = item?.GetExtension<InventoryItemExt>();
                if (itemExt != null)
                   e.Cache.SetValueExt<RSSVRepairItem.repairItemType>(
                    row, itemExt.UsrRepairItemType);
            }
            //Trigger the FieldDefaulting event handler for basePrice.
            e.Cache.SetDefaultExt<RSSVRepairItem.basePrice>(e.Row);
        }
3. Define the FieldDefaulting event handler for the RSSVRepairItem.basePrice field in the
RSSVRepairPriceMaint class as follows to calculate the default value of the field.
        //Set the value of the Price column.
        protected void _(Events.FieldDefaulting<RSSVRepairItem,
            RSSVRepairItem.basePrice> e)
        {
            RSSVRepairItem row = e.Row;
            if (row.InventoryID != null)
            {


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 51
                //Use the PXSelector attribute to select the stock item.
                var item = PXSelectorAttribute.
                    Select<RSSVRepairItem.inventoryID>(e.Cache, row)
                    as InventoryItem;
                //Retrieve the base price for the stock item.
                var curySettings =
                    InventoryItemCurySettings.PK.Find(
                    this, item?.InventoryID, Accessinfo.BaseCuryID ?? "USD");
                //Copy the base price from the stock item to the row.
                if (curySettings != null) e.NewValue = curySettings.BasePrice;
            }
        }
4. Build the project.
5. In the RS203000.ts file, make sure that the PXFieldOptions.CommitChanges option is specified
for the InventoryID property of the RSSVRepairItem view class.
You can edit TypeScript and HTML files in your customization project in the following ways:
•
By using the built-in editor: You do this by clicking the needed file on the Modern UI Files
files page of the Customization Project Editor, which opens the file in the built-in editor.
Note that you cannot edit system-generated TypeScript or HTML extensions by using this
method.
•
By using an external editor: You do this by first exporting the Modern UI files in your
customization project to the FrontendSources\screen\src\development
folder of your instance. Then you use the external editor to open the needed file within
the appropriate subfolder of the development\screens folder. You export the files by
clicking Export to Development Folder on the page toolbar of the Modern UI Files page.
6. Update the Modern UI files in the customization project and publish the project.
Testing the Logic
On the Services and Prices (RS203000) form, do the following:
1. In the Summary area, select the Battery Replacement service and the Nokia 3310 device.
2. On the Repair Items tab, add a row, and select Battery in the Repair Item Type column and BAT3310 in the
Inventory ID column. Shi the focus away from the column. Make sure the system has filled in values in the
Description and Price columns.
3. Save the record.
Related Links
•
Access to a Custom Field
•
PXSelectorAttribute Class
Step 2.2.3: Updating a Field of Another Record on Update of a Field (with
RowUpdated)
In this step, you will change the business logic so that when a field of a detail record is updated, the values that
depend on this field will be changed in other detail records. On the Repair Items tab of the Services and Prices
(RS203000) form, when the Default check box is selected for a repair item, this check box must be cleared for other
repair items with the same repair item type.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 52
To update the field in other detail lines, you will use the RowUpdated event handler.
If you are wondering if you could use a FieldUpdated event handler in this case we do not
recommend this approach, because it can cause data inconsistency. With FieldUpdated, if the
update of the current detail record did not finish—for example, due to a validation error—the changes
in other detail records would not be discarded.
In the RowUpdated event handler, you will use the following techniques:
•
You will use LINQ to filter the data of the data view that provides data for the Repair Items tab.
You can use language-integrated query (LINQ) provided by the System.Linq library when
you need to select records from the database in the code of Acumatica Framework-based
applications or if you want to apply additional filtering to the data of a BQL query. However,
you still have to use business query language (BQL) to define the data views in graphs and to
specify the data queries in attributes of data fields.
•
You will use the Update() method of the RepairItems data view to update repair items in PXCache.
The data view's Update() method just invokes the Update() method of the PXCache object. The
Update() method of the PXCache object raises field-level events for each field of the modified record
and row-level events for the modified record. You need to use this method to update PXCache if the event
handler modifies records other than the record for which the event has been raised. If you do not call this
method in the RowUpdated event handler, the changes that you've made in the event handler may not be
saved to the database and can cause data inconsistency issues.
Because you cannot modify another record in FieldUpdated event handlers, you should
never call the PXCache.Update() method in one of these handlers.
•
You will use the PXView.RequestRefresh() method. To optimize the performance, by default, in the
RowUpdated event, the system synchronizes the values in the UI with the data in PXCache for only the
current record. Because you have updated the values in other detail records, you need to refresh the UI by
using the PXView.RequestRefresh() method.
Updating Fields in Detail Records
Do the following to update fields in detail records on update of a field:
1. In the RSSVRepairPriceMaint.cs file, add the using System.Linq; directive.
2. In the RSSVRepairPriceMaint class, add the following event handler.
        //Update the IsDefault field of other records with the same repair item type 
        //when the IsDefault field is updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            // Make sure the handler runs only when the IsDefault field is edited.
            if (e.Cache.ObjectsEqual<RSSVRepairItem.isDefault>(e.Row, e.OldRow))
                return;
            RSSVRepairItem row = e.Row;
            //Use LINQ to select the repair items 
            // with the same repair item type as in the updated row.
            var repairItems = RepairItems.Select().Where(item =>
                item.GetItem<RSSVRepairItem>().RepairItemType == row.RepairItemType);
            foreach (RSSVRepairItem repairItem in repairItems)


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 53
            {
                if (repairItem.LineNbr == row.LineNbr) continue;
                //Set IsDefault to false for all other items.
                if (row.IsDefault == true && repairItem.IsDefault == true)
                {
                    repairItem.IsDefault = false;
                    RepairItems.Update(repairItem);
                }
            }
            //Refresh the UI.  
            RepairItems.View.RequestRefresh();
        }
3. Build the project.
4. In the RS203000.ts file, specify PXFieldOptions.CommitChanges for the IsDefault property in
the RSSVRepairItem view class.
5. Update the Modern UI files in the customization project and publish the project.
Testing the Default Field
On the Services and Prices (RS203000) form, do the following:
1. Select the price record for the Battery Replacement service and the Nokia 3310 device.
2. On the Repair Items tab, add a new line with the following settings:
•
Repair Item Type: Battery
•
Inventory ID: BAT3310EX
•
Default: Selected
3. Add another line with the following settings:
•
Repair Item Type: Back Cover
•
Inventory ID: BCOV3310
•
Default: Selected
4. Save the record.
5. On the Repair Items tab, select the Default check box for the BAT3310 repair item (which has the Battery
repair item type). Make sure that this check box has become cleared for the BAT3310EX repair item (which
also has the Battery repair item type) and remains selected for the BCOV3310 repair item (which has the
Back Cover repair item type).
6. Save the record, and make sure the values of the Default check box in all rows have not changed aer you
saved it.
Related Links
•
Creating LINQ Queries
•
PXView Class
Step 2.2.4: Updating Fields on Update of Another Field—Self-Guided Exercise
In this step, you will define the UI logic of the Repair Item Type and Required columns on the Services and Prices
(RS203000) form on your own.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 54
The Required and Repair Item Type columns must meet the following criteria:
•
For all repair items of the same repair item type, the state of the Required check box must be identical.
•
If a value is selected in the Repair Item Type column for any row, the system should set the state of the
Required check box for this row to the state of the Required check box specified in other rows that have the
same repair item type as the selected one.
•
For the Repair Item Type column, when its value is edited for a row, the system should clear the values of
the Inventory ID and Default columns in this row.
As you implement the logic, use the tips described in the following sections.
Updating Fields of the Same Record on Update of the RepairItemType Field
You should use the same approach as was described in Step 2.2.2: Updating Fields of the Same Record on Update of
a Field (with FieldUpdated and FieldDefaulting). Specifically, you should do the following:
•
Define the FieldUpdated event handler for the RSSVRepairItem.repairItemType field in the
RSSVRepairPriceMaint class.
•
In the event handler, clear the RSSVRepairItem.inventoryID and RSSVRepairItem.isDefault
field values when a repair item type is updated by using the SetValueExt<field> and
SetValue<field> methods of PXCache. Use the SetValueExt<field> method to set the
value of the InventoryID field and to trigger the FieldUpdated event for this field right aer the
RSSVRepairItem.repairItemType field is updated. Use the SetValue<field> method for the
IsDefault field.
•
In the event handler, trigger the FieldDefaulting event for the RSSVRepairItem.required field
by using the SetDefaultExt<field> method of PXCache.
•
In the FieldDefaulting event handler for the RSSVRepairItem.required field, use LINQ to check
whether there are any repair items with the same repair item type and copy the Required value from the
previous records.
•
Do not forget to specify PXFieldOptions.CommitChanges for the
RSSVRepairItem.RepairItemType in the RS203000.ts file
The following code fragment shows the result of this section.
        //When Repair Item Type is updated,
        //clear the values of the Inventory ID and Default columns and
        //trigger FieldDefaulting for the Required column.
        protected void _(Events.FieldUpdated<RSSVRepairItem,
            RSSVRepairItem.repairItemType> e)
        {
            RSSVRepairItem row = e.Row;
            e.Cache.SetDefaultExt<RSSVRepairItem.required>(row);
            if (e.OldValue != null)
            {
                e.Cache.SetValueExt<RSSVRepairItem.inventoryID>(row, null);
                e.Cache.SetValue<RSSVRepairItem.isDefault>(row, false);
            }
        }
        //Set the value of the Required column.
        protected void _(Events.FieldDefaulting<RSSVRepairItem,
            RSSVRepairItem.required> e)
        {
            RSSVRepairItem row = e.Row;
            if (row.RepairItemType != null)
            {
                // Use LINQ to check whether there are any repair items 


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 55
                // with the same repair item type.
                var repairItem = (RSSVRepairItem)RepairItems.Select()
                    .FirstOrDefault(item =>
                        item.GetItem<RSSVRepairItem>().RepairItemType ==
                            row.RepairItemType &&
                        item.GetItem<RSSVRepairItem>().LineNbr != row.LineNbr);
                //Copy the Required value from the previous records.
                e.NewValue = repairItem?.Required;
            }
        }
Updating Fields of Another Record on Update of the Required Field
You should use the same approach as was described in Step 2.2.3: Updating a Field of Another Record on Update of a
Field (with RowUpdated).
As you implement the logic, use the following tips:
•
Modify the RowUpdated<RSSVRepairItem> event handler to implement modifications of the repair
item records with the same repair item type as is selected in the updated record.
•
In the RowUpdated event handler, check whether the value of the Required field of the updated record
has changed and update other records by using the following code.
                //Make the Required field identical for all items of the type.
                if (row.Required != e.OldRow.Required &&
                    repairItem.Required != row.Required)
                {
                    repairItem.Required = row.Required;
                    RepairItems.Update(repairItem);
                }
•
Do not forget to specify PXFieldOptions.CommitChanges for the RSSVRepairItem.Required in
the RS203000.ts file
The following code fragment shows the result of this section.
        //Update the IsDefault and Required fields of other records 
        //with the same repair item type when these fields are updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            if (e.Cache.ObjectsEqual<RSSVRepairItem.isDefault,
                                     RSSVRepairItem.required>(e.Row, e.OldRow)) return;
            RSSVRepairItem row = e.Row;
            //Use LINQ to select the repair items 
            // with the same repair item type as in the updated row.
            var repairItems = RepairItems.Select()
                .Where(item => item.GetItem<RSSVRepairItem>()
                .RepairItemType == row.RepairItemType);
            foreach (RSSVRepairItem repairItem in repairItems)
            {
                if (repairItem.LineNbr == row.LineNbr) continue;
                //Set IsDefault to false for all other items.
                if (row.IsDefault == true && repairItem.IsDefault == true)
                {
                    repairItem.IsDefault = false;
                    RepairItems.Update(repairItem);


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 56
                }
                //Make the Required field identical for all items of the type.
                if (row.Required != e.OldRow.Required &&
                    repairItem.Required != row.Required)
                {
                    repairItem.Required = row.Required;
                    RepairItems.Update(repairItem);
                }
            }
            //Refresh the UI.
            RepairItems.View.RequestRefresh();
        }
Testing the Logic
Test the added logic as follows:
1. On the Services and Prices (RS203000) form, add the price record for the Liquid Damage service and the
Nokia 3310 device.
2. On the Repair Items tab, add a new line with the following settings:
•
Repair Item Type: Battery
•
Required: Selected
•
Inventory ID: BAT3310
•
Default: Selected
3. Add another line with the following settings:
•
Repair Item Type: Battery
•
Inventory ID: BAT3310EX
Make sure that the Required check box becomes selected once you select the value in the Repair Item Type
box because you have already defined the Battery repair item type as being required in the first row.
4. Save the record.
5. Clear the Required check box in the row for the BAT3310EX stock item. Make sure the Required check box
becomes cleared in the row for the BAT3310 stock item.
6. Save the record.
7. For the first row in the table, change the value in the Repair Item Type column to Screen and click another
column. Make sure the values in the Inventory ID and Default columns become cleared.
8. Do not save the latest changes.
Lesson Summary
In this lesson, you have configured the business logic of the Repair Items tab of the Services and Prices (RS203000)
form as follows:
•
You have used the PXRestrictor attribute to configure a restriction on the values in the Inventory ID
column.
•
You have used a class with the PXLocalizable attribute specified to make the text available for
localization.
•
You have used the FieldUpdated and FieldDefaulting event handlers to modify the values
of a detail record on update of the column of this detail record. In the event handlers, you have used
the PXSelectorAttribute.Select<>() method to obtain the stock item record with the


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 57
inventory ID selected in the updated field. You have also used the SetValueExt<field> and
SetDefaultExt<field> methods to trigger additional events for particular fields.
•
You have used the RowUpdated event handler so that when particular fields of one detail record are
updated, the values in the other detail records are modified. In this event handler, you have used the
PXCache.Update() method to update in PXCache the records other than the record for which the
event has been raised. You have also used the PXView.RequestRefresh() method to display in the UI
the changes in PXCache that you have made in the event handler. You have also used LINQ for the filtering
of the records of the data view.
Additional Information: Application Localization
In this lesson, you have used messages that can be localized in the PXRestrictor attribute. Application
localization is outside of the scope of this course, but this information may be useful to some readers.
Localization Process
Acumatica ERP provides built-in localization functionality, so you can easily translate Acumatica ERP into any
language without using third-party products. You can collect the strings used in the whole system or on a particular
form, and translate them for any locale available in Acumatica ERP. Once the translated strings are entered and
applied, the application does not require any recompilation or reinstallation.
For more information about how to use the built-in localization mechanism, see Translation Process in the
documentation.
Preparation of the Application Code and DACs for Localization
To prepare an application for localization, you must prepare data access classes (DACs) and the application code.
For details about the preparation of the application code and DACs, see UI Localization: DACs and UI Localization:
Application Messages in C#.
Multilanguage Fields
With Acumatica Framework, you can create fields configured to accept values entered in multiple languages if
multiple locales are defined in Acumatica ERP. For example, if the English and French locales have been set up in
Acumatica ERP, a user can specify the value of the Description box on the Stock Items (IN202500) form in English
and French.
For the information about configuring multilanguage fields in code, see UI Localization: Multilanguage Fields.
Optimization of Memory Consumption of Localized Data
To optimize the memory consumption of static data, you can move the localization data from all customer
application instances to centralized storage. By default, the localization data is kept in the database of every
Acumatica ERP instance, and the total size of this data therefore equals the number of instances times the size of
the data. If you move the localization data to centralized storage, there is only one copy of this data.
See details about this optimization in UI Localization: Optimization of Memory Consumption of Localized Data.


Part 2: Master-Detail Relationship and Business Logic (Services and Prices Form) | 58
Additional Information: Data Querying
In this lesson, you have used fluent BQL for data querying. Details about the data querying in Acumatica
Framework are outside of the scope of this course but may be useful to some readers.
Data Querying in Acumatica Framework
In Acumatica Framework, you generally use business query language (BQL) to query data from the database. BQL
statements represent specific SQL queries and are translated into SQL by Acumatica Framework, which helps you
to avoid the specifics of the database provider and validate the queries at the time of compilation. Acumatica
Framework provides two dialects of BQL: traditional BQL and fluent BQL.
To query data from the database, you can also use language-integrated query (LINQ), which is a part of the .NET
Framework. In the code of Acumatica Framework-based applications, you can use both the standard query
operators (provided by LINQ libraries) and the Acumatica Framework-specific operators that are designed to query
database data.
For details about building queries, see the following chapters in the documentation:
•
Creating Fluent BQL Queries
•
Creating Traditional BQL Queries
•
Creating LINQ Queries
For a comparison of these approaches in data querying, see Comparison of Fluent BQL, Traditional BQL, and LINQ.
Execution of Data Queries in Acumatica Framework
If you want to know how data queries are executed in the system, such as how a BQL statement is converted to an
SQL query, see the following topics in the documentation:
•
Data Query Execution
•
Translation of a BQL Command to SQL
•
Merge of the Records with PXCache
Additional Information: PXCache Objects
When a graph executes a data view, the graph creates the following objects:
•
The PXView object, which contains the BQL command that corresponds to the data view
•
The PXCache<DAC> objects whose type parameter is defined by the data access classes (DACs) that are
used in the BQL command
For more information about PXCache<DAC> objects, see PXView and PXCache of the Data View.


Part 3: Custom Tab (Stock Items Form) | 59
Part 3: Custom Tab (Stock Items Form)
In this part of the course, you will add a custom tab to the Stock Items (IN202500) form. If the selected item is a
repair item (that is, if the Repair Item check box is selected on the General tab of the form), this tab displays a list
of devices that can be serviced by using the item.
Aer you complete the lessons of this part, you will be able to test the ability of a user to add compatible devices for
a repair item.
Lesson 3.1: Adding a New Tab
In this lesson, you will learn how to add a new data view to the business logic of an Acumatica ERP form.
On the Stock Items (IN202500) form, you will create the Compatible Devices tab, which will contain a grid with the
compatible serviceable devices for each repair item. On this tab, managers of the Smart Fix company will select the
devices that can be serviced by using the item. You will place the new tab right of the other tabs on the form, as the
following screenshot shows.
Figure: The Compatible Devices tab
A form may contain controls for only data fields that are accessible through a data view. To provide access to data
fields for a container control on a form, you have to define a separate data view for the container in the graph. Each
data view should refer to a unique main data access class (DAC) unless you want to display the same data record in
multiple containers.
To provide data for the new tab item, you have to create a data view in the InventoryItemMaint graph that
provides the business logic for the Stock Items form.
Lesson Objectives
As you complete this lesson, you will learn how to do the following:
•
Add a custom data view for an Acumatica ERP form
•
Create a custom tab on an Acumatica ERP form
•
Conditionally hide a custom tab on an Acumatica ERP form


Part 3: Custom Tab (Stock Items Form) | 60
Step 3.1.1: Creating a DAC—Self-Guided Exercise
As you completed the Initial Configuration, you created the RSSVStockItemDevice database table. In this step,
you will create a data access class for this table. The ways to create a DAC are described in detail in Lesson 1.4:
Configure the Data Access Class and Step 2.1.2: Create a DAC in Visual Studio in the T200 Maintenance Forms training
course.
As you add the DAC, you perform the following general actions:
1. Create the RSSVStockItemDevice data access class and define its system fields. For more information
about definition of the system fields, see Audit Fields, Concurrent Update Control (TStamp), and Attachment of
Additional Objects to Data Records (NoteID) in the documentation.
2. In the Helper\Messages.cs file, in the Messages class, add the following constant for the user-
friendly DAC name.
        public const string RSSVStockItemDevice = "Device Compatible with Stock Item";
3. For the DAC, specify the following attribute.
    [PXCacheName(Messages.RSSVStockItemDevice)]
4. In the RSSVStockItemDevice DAC, define the attributes as follows:
•
Mark the InventoryID and DeviceID fields as the key fields.
•
Specify the display names for the fields as follows:
•
For the DeviceID field, Device
•
For the InventoryID field, no display name
•
Set up a master-detail relationship between the InventoryItem and RSSVStockItemDevice
DACs. You have learned how to define a master-detail relationship in Step 2.1.2: Defining the Master-Detail
Relationship Between Data (with PXParent and PXDBDefault).
•
Add the following PXSelector attribute to the DeviceID field.
        [PXSelector(
            typeof(RSSVDevice.deviceID),
            typeof(RSSVDevice.deviceCD),
            typeof(RSSVDevice.description),
            SubstituteKey = typeof(RSSVDevice.deviceCD),
            DescriptionField = typeof(RSSVDevice.description))]
The full code of the RSSVStockItemDevice DAC, which is the result of this step, is provided in the
Customization\T210\CodeSnippets\Step3.1.1 folder, which you have downloaded from
Acumatica GitHub.
Step 3.1.2: Creating a Data View
Now you will define the CompatibleDevices data view in the InventoryItemMaint graph.
Creating the Data View
To create the CompatibleDevices data view, perform the following actions:


Part 3: Custom Tab (Stock Items Form) | 61
1. In the InventoryItemMaint.cs file in Visual Studio, add the following using directives.
using PhoneRepairShop;
using PX.Data.BQL.Fluent;
2. In the InventoryItemMaint_Extension extension class, define the CompatibleDevices data
view as shown in the following code.
        #region Data Views
        public SelectFrom<RSSVStockItemDevice>.
            Where<RSSVStockItemDevice.inventoryID.
                IsEqual<InventoryItem.inventoryID.FromCurrent>>.View
            CompatibleDevices = null!;
        #endregion
In the data view declaration, the fluent BQL statement selects the devices that are compatible with the
current stock item.
3. Build the project.
Step 3.1.3: Creating the Tab Item, Grid, and Columns
Now you will customize the Stock Items (IN202500) form to have a new tab item that displays the compatible
devices of the selected stock item.
You will create the following interface elements on the Stock Items form:
•
The Compatible Devices tab
•
A grid on the tab
•
The following columns in the grid:
•
Device
•
Description
In the Device column, you will provide hyperlinks to the Serviced Devices (RS202000) form so that a user can review
the details of the device and edit them.
You will perform the following tasks, each of which is described in the sections below:
1. Defining the Grid View in TypeScript
2. Adjusting the HTML Template
3. Providing Hyperlinks in the Device Column
In this step, you will use the Customization Project Editor to add the interface elements; however, you
could perform the same tasks in VS Code.
1. Defining the Grid View in TypeScript
Define a grid view for the Compatible Devices tab as follows:
1. Open the IN202500_PhoneRepairShop_generated.ts file located in the development
folder. This file was already exported to the development folder alongside the RS203000.ts,
RS203000.html, and IN202500_PhoneRepairShop_generated.html files in Step 2.1.1 of Lesson
2.1 when you clicked Export to Development Folder on the Modern UI Files page.


Part 3: Custom Tab (Stock Items Form) | 62
Once you start working with the files that were generated from the Modern UI Editor, in the
development folder, you need to continue editing them only in the development folder
because they will be overwritten by the Modern UI Editor. Alternatively, you can create a new
extension and edit it in the development folder. The Customization Project Editor will merge
the extensions when you publish the project.
2. In the IN202500_PhoneRepairShop_generated.ts file, define a view class.
3. In the view class, define the DeviceID and DeviceID_description fields, as shown below. For the
DeviceID field, specify the commitChanges property.
@gridConfig({ preset: GridPreset.Details })
export class RSSVStockItemDevice extends PXView {
  DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;
  DeviceID_description: PXFieldState;
}
This is the Description field of the RSSVDevice DAC, which is available through
PXSelector assigned to the DeviceID field of the RSSVStockItemDevice DAC.
4. Initialize the CompatibleDevices view in the IN202500_PhoneRepairShop_generated class.
  @viewInfo({ containerName: "Compatible Devices" })
  CompatibleDevices = createCollection(RSSVStockItemDevice);
5. Update the Modern UI files in the customization project.
6. Publish the customization project to apply the changes in the extension file.
2. Adjusting the HTML Template
In the HTML template you need to add the Compatible Devices tab and the table inside it. Do the following in the
Modern UI Editor:
1. On the HTML tab, locate the definition of the Price/Cost tab.
2. Add the qp-tab tag for the Compatible Device tab aer the Price/Cost tab.
3. In the qp-tab tag, add the qp-grid tag.
The resulting code is shown below.
  <qp-tab
    after="#tab-PriceCost"
    id="tab-CompatibleDevices"
    caption="Compatible Devices"
  >
    <qp-grid id="grid-CompatibleDevices" view.bind="CompatibleDevices">
    </qp-grid>
  </qp-tab>
4. On the tab toolbar, click Preview HTML Extension.
In the Preview HTML Extension dialog box, you can see the qp-tab and qp-grid tags added aer the
field tag. The qp-tab contains the after attribute that indicates the location of the tab.
5. Click OK.
6. On the page toolbar, click Save.


Part 3: Custom Tab (Stock Items Form) | 63
The system updates the existing IN202500_PhoneRepairShop_generated.html
file on the Modern UI Files page. However, since this file was also previously
exported to the development folder, the version of this file in the development
folder is now outdated. Since you have already updated your customization
project with the changes made to the other files in the development folder in
the previous steps, you can, as an optional step, click Export to Development
Folder again to replace all the files in the development with those currently in
your customization project. Alternatively, you can manually copy the contents of
IN202500_PhoneRepairShop_generated.html on the Modern UI Files page and
paste them in the corresponding IN202500_PhoneRepairShop_generated.html file
in the development folder.
7. Publish the customization project.
8. Refresh the Stock Items (IN202500) form to view the content of the created tab, which is shown in the
following screenshot.
Figure: The final layout of the tab


Part 3: Custom Tab (Stock Items Form) | 64
Try to add a record to the tab. Notice that the value in the Device column is displayed as a link but does not
open any page. This happens because for all selector fields, links are enabled in the frontend by default.
However, you also need to configure the backend, which you will do in the next section.
3. Providing Hyperlinks in the Device Column
Acumatica Framework provides two ways to add redirection hyperlinks to a grid column. One way involves
defining an action that redirects the browser to the needed page. In this step, you will use another way, which
begins with adding the PXSelector attribute for the field used to create the column. You will also specify the
PXPrimaryGraph attribute for the graph that corresponds to the page that should be opened when a user click
the link.
The way you configure the link in frontend depends on where it is located: in a fieldset or in a grid. In a grid, all
selector fields have the link enabled by default with the hideViewLink = false property. For more detail, see
Selector Control: Configuration of a Link.
A selector control is bound to a DAC field containing the foreign key of another table. Therefore, the
control can be used to redirect a user to the form designed to edit the data record.
You provide hyperlinks in the Device column as follows:
1. In the TypeScript extension, make sure the DeviceID field has the CommitChanges property specified.
2. In Visual Studio, assign the PXPrimaryGraph attribute to the RSSVDevice DAC, as shown in the
following code.
    [PXPrimaryGraph(typeof(RSSVDeviceMaint))]
You use the PXPrimaryGraph attribute to specify the graph that corresponds to the default editing form
for records of the DAC.
3. Build the PhoneRepairShop_Code project.
4. Publish the customization project.
Related Links
•
Tab Item Container (PXTabItem)
•
Appendix C: Troubleshooting the Customization Project Editor
Step 3.1.4: Hiding the Tab from the Form (with RowSelected)
In this step, you will define the Compatible Devices tab of the Stock Items (IN202500) form to be hidden if the
Repair Item check box is cleared on the General tab of the form.
You will modify the RowSelected event handler, which you have added in Step 1.1.6: Making the Custom Field
Conditionally Available (with RowSelected).
Hiding the Tab from the Form
To conditionally hide the tab from the form, do the following:
1. In Visual Studio, in the InventoryItemMaint_Extension class, modify the RowSelected event
handler for the InventoryItem DAC as shown in the following code.
        protected void _(Events.RowSelected<InventoryItem> e)
        {
            if (e.Row == null) return;


Part 3: Custom Tab (Stock Items Form) | 65
            InventoryItem item = e.Row;
            InventoryItemExt itemExt = PXCache<InventoryItem>.
                GetExtension<InventoryItemExt>(item);
            bool enableFields = itemExt != null &&
                itemExt.UsrRepairItem == true;
            //Make the Repair Item Type box available
            //when the Repair Item check box is selected.
            PXUIFieldAttribute.SetEnabled<InventoryItemExt.usrRepairItemType>(
                e.Cache, e.Row, enableFields);
            //Display the Compatible Devices tab when the Repair Item check box 
            //is selected.
            CompatibleDevices.Cache.AllowSelect = enableFields;
        }
2. Build the project.
Related Links
•
Conditional Hiding of a Tab Item
Step 3.1.5: Using the New Tab
In this step, you will test the new Compatible Devices tab of the Stock Items (IN202500) form as follows:
1. On the Stock Items form, select the BAT3310 item, and make sure the Compatible Devices tab is displayed
for this item.
2. On the Compatible Devices tab, add the Nokia3310 device to the list.
3. Save your changes.
4. Create the SCRSGS4 item with the following settings:
•
Inventory ID: SCRSGS4
•
Description: Screen for Samsung Galaxy S4
•
Item Class (Item Defaults section of the General tab): STOCKITEM
•
Default Warehouse (Warehouse Defaults section of the General tab): MAIN
•
Default Price (Price Management section of the Price/Cost tab): 50
5. Make sure that the Compatible Devices tab is not displayed.
6. Also, make sure that in the Item Defaults section of the General tab, the Repair Item Type box is
unavailable for editing.
7. On the General tab, select the Repair Item check box, and select Screen in the Repair Item Type box.
8. On the Compatible Devices tab, add the Samsung Galaxy S4 device.
9. Save your changes.
Lesson Summary
In this lesson, you have practiced implementing extensions for both the business logic and the user interface of an
existing form, and you have learned the following tasks:
•
Creating a control container on a form


Part 3: Custom Tab (Stock Items Form) | 66
•
Creating the data view in the extension of the graph for the form to provide data for the container
•
Conditionally hiding a tab
During the lesson, you have added to the customization project the following items:
•
On the Stock Items (IN202500) form, the Compatible Devices tab, which contains the Device and
Description columns
•
The CompatibleDevices data view in the InventoryItemMaint_Extension graph
•
The RSSVStockItemDevice DAC
You have also modified the RowSelected<InventoryItem> event handler.


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 67
Part 4: Calculations and Insertion of a Default Record
(Services and Prices Form)
In this part, you will add the Labor and Warranty tabs to the Services and Prices (RS203000) form. You will also add
the business logic of these tabs: You will implement calculations of the fields on the Labor tab and in the Summary
area of the form; you will also add a default record to the Warranty tab.
Aer you complete the lessons of this part, you will be able to test the functionality of the Services and Prices form.
Lesson 4.1: Calculating Field Values
In this lesson, you will implement the calculation of the value in the Approximate Price box on the Services and
Prices (RS203000) form. This value will be calculated as a sum of the values in the Price column on the Repair
Items tab and the values of the Ext. Price column on the Labor tab, which you will add as a self-guided exercise. In
the Approximate Price box, a manager of the Smart Fix company can view the price of the selected service for the
selected device, which is an approximate price for the corresponding work order.
Description of Form Elements That Are Created in This Lesson
The Labor tab will display the non-stock item records, which represent work for the particular service and device
that are selected in the Summary area of the form. The tab will have the following columns:
•
Inventory ID: One of the non-stock items (which the user can select) defined on the Non-Stock Items
(IN202000) form
•
Description: The description of the non-stock item that has been selected in the Inventory ID column
•
Default Price: The price of the non-stock item
•
Quantity: The quantity of the included non-stock item
•
Ext. Price: The extended price, which the system calculates as the default price multiplied by the quantity
The following screenshot shows the form as it will look aer you complete this lesson.
Figure: The Labor tab
Relationships Between Database Tables
The repair prices for particular services and devices (RSSVRepairPrice) and the non-stock items
(InventoryItem) have a many-to-many relationship. The many-to-many links between records are stored in


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 68
the separate RSSVLabor custom table. The following diagram illustrates the relationships between the database
tables that are used in this lesson.
Lesson Objectives
You will learn how to use the PXFormula attribute for calculations.
Step 4.1.1: Adding the Labor Tab—Self-Guided Exercise
In this step, you will add the Labor tab of the Services and Prices (RS203000) form on your own. Although this is a
self-guided exercise, you can use the details and suggestions in this topic as you modify the form. You have learned
how to add a tab to a form in Lesson 3.1: Adding a New Tab.
DAC
You should define the RSSVLabor DAC, and set up its fields as follows:
•
In the Helper\Messages.cs file, in the Messages class, add the following constant for the user-
friendly DAC name.
        public const string RSSVLabor = "Repair Labor";
•
For the DAC, define the following attribute.
    [PXCacheName(Messages.RSSVLabor)]
•
For the system fields, define the attributes as described in Step 1.4.2: Configure the Attributes of the New DAC
in the T200 Maintenance Forms training course or in Audit Fields, Concurrent Update Control (TStamp), and
Attachment of Additional Objects to Data Records (NoteID) in the documentation.
•
For the InventoryID field, replace the existing attributes with the [Inventory(IsKey = true)]
attribute from the PX.Objects.IN namespace. (This selector displays only the inventory items for which
the current user has access rights and that do not have the Inactive or Marked for Deletion status.)


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 69
•
For the InventoryID field, add the PXRestrictor attribute to display only the inventory items that are
non-stock items (that is, only those for which InventoryItem.stkItem is equal to False). Make sure
that you have added the message specified in the restrictor to the localizable Messages class.
•
Mark the ServiceID and DeviceID fields as the key fields.
Unlike the RSSVRepairItem DAC, the RSSVLabor DAC does not contain additional
LineNbr key field because users cannot add multiple labor items with the same inventory ID,
service ID, and device ID on the Labor tab of the Services and Prices (RS203000) form.
•
Define a master-detail relationship between the RSSVRepairPrice DAC and the RSSVLabor DAC. You
have learned how to define a master-detail relationship in Step 2.1.2: Defining the Master-Detail Relationship
Between Data (with PXParent and PXDBDefault).
•
For the DefaultPrice field, add attributes, as shown in the following code.
        #region DefaultPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Default Price")]
        public virtual Decimal? DefaultPrice { get; set; }
        public abstract class defaultPrice :
            PX.Data.BQL.BqlDecimal.Field<defaultPrice>
        { }
        #endregion
•
For the Quantity field, add attributes, as shown in the following code.
        #region Quantity
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Quantity")]
        public virtual Decimal? Quantity { get; set; }
        public abstract class quantity : PX.Data.BQL.BqlDecimal.Field<quantity> { }
        #endregion
•
For the ExtPrice field, add attributes, as shown in the following code.
        #region ExtPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Ext. Price", Enabled = false)]
        public virtual Decimal? ExtPrice { get; set; }
        public abstract class extPrice : PX.Data.BQL.BqlDecimal.Field<extPrice> { }
        #endregion
You can see the full example of the RSSVLabor DAC in the CodeSnippets folder for this course,
which is available on Acumatica GitHub.
Graph
You need to define the data view for the Labor tab of the Services and Prices (RS203000) form, as shown in the
following code.
        public SelectFrom<RSSVLabor>.
            Where<RSSVLabor.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVLabor.serviceID.


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 70
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.View
            Labor = null!;
As with the RepairItems data view, the Labor detail data view must be defined aer the
RepairPrices master data view.
Definition of Controls in the Modern UI
When you define controls for the Labor tab of the Services and Prices (RS203000) form, you can use the following
tips:
•
In TypeScript, define a view class named RSSVLabor.
In the gridConfig decorator, specify the same properties as for the grid on the Repair Items tab.
•
In the view class, add properties for the following fields: InventoryID, InventoryID_description,
DefaultPrice, Quantity, ExtPrice.
•
In the view class, specify PXFieldOptions.CommitChanges for the InventoryID field.
•
In TypeScript, initialize the view for the grid by calling the createCollection method.
•
In the HTML template, add a new qp-tab control with a qp-grid control in it.
We recommend exporting the Modern UI files for the Services and Prices form to the development
folder and continue to work there to avoid any functionality that is not yet fully supported in the
Modern UI Editor. You can find the resulting files among other code snippets for the step.
Step 4.1.2: Calculating Field Values (with PXFormula)
You will now use the PXFormula attribute to implement calculations on the Services and Prices (RS203000) form.
Because you are using the PXFormula attribute, you do not need to define any event handlers to implement these
calculations.
In the PXFormula attribute, you can define conditional calculation of the field value. For details
about conditional calculation, see Calculation of Field Values in the documentation.
Calculating Field Values
Do the following to implement the calculation of the field values:
1. Add the PXFormula attribute to the BasePrice field of the RSSVRepairItem DAC as shown in the
following code.
        #region BasePrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Price")]
        [PXFormula(null,
            typeof(SumCalc<RSSVRepairPrice.price>))]
        public virtual Decimal? BasePrice { get; set; }
        public abstract class basePrice : 
            PX.Data.BQL.BqlDecimal.Field<basePrice> { }
        #endregion


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 71
In this code, the PXFormula attribute calculates the Price value of the parent document as the sum of
the BasePrice values of all detail records.
2. Add the PXFormula attribute to the ExtPrice field of the RSSVLabor DAC as shown in the following
code.
        #region ExtPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Ext. Price", Enabled = false)]
        [PXFormula(
            typeof(RSSVLabor.quantity.Multiply<RSSVLabor.defaultPrice>),
            typeof(SumCalc<RSSVRepairPrice.price>))]
        public virtual Decimal? ExtPrice { get; set; }
        public abstract class extPrice : PX.Data.BQL.BqlDecimal.Field<extPrice> { }
        #endregion
In this code, the PXFormula attribute calculates the ExtPrice value as the product of the Quantity
and DefaultPrice values of the same record, and calculates the Price value of the parent document as
the sum of ExtPrice values of all detail records.
3. Build the project.
4. In the RS203000.ts file, specify PXFieldOptions.CommitChanges for the following fields (see the
code below):
•
In the RSSVRepairItem class, the BasePrice field
•
In the RSSVLabor class, the DefaultPrice and Quantity fields
export class RSSVRepairItem extends PXView {
    RepairItemType : PXFieldState<PXFieldOptions.CommitChanges>;
 Required : PXFieldState<PXFieldOptions.CommitChanges>;
 InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
 InventoryID_description : PXFieldState;
 BasePrice : PXFieldState<PXFieldOptions.CommitChanges>;
 IsDefault : PXFieldState<PXFieldOptions.CommitChanges>;
}
export class RSSVLabor extends PXView  {
 InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
 InventoryID_description : PXFieldState;
 DefaultPrice : PXFieldState<PXFieldOptions.CommitChanges>;
 Quantity : PXFieldState<PXFieldOptions.CommitChanges>;
 ExtPrice : PXFieldState;
}
5. Update the Modern UI files in the customization project and publish it.
Testing the Calculations
Now you will test the calculations you have implemented. Open the Services and Prices (RS203000) form, and
remove all records on the form because they have incorrect price values saved in the database.
Test the calculations as follows:
1. Add a record for the Battery Replacement service and the Nokia 3310 device.
2. On the Repair Items tab, add new lines with the following values.


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 72
UI Element
First Line
Second Line
Repair Item Type
Battery
Back Cover
Required
Selected
Cleared
Inventory ID
BAT3310
BCOV3310
Default
Selected
Selected
Once you have added the line, notice that the Approximate Price value is calculated in the Summary area.
3. On the Labor tab, add a row and specify the following settings:
•
Inventory ID: CONSULT
•
Default Price: 5
•
Quantity: 1
Once you have finished editing the row, notice that the Ext. Price value has been calculated for the line. Also
notice that the calculated value has been added to the Approximate Price value in the Summary area, as
shown in the following screenshot.
Figure: The calculation of the price
4. Save your changes.
5. Add another record for the Liquid Damage service and the Nokia 3310 device.
6. On the Repair Items tab, add new lines with the following values.
UI Element
First Line
Second Line
Repair Item Type
Battery
Battery
Required
Cleared
Cleared
Inventory ID
BAT3310
BAT3310EX
Default
Selected
Cleared
7. Save your changes. Make sure the value of Approximate Price in the Summary area is 50.
Related Links
•
PXFormulaAttribute Class


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 73
•
Calculation of Field Values
Lesson Summary
In this lesson, you have implemented the calculation of the value of the Approximate Price box on the Services
and Prices (RS203000) form. You have used the PXFormula attribute to configure the calculation.
Lesson 4.2: Inserting a Default Detail Record
In this lesson, you will add the Warranty tab to the Services and Prices (RS203000) form. This tab will display the
list of warranties (which are contract templates in Acumatica ERP) for the particular service and device selected in
the Summary area of the form. On this tab, the default warranty item is added each time a new service–device pair
is defined on the form.
Description of Form Elements That Are Created in This Lesson
The Warranty tab has the Contract ID column, which contains the ID of the contract template. The tab also has the
Description, Duration, Duration Unit, and Contract Type columns, which the system fills in with the read-only
values of the corresponding fields of the selected contract template.
The Warranty tab with the default warranty item is shown in the following screenshot.
Figure: The Warranty tab
Relationships Between Database Tables
The repair prices for particular services and devices (RSSVRepairPrice) and the contract templates
(CT.ContractTemplate, which are stored in the Contract database table) have a many-to-many
relationship. The many-to-many links between records are stored in the separate RSSVWarranty custom table.
The following diagram illustrates the relationships between the database tables that are used in this lesson.


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 74
Lesson Objectives
In this lesson, you will learn how to add a default detail record to the grid.
Step 4.2.1: Adding the Warranty Tab—Self-Guided Exercise
In this step, you will add the Warranty tab of the Services and Prices (RS203000) form on your own. Although this
is a self-guided exercise, you can use the details and suggestions in this topic as you create the form. You have
learned how to add a tab to a form in Lesson 3.1: Adding a New Tab.
DAC
You should define the RSSVWarranty DAC, and set up its fields and attributes as follows:
•
In the Helper\Messages.cs file, in the Messages class, add the following constant for the user-
friendly DAC name.
        public const string RSSVWarranty = "Warranty";
•
For the DAC, define the following attribute.
    [PXCacheName(Messages.RSSVWarranty)]
•
For the system fields, define the attributes as described in Step 1.4.2: Configure the Attributes of the New DAC
in the T200 Maintenance Forms training course or in Audit Fields, Concurrent Update Control (TStamp), and
Attachment of Additional Objects to Data Records (NoteID) in the documentation.
•
Mark the ServiceID, DeviceID, and ContractID fields as the key fields.
•
Configure a master-detail relationship between the RSSVRepairPrice DAC and the RSSVWarranty
DAC.
•
For the ContractID field, use the PXDimensionSelector attribute as shown in the following code.
        [PXDimensionSelector(ContractTemplateAttribute.DimensionName,
            typeof(Search<ContractTemplate.contractID,
                Where<ContractTemplate.baseType.IsEqual<


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 75
                    CTPRType.contractTemplate>>>),
                typeof(ContractTemplate.contractCD),
            DescriptionField = typeof(ContractTemplate.description))]
Add the PX.Objects.CT using directive to the file that contains the definition of the
RSSVWarranty DAC to use the ContractTemplateAttribute, ContractTemplate,
and CTPRType classes.
•
For the DefaultWarranty field, use the attributes shown in the following code. Because you will use
the PXDefault attribute with a constant as an argument specifying the default value for the field and the
PersistingCheck property set to PXPersistingCheck.Nothing, the data field will have a default
value and will not be required to be entered.
        #region DefaultWarranty
        [PXDBBool()]
        [PXUIField(DisplayName = "Default Warranty")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? DefaultWarranty { get; set; }
        public abstract class defaultWarranty :
            PX.Data.BQL.BqlBool.Field<defaultWarranty>
        { }
        #endregion
•
Add the following unbound fields and add PXFormula attribute to them as the following code shows. This
way you will be able to display values from the ContractTemplate DAC without joining it in a data view.
        #region ContractDuration
        [PXInt(MinValue = 1, MaxValue = 1000)]
        [PXUIField(DisplayName = "Duration", Enabled = false)]
        [PXFormula(typeof(
            ContractTemplate.duration.FromSelectorOf<RSSVWarranty.contractID>))]
        public virtual int? ContractDuration { get; set; }
        public abstract class contractDuration :
            PX.Data.BQL.BqlInt.Field<contractDuration>
        { }
        #endregion
        #region ContractDurationType
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Duration Unit", Enabled = false)]
        [Contract.durationType.List]
        [PXFormula(typeof(
            ContractTemplate.durationType.FromSelectorOf<RSSVWarranty.contractID>))]
        public virtual string? ContractDurationType { get; set; }
        public abstract class contractDurationType :
            PX.Data.BQL.BqlString.Field<contractDurationType>
        { }
        #endregion
        #region ContractType
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Contract Type", Enabled = false)]
        [Contract.type.List]
        [PXFormula(typeof(
            ContractTemplate.type.FromSelectorOf<RSSVWarranty.contractID>))]
        public virtual string? ContractType { get; set; }
        public abstract class contractType :


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 76
            PX.Data.BQL.BqlString.Field<contractType>
        { }
        #endregion
You can see the full example of the RSSVWarranty DAC in the CodeSnippets folder for this
course, which is available on Acumatica GitHub.
Graph
You need to define the data view for the Warranty tab of the Services and Prices (RS203000) form as follows:
        public SelectFrom<RSSVWarranty>.
            Where<RSSVWarranty.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVWarranty.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.
            OrderBy<RSSVWarranty.defaultWarranty.Desc>.View
            Warranty = null!;
Definitions of Controls in the Modern UI
When you define controls for the Warranty tab of the Services and Prices (RS203000) form, you can use the
following tips:
•
In TypeScript, define a view class named RSSVWarranty.
In the gridConfig decorator, specify the same properties as for the grid on the Repair Items tab.
•
In the view class, add properties for the following fields: ContractID, ContractID_description,
ContractDuration, ContractDurationType, ContractType.
•
In the view class, specify PXFieldOptions.CommitChanges for the ContractID field.
•
In TypeScript, initialize the view for the grid by calling the createCollection method.
•
In the HTML template, add a new qp-tab control with a qp-grid control in it.
Step 4.2.2: Inserting a Default Detail Record (with RowInserted)
In this step, you will implement the automatic addition of a warranty, which the system adds by default with every
new service–device pair on the Services and Prices (RS203000) form. A default warranty is a contract template
with the ContractCD field equal to DFLTWARRNT, which is preconfigured for this training course. This contract
template is added as a RSSVWarranty record, which is a detail record for a RSSVRepairPrice record, aer a
price is successfully inserted into the cache object.
Insertion of a Record
You will insert a new record into the cache of RSSVRepairPrice records in the RowInserted event handler.
The Warranty data view will select the detail records of the new service–device pair, because it is already the
pair referenced by the Current property of the RSSVRepairPrice cache. The Current property is set to the
inserted record right before the RowInserted event. Therefore, you can use the Warranty data view to check
that it does not contain any records before insertion of the default record.
You will keep the original value of the IsDirty property of the RSSVWarranty cache to hide the fact that the
handler performs modifications in the cache. Because of this setting, if a user adds a new service–device pair


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 77
and tries to leave the form before entering any data, no confirmation will be requested from the user. For more
information about insertion of data records, see Insertion of a Data Record.
Instructions for Inserting a Default Detail Record
Do the following to insert the default detail data record:
1. In the RSSVRepairPriceMaint graph, add the fluent BQL constant that defines the string with the ID of
the contract with the default warranty.
        #region Constant
        //The fluent BQL constant for the free warranty that is inserted by default
        public const string DefaultWarrantyConstant = "DFLTWARRNT";
        public class defaultWarranty : PX.Data.BQL.BqlString.Constant<defaultWarranty>
        {
            public defaultWarranty()
                : base(DefaultWarrantyConstant)
            {
            }
        }
        #endregion
2. Add the following using directive.
using PX.Objects.CT;
3. Add the data view for the default warranty, as the following code shows.
        //The view for the default warranty
        public SelectFrom<ContractTemplate>.
            Where<ContractTemplate.contractCD.IsEqual<defaultWarranty>>.
            View DefaultWarranty = null!;
4. Add the following RowInserted event handler to the RSSVRepairPriceMaint graph.
        //Insert the default detail record.
        protected virtual void _(Events.RowInserted<RSSVRepairPrice> e)
        {
            if (Warranty.Select().Count == 0)
            {
                bool oldDirty = Warranty.Cache.IsDirty;
                // Retrieve the default warranty.
                Contract defaultWarranty = (Contract)DefaultWarranty.Select();
                if (defaultWarranty != null)
                {
                    RSSVWarranty line = new RSSVWarranty();
                    line.ContractID = defaultWarranty.ContractID;
                    line.DefaultWarranty = true;
                    // Insert the data record into
                    // the cache of the Warranty data view.
                    Warranty.Insert(line);
                    // Clear the flag that indicates in the UI whether the cache
                    // contains changes.
                    Warranty.Cache.IsDirty = oldDirty;
                }
            }
        }


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 78
In this code, you ensure that there are no RSSVWarranty records for the new RSSVRepairPrice
record, and you insert a new RSSVWarranty record with the default warranty. You retrieve the default
warranty through the DefaultWaranty data view and use it to set the fields of the new warranty line.
5. Build the project.
6. Update the files in the customization project and publish it.
Instructions for Testing the Logic
To test the logic you have implemented, open the Services and Prices (RS203000) form, and do the following:
1. Create a record, and in the Summary area, specify the Screen Repair service and the Samsung Galaxy S4
device.
2. On the Warranty tab, ensure that the default warranty has been added automatically for the new service–
device pair.
3. Navigate away from the page without saving the new pair. No dialog box should appear asking you to
confirm that you want to leave the page.
Related Links
•
Insertion of a Data Record
•
Cancellation of Attribute Event Handlers
•
PXCache Class
•
Constants in Fluent BQL
Step 4.2.3: Adding UI Representation Logic (with RowSelected and RowDeleting)
You will now add UI presentation logic for RSSVWarranty detail data records.
You will use the RowDeleting event handler to prevent removal of the default line. In this event handler, you will
throw PXException if a user tries to delete the default line. The exception message will not be displayed if the
line is deleted along with the parent item.
You will use the RowSelected event handler to make the default line unavailable for editing. In this event
handler, you will invoke the SetEnabled() method, which doesn't specify the field, so the method makes
unavailable or available for editing all fields for the given data record. Here you will make unavailable for editing all
fields of the warranty line corresponding to the default warranty; you will make available for editing all fields for all
other warranty lines.
Preventing the Removal of the Default Line
To prevent a user from deleting the default line on the Services and Prices (RS203000) form, do the following:
1. In the Helper\Messages.cs file, in the Messages class, add the following constant for the error
message.
        public const string DefaultWarrantyCanNotBeDeleted =
            "The default warranty cannot be deleted.";
2. Implement the RowDeleting event handler of the RSSVWarranty DAC in the
RSSVRepairPriceMaint graph as the following code shows.
        //Prevent removal of the default warranty.
        protected virtual void _(Events.RowDeleting<RSSVWarranty> e)
        {
            if (e.Row.DefaultWarranty != true) return;


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 79
            if (e.ExternalCall && RepairPrices.Current != null &&
                RepairPrices.Cache.GetStatus(RepairPrices.Current) !=
                PXEntryStatus.Deleted)
            {
                throw new PXException(Messages.DefaultWarrantyCanNotBeDeleted);
            }
        }
Making the Default Line Unavailable for Editing
To make the default line unavailable for editing, do the following:
1. Add the following RowSelected event handler of the RSSVWarranty DAC to the
RSSVRepairPriceMaint graph.
        //Make the default warranty unavailable for editing.
        protected virtual void _(Events.RowSelected<RSSVWarranty> e)
        {
            if (e.Row == null) return;
            RSSVWarranty line = e.Row;
            PXUIFieldAttribute.SetEnabled<RSSVWarranty.contractID>(e.Cache,
                line, line.DefaultWarranty != true);
        }
2. Build the project.
3. Publish the customization project to include the latest changes in the project.
Testing the Logic
To make sure that the event handlers work as expected, refresh the Services and Prices (RS203000) form, and do
the following:
1. In the Summary area, create a record for the Screen Repair service and the Samsung Galaxy S4 device.
2. On the Warranty tab, notice that all columns in the line for the default warranty are disabled: You can't edit
or copy a value in any column.
3. Click the row with the default warranty and click Delete Row on the table toolbar. The error is displayed for
the row as shown in the following screenshot.
Figure: The error on the form


Part 4: Calculations and Insertion of a Default Record (Services and Prices Form) | 80
4. On the Repair Items tab, add a row with the following settings:
•
Repair Item Type: Screen
•
Required: Selected
•
Inventory ID: SCRSGS4
5. Save the changes.
Lesson Summary
In this lesson, you have learned how to add a default detail record to the grid. To add a default record, you have
used the RowInserted event handler for the parent DAC. You have also defined the UI presentation of the default
detail record in the RowSelected and RowDeleting event handlers for the child DAC.


Additional Information | 81
Additional Information
In the following topics, you can find additional information related to the initial configuration of an instance that
is needed to complete the activities of this course. You can also find a list of the scenarios in which particular event
handlers have been used in this course.
Appendix A: Initial Configuration
If for some reason you cannot complete the instructions in Step 2: Preparing the Needed Acumatica ERP Instance
for the Training Course in Initial Configuration, you can create an Acumatica ERP instance and manually publish the
needed customization project as described in this topic.
Step 1: Deploying the Needed Acumatica ERP Instance for the Training Course
You deploy an Acumatica ERP instance and configure it as follows:
1. To deploy a new application instance, open the Acumatica ERP Configuration wizard, and do the following:
a. On the Database Configuration page, type the name of the database: SmartFix_T210.
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
Load the customization project with the results of the T200 Maintenance Forms training course and publish this
project as follows:


Additional Information | 82
1. On the Customization Projects (SM204505) form, create a project with the name PhoneRepairShop and open
it.
2. On the menu of the Customization Project Editor, click Source Control > Open Project from Folder.
3. In the dialog box that opens, specify the path to the Customization\T200\PhoneRepairShop folder,
which you have downloaded from Acumatica GitHub, and click OK.
4. Bind the customization project to the source code of the extension library as follows:
a. Copy the Customization\T200\PhoneRepairShop_Code folder to the App_Data\Projects
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
b. Open the solution and build the PhoneRepairShop_Code project.
c. Reload the Customization Project Editor.
d. In the menu of the Customization Project Editor, click Extension Library > Bind to Existing.
e. In the dialog box that opens, specify the path to the App_Data\Projects
\PhoneRepairShop_Code folder, and click OK.
5. On the menu of the Customization Project Editor, click Publish > Publish Current Project.
The Modified Files Detected dialog box opens before publication because you have rebuilt
the extension library in the PhoneRepairShop_Code Visual Studio project. The Bin
\PhoneRepairShop_Code.dll file has been modified and you need to update it in the
project before the publication.
The published customization project contains all changes to Acumatica ERP website and database that have been
performed during the completion of the T200 Maintenance Forms training course. This project also contains the
customization plug-in that fills in the tables created in the T200 Maintenance Forms training course with the custom
data entered in this training course. For details about the customization plug-ins, see To Add a Customization Plug-
In to a Project. The creation of customization plug-ins is outside of the scope of this course.
Appendix B: Use of Event Handlers
This topic lists the scenarios in which particular event handlers have been used in this course.
Table: Use of Event Handlers
Event
Scenario
Examples in the Guide
FieldUpdated
Update of the fields of a data record on up-
date of a field of this record
Step 2.2.2: Updating Fields of the Same
Record on Update of a Field (with FieldUp-
dated and FieldDefaulting)


Additional Information | 83
Event
Scenario
Examples in the Guide
FieldDefault-
ing
Setting of a default value of a field that de-
pends on other field values
Step 2.2.2: Updating Fields of the Same
Record on Update of a Field (with FieldUp-
dated and FieldDefaulting)
RowDeleting
Prevention of the removal of a data record
at runtime
Step 4.2.3: Adding UI Representation Logic
(with RowSelected and RowDeleting)
RowInserted
Insertion of a detail record
Step 4.2.2: Inserting a Default Detail Record
(with RowInserted)
RowSelected
Configuration of the UI logic:
•
Making a box or tab available or un-
available at runtime
•
Making a row unavailable for editing
•
Step 1.1.6: Making the Custom Field Con-
ditionally Available (with RowSelected)
•
Step 3.1.4: Hiding the Tab from the Form
(with RowSelected)
•
Step 4.2.3: Adding UI Representation
Logic (with RowSelected and RowDelet-
ing)
RowUpdated
Update of the field of a data record on up-
date of a field of another record
Step 2.2.3: Updating a Field of Another
Record on Update of a Field (with RowUp-
dated)
Appendix C: Troubleshooting the Customization Project Editor
If you encounter any errors in the Customization Project Editor that are not related to the actual content of the
course, you can try the following approaches to resolve the issue:
•
Close the editor and open it again on the Customization Projects (SM204505) form
•
Reset caches on the Apply Updates (SM203510) form
•
If publishing freezes for a long time while compiling frontend files, close the window and start the
publication again. Check if the changes are already applied.
•
To avoid publishing project every time you make changes in frontend files, you can
Tips on working in the Modern UI Editor:
1. Always save your changes made on the tabs or dialog boxes by clicking Save on the page toolbar.
2. Review the generated code in files on the Modern UI Files page.
3. If the Modern UI Editor does not yet support the needed functionality, switch to working in the
development folder. Aer working in the development folder, do not forget to update the files in the
customization project. If you continue your work in the Modern UI Editor, the changes in the development
folder will be overwritten.


Appendix C: Troubleshooting the Customization Project Editor | 84
Appendix C: Troubleshooting the Customization Project
Editor
If you encounter any errors in the Customization Project Editor that are not related to the actual content of the
course, you can try the following approaches to resolve the issue:
•
Close the editor and open it again on the Customization Projects (SM204505) form
•
Reset caches on the Apply Updates (SM203510) form
•
If publishing freezes for a long time while compiling frontend files, close the window and start the
publication again. Check if the changes are already applied.
•
To avoid publishing project every time you make changes in frontend files, you can
Tips on working in the Modern UI Editor:
1. Always save your changes made on the tabs or dialog boxes by clicking Save on the page toolbar.
2. Review the generated code in files on the Modern UI Files page.
3. If the Modern UI Editor does not yet support the needed functionality, switch to working in the
development folder. Aer working in the development folder, do not forget to update the files in the
customization project. If you continue your work in the Modern UI Editor, the changes in the development
folder will be overwritten.
