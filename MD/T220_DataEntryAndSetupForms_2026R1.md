Developer Course
Customization
T220 Data Entry and Setup Forms
2026 R1
Revision: 3/19/2026


Contents | 2
Contents
Copyright...............................................................................................................................................4
How to Use This Course.......................................................................................................................... 5
Company Story and Customization Description........................................................................................ 7
Initial Configuration............................................................................................................................... 9
Step 1: Preparing the Environment.................................................................................................................. 9
Step 2: Preparing an Acumatica ERP Instance for the Training Course.......................................................... 9
Step 3: Creating the Database Tables.............................................................................................................10
Part 1: Data Entry Form (Repair Work Orders).........................................................................................12
Lesson 1.1: Define the Layout of a Data Entry Form..................................................................................... 12
Step 1.1.1: Creating the Form (Self-Guided Exercise)...........................................................................14
Step 1.1.2: Configuring the Controls of the Summary Area................................................................. 19
Data Entry Form: General Information..................................................................................................25
Data Entry Form: Definition in TypeScript and HTML.......................................................................... 29
Step 1.1.3: Creating the Form’s UI......................................................................................................... 32
Step 1.1.4: Adding the Generic Inquiry Form and Filter to the Project................................................38
Additional Information: Configuration of Controls.............................................................................. 39
Lesson 1.2: Copying Field Values from One Record to Another.................................................................... 39
Step 1.2.1: Adding Default Rows to Work Orders (with RowUpdated).................................................40
Step 1.2.2: Updating Fields of a Record on Update of Its Field (with FieldUpdated and FieldDefaulting)
—Self-Guided Exercise............................................................................................................................43
Lesson 1.3: Validating the Field Values.......................................................................................................... 43
Step 1.3.1: Validating an Independent Field Value (with FieldVerifying).............................................44
Step 1.3.2: Validating Dependent Fields of Records (with RowUpdating)...........................................47
Part 2: Setup Form (Repair Work Order Preferences)...............................................................................50
Lesson 2.1: Configuring the Auto-Numbering of a Field Value..................................................................... 50
Step 2.1.1: Creating the Form (Self-Guided Exercise)...........................................................................51
UI of a Setup Form: General Information..............................................................................................53
UI of a Setup Form: A Form with Only a Summary Area...................................................................... 55
Step 2.1.2: Creating the Form’s UI......................................................................................................... 58
Step 2.1.3: Configuring the DAC for the Setup Form (with PXPrimaryGraph and PXCacheName).....60
Step 2.1.4: Configuring the Auto-Numbering of Records (with CS.AutoNumberAttribute).................61
Additional Information: Custom Feature Switches.............................................................................. 65
Additional Information..........................................................................................................................66
Appendix A: Initial Configuration....................................................................................................................66


Contents | 3
Appendix B: Use of Event Handlers................................................................................................................67


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
Last Updated: 03/19/2026


How to Use This Course | 5
How to Use This Course
The T220 Data Entry and Setup Forms training course shows how to create data entry and setup forms by using
Acumatica Framework and the customization tools of Acumatica ERP. The course describes in detail how to define
the layout of a data entry form and implement the business logic of the form (such as insertion of data from a
template and validation of a field value). The course also shows how to provide configuration parameters for a data
entry form by using a setup form.
This course is intended for application developers who are starting to learn how to customize Acumatica ERP.
The course is based on a set of examples that demonstrate the general approach to customizing Acumatica ERP.
It’s designed to give you ideas about how to develop your own embedded applications by using the customization
tools. As you go through the course, you’ll continue the development of the customization for the cell phone repair
shop. You performed this customization in the T200 Maintenance Forms, and T210 Customized Forms and Master-
Details Relationships training courses—all of which we recommend that you take before completing the current
course.
Aer you complete all the lessons of the course, you will be familiar with the programming techniques for defining
even complex layouts of Acumatica ERP forms, the implementation of particular business logic scenarios, and the
configuration of setup parameters of Acumatica ERP forms.
We recommend that you complete the examples in the presented order because some examples use
the results of previous ones.
What the Course Prerequisites Are
To complete this course, you should be familiar with the basic concepts of the Acumatica Framework and the
Acumatica Customization Platform. We recommend that you complete the T200 Maintenance Forms and T210
Customized Forms and Master-Details Relationships training courses before you begin this course.
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


How to Use This Course | 6
•
The following experience with IIS:
•
Configuring and deploying ASP.NET websites
•
Configuring and securing IIS
What Is in a Part
The first part of the course explains how to create a data entry form, configure its layout, and implement basic
business logic scenarios.
The second part of the course shows how to create a setup form and configure the automatic numbering of data
records on the data entry form.
Each part of the course consists of lessons you should complete.
What Is in a Lesson
Each lesson focuses on a particular development scenario that you can implement by using Acumatica ERP
customization tools and the Acumatica Framework. Each lesson consists of a brief description of the scenario and
an example of its implementation.
The lesson may also include Additional Information topics, which are outside of the scope of this course but may be
useful to some readers.
Where the Source Code Is
You can find the source code of the customization described in this course and code snippets for the course in the
Customization\T220 folder of the Help-and-Training-Examples repository in Acumatica GitHub.
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
In this course, you will continue the development to support the cell phone repair shop of the Smart Fix company;
you began this development while completing the T200 Maintenance Forms and T210 Customized Forms and Master-
Details Relationships training courses.
If you haven’t completed these training courses, you will load and publish the customization project
with the results of these courses in Initial Configuration.
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
In this course, you will create the Repair Work Orders (RS301000) data entry form, which is used to create and
manage work orders for repairs. You will also create the Repair Work Order Preferences (RS101000) setup form,
which an administrative user will use to specify the company's preferences for the repair work orders.
Repair Work Orders Form
The following screenshot shows how the Repair Work Orders (RS301000) form will look.
Figure: Repair Work Orders form
The form will contain the following tabs:
•
Repair Items: Will show the list of repair items (stock items) necessary to complete the repair work order
•
Labor: Will contain the list of labor items (non-stock items) that are performed for the selected repair work
order
You’ll also import a substitute form of the inquiry type with a preconfigured filter of records; this substitute form
will serve as an entry point to the Repair Work Orders form.
The Repair Work Orders form will use the following custom tables, which you will add to the application database
in Initial Configuration:


Company Story and Customization Description | 8
•
RSSVWorkOrder: This table’s data will be displayed in the Summary area of the form.
•
RSSVWorkOrderItem: The data of this table will be shown on the Repair Items tab.
•
RSSVWorkOrderLabor: This table’s data will be displayed on the Labor tab.
Repair Work Order Preferences Form
Below you can see how the Repair Work Order Preferences (RS101000) form will look when you have developed it.
Figure: Repair Work Order Preferences form
The form will contain the following boxes:
•
Numbering Sequence: The numbering sequence that is used to auto-number repair work orders
•
Walk-In Customer: The identifier of the customer record that should be inserted for walk-in orders
•
Default Employee: The default assignee for repair work orders
•
Prepayment Percent: The percent of prepayment a customer should pay for a service that requires
prepayment—that is, a service that has the Requires Prepayment check box selected on the Repair
Services (RS201000) form
This form will use the RSSVSetup custom table, which you will add to the application database in Initial
Configuration.


Initial Configuration | 9
Initial Configuration
You need to perform prerequisite actions before you start to complete the course.
If you’ve deployed an instance for the T210 Customized Forms and Master-Details Relationships course and have the
customization project and the source code for this course, you need to perform only Step 3.
Step 1: Preparing the Environment
If you’ve completed the T210 Customized Forms and Master-Details Relationships training course and
are using the same environment for the current course, you can skip this step.
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
Step 2: Preparing an Acumatica ERP Instance for the Training Course
If you have completed the T210 Customized Forms and Master-Details Relationships training course,
instead of deploying a new instance, you can use the Acumatica ERP instance that you deployed and
used for that training course.
You deploy an Acumatica ERP instance and configure it as follows:
1. Open the Acumatica ERP Configuration wizard and do the following:
a. Click Deploy a New Acumatica ERP Instance for T-Series Developer Courses.
b. On the Instance Configuration page, do the following:
a. In the Training Course box, select T220 Data Entry and Setup Forms.
b. In the Local Path to the Instance box, select a folder that’s outside of the C:\Program Files
(x86), C:\Program Files, and C:\Users folders. (We recommend that you store the website
folder outside of these folders to avoid an issue with permission to work in these folders when you
customize the website.)


Initial Configuration | 10
c. On the Database Configuration page, make sure the name of the database is SmartFix_T220.
The system creates a new Acumatica ERP instance, adds a new tenant, loads the data to it, and publishes
the customization project that’s needed for activities of this training course.
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
If for some reason you cannot complete the instructions in this step, you can create an Acumatica ERP
instance and manually publish the needed customization project, as described in Appendix A: Initial
Configuration.
Step 3: Creating the Database Tables
Create the database tables that are necessary for the T220 Data Entry and Setup Forms training course and include
the scripts in the customization project as follows:
1. In SQL Server Management Studio, execute the T220_DatabaseTables.sql script to create the
database tables you need for the T220 Data Entry and Setup Forms training course.
The script creates the following tables, which are new for this course: RSSVWorkOrder,
RSSVWorkOrderItem, RSSVWorkOrderLabor, and RSSVSetup.
•
You can find the script in the Customization\T220\SourceFiles\DBScripts
folder, which you have downloaded from Acumatica GitHub.
•
If you use the Acumatica ERP instance you’ve deployed for the T210 Customized Forms and
Master-Details Relationships training course, before executing the script, change the name
of the database in the script. You should change it from SmartFix_T220 to the name of
the database used in your instance.
•
The design of database tables is outside of the scope of this course. For details on designing
database tables for Acumatica ERP, see Designing the Database Structure and DACs.
2. On the Database Scripts page of the Customization Project Editor, for each added table, do the following:
a. On the page toolbar, click Add Custom Table Schema.
b. In the dialog box that opens, select the table and click OK.
3. Publish the project.


Initial Configuration | 11
During the first publication of a customization project that contains any Modern UI files, which is
the case for the customization project of this training course, the system installs the dependencies
needed to build the Modern UI sources.


Part 1: Data Entry Form (Repair Work Orders) | 12
Part 1: Data Entry Form (Repair Work Orders)
The Smart Fix company needs a custom Acumatica ERP form on which users will create repair work orders. For
this purpose, in this part of the course, you’ll create the Repair Work Orders (RS301000) data entry form, which is
described in Company Story and Customization Description.
Data entry forms are the most frequently used forms of Acumatica ERP. Typically, these forms are used for the
input of business documents and records, such as sales orders and cases.
These forms have IDs that start with a two-letter abbreviation (indicating the functional area of the form) followed
by 30 and then four additional digits. For example, RS301000 will be the ID of the Repair Work Orders form.
The names of the graphs that work with data entry forms have the Entry suﬀix. For instance,
RSSVWorkOrderEntry will be the name of the graph for the Repair Work Orders form.
For details about the naming conventions for the forms and graphs, see Form and Report Numbering and Graph
Naming.
Lesson 1.1: Define the Layout of a Data Entry Form
In this lesson, you will learn how to define the layout of controls on a data entry form through the creation of the
Repair Work Orders (RS301000) form.
You’ll also import a generic inquiry designed to show the records entered on the Repair Work Orders form (the
entry form) in a table. The generic inquiry will function as a substitute form: It will be brought up instead of the
entry form when a user clicks the form’s name in a workspace.
You may see list of records used to describe the substitute form from the user’s perspective.
A shared filter has been developed for this substitute form. You will import it along with the generic inquiry.
You’ll also include the generic inquiry, filter, and entry form in the customization project.
The Form Elements
The Summary area of the Repair Work Orders (RS301000) data entry form will contain the following boxes:
•
Order Nbr.: The number that identifies the repair work order. In this box, a user can add a new number or
select an existing one to view the repair work order identified by the number.
•
Status (read-only): The status of the repair work order, which is one of these:
•
On Hold
•
Pending Payment
•
Ready for Assignment
•
Assigned
•
Completed
•
Paid
•
Date Created: The date of creation of the repair work order. The system will insert the current business date
by default.
•
Date Completed (read-only): The date when the repair work order is assigned the Completed status. (You'll
develop the business logic to fill in this date in a later training course.)
•
Priority: The priority of the repair.
•
Customer ID: The ID of the customer who requested the repair.


Part 1: Data Entry Form (Repair Work Orders) | 13
•
Service: The service to be performed in the repair work order; the user selects one of the services defined
on the Repair Services (RS201000) form.
•
Device: The device to be serviced; the user selects one of the devices defined on the Serviced Devices
(RS203000) form.
•
Assignee: The employee who performs the repair.
•
Description: The description of the repair work order.
•
Order Total (read-only): The price of the repair.
•
Invoice Nbr. (read-only): The number of the invoice created for the repair work order. (You’ll develop the
business logic to fill in this date in a later training course.)
The Repair Items tab will contain the same columns as the Repair Items tab of the Services and Prices (RS203000)
form, excluding the Required and Default columns from that form.
The Labor tab will contain the same columns as the Labor tab of the Services and Prices form.
Layout of the Form
In the Summary area of the form, you’ll define the layout as follows:
•
Arrange the input controls in three columns on the form
•
Specify a blue background for the third column
The resulting layout of the Repair Work Orders form is shown below.
Figure: The Repair Work Orders form
Database Tables Used for the Form
Below is the class diagram, which shows the relationships between the RSSVWorkOrder,
RSSVWorkOrderItem, and RSSVWorkOrderLabor data access classes, which are used for this form. The
relationships are defined by the following fields:
•
The RSSVWorkOrderItem.OrderNbr and RSSVWorkOrderLabor.OrderNbr fields are the
references to the data record of the master RSSVWorkOrder class.
•
The RSSVWorkOrderItem.InventoryID field is the reference to the stock item added to the repair
work order.
•
The RSSVWorkOrderLabor.InventoryID field is the reference to the non-stock item added to the
repair work order.
The structure of the key fields of the RSSVWorkOrderItem and RSSVWorkOrderLabor DACs is copied
from the structure of the key fields of the RSSVRepairItem and RSSVLabor DACs, respectively. The
RSSVWorkOrderItem DAC has the additional LineNbr key field because users can add multiple repair items


Part 1: Data Entry Form (Repair Work Orders) | 14
with the same service ID, device ID, and inventory ID to the Repair Items tab of the Repair Work Orders (RS301000)
form.
For a repair work order, the user specifies the repair service and device, which are represented as the references to
the corresponding classes as follows:
•
RSSVWorkOrder.ServiceID is a reference to the RSSVRepairService class.
•
RSSVWorkOrder.DeviceID is a reference to the RSSVDevice class.
Lesson Objectives
In this lesson, you will learn how to do the following:
•
Define the layout of a data entry form
•
Add a substitute form with a shared filter to the customization project
Step 1.1.1: Creating the Form (Self-Guided Exercise)
In this step, you’ll create the Repair Work Orders (RS301000) form on your own. Although this is a self-guided
exercise, you can use the details and suggestions in this topic as you create the form. The creation of a form is
described in detail in the T200 Maintenance Forms training course.
Creating the Form and Graph
Create the form and graph as follows:
1. On the toolbar of the Screens page of the Customization Project Editor, click Create New Screen.
2. In the Create New Screen dialog box, which opens, specify the following values:


Part 1: Data Entry Form (Repair Work Orders) | 15
•
Screen ID: RS.30.10.00
•
Graph Name: RSSVWorkOrderEntry
•
Graph Namespace: PhoneRepairShop
•
Page Title: Repair Work Orders
•
Template: FormTab
•
Create Modern UI Files: Selected
3. Move the generated RSSVWorkOrderEntry graph to the extension library.
Generating the DACs
Generate the DACs as specified below:
1. In Code Editor, generate the RSSVWorkOrder, RSSVWorkOrderItem, and RSSVWorkOrderLabor
DACs and move them to the extension library.
2. In the Helper\Messages.cs file, add the following properties to the Messages class.
        public const string RSSVWorkOrder = "Repair Work Order";
        public const string RSSVWorkOrderItem = 
            "Repair Item Included in Repair Work Order";
        public const string RSSVWorkOrderLabor = "Work Order Labor";
Configuring the RSSVWorkOrder DAC
In Visual Studio, specify the system attributes and other attributes of the RSSVWorkOrder DAC as shown in the
code fragments below:
•
For the DAC, define the following attribute.
    [PXCacheName(Messages.RSSVWorkOrder)]
•
Define attributes for the system fields. (For details about the definition of the attributes of the system fields,
see the T200 Maintenance Forms training course or Audit Fields, Concurrent Update Control (TStamp), and
Attachment of Additional Objects to Data Records (NoteID).)
•
Define the attributes of the RepairItemLineCntr field, which is not displayed on the UI, as follows. (You
will configure the attributes of the fields that are displayed in the UI in Step 1.1.2: Configuring the Controls of
the Summary Area.)
        [PXDBInt()]
        [PXDefault(0)]
        public virtual int? RepairItemLineCntr { get; set; }
        public abstract class repairItemLineCntr :
            PX.Data.BQL.BqlInt.Field<repairItemLineCntr> { }
You need this field to define the numbering of repair items on the Repair Items tab of the Repair Work
Orders (RS301000) form by using the predefined PXLineNbr attribute. For details about this approach, see
the T210 Customized Forms and Master-Details Relationships training course.
Configuring the RSSVWorkOrderItem DAC
Specify the system attributes and other attributes of the RSSVWorkOrderItem DAC as shown in the code
fragments below:
•
For the DAC, define the following attribute.


Part 1: Data Entry Form (Repair Work Orders) | 16
    [PXCacheName(Messages.RSSVWorkOrderItem)]
•
Configure the same attributes that have been configured for the corresponding fields of the
RSSVRepairItem DAC except for the PXParent attribute, which you need to assign to the OrderNbr
field.
•
Make OrderNbr and LineNbr to be the key fields.
You need to make the LineNbr a key field so that a user can add multiple items with the
same InventoryID. In this case, the LineNbr is an alternative field to the InventoryID
field.
The RSSVWorkOrderItem DAC fields excluding the system fields should look as shown in the following code.
    [PXCacheName(Messages.RSSVWorkOrderItem)]
    public class RSSVWorkOrderItem : PXBqlTable, IBqlTable
    {
        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXDBDefault(typeof(RSSVWorkOrder.orderNbr))]
        [PXParent(typeof(SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.orderNbr.
                IsEqual<RSSVWorkOrderItem.orderNbr.FromCurrent>>))]
        public virtual string? OrderNbr { get; set; }
        public abstract class orderNbr : 
            PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion
        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(RSSVWorkOrder.repairItemLineCntr))]
        [PXUIField(DisplayName = "Line Nbr.", Visible = false)]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion
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
            }
            )]
        [PXUIField(DisplayName = "Repair Item Type")]


Part 1: Data Entry Form (Repair Work Orders) | 17
        public virtual string? RepairItemType { get; set; }
        public abstract class repairItemType : 
            PX.Data.BQL.BqlString.Field<repairItemType> { }
        #endregion
        #region InventoryID
        [Inventory]
        [PXRestrictor(typeof(
            Where<InventoryItemExt.usrRepairItem.IsEqual<True>.
            And<Brackets<
                RSSVWorkOrderItem.repairItemType.FromCurrent.IsNull.
                Or<InventoryItemExt.usrRepairItemType.
                    IsEqual<RSSVWorkOrderItem.repairItemType.FromCurrent>>>>>),
            Messages.StockItemIncorrectRepairItemType,
            typeof(RSSVWorkOrderItem.repairItemType))]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : 
            PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion
        #region BasePrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Price")]
        [PXFormula(null, typeof(SumCalc<RSSVWorkOrder.orderTotal>))]
        public virtual Decimal? BasePrice { get; set; }
        public abstract class basePrice : 
            PX.Data.BQL.BqlDecimal.Field<basePrice> { }
        #endregion
    
    // system fields
}
Configuring the RSSVWorkOrderLabor DAC
Specify the system attributes and other attributes of the RSSVWorkOrderLabor DAC as shown in the code
fragments below:
•
For the DAC, define the following attribute.
    [PXCacheName(Messages.RSSVWorkOrderLabor)]
•
Configure the same attributes that have been configured for the corresponding fields of the RSSVLabor
DAC except for the PXParent attribute, which you need to assign to the OrderNbr field.
•
Make OrderNbr and InventoryID to be the key fields.
You need to make InventoryID a key field so that a user cannot specify multiple items with
the same InventoryID.
The RSSVWorkOrderLabor DAC fields excluding the system fields should look as shown in the following code.
    [PXCacheName(Messages.RSSVWorkOrderLabor)]
    public class RSSVWorkOrderLabor : PXBqlTable, IBqlTable
    {
        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXDBDefault(typeof(RSSVWorkOrder.orderNbr))]


Part 1: Data Entry Form (Repair Work Orders) | 18
        [PXParent(typeof(SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.orderNbr.
                IsEqual<RSSVWorkOrderLabor.orderNbr.FromCurrent>>))]
        public virtual string? OrderNbr { get; set; }
        public abstract class orderNbr : 
            PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion
        #region InventoryID
        [Inventory(IsKey = true)]
        [PXRestrictor(typeof(Where<InventoryItem.stkItem, Equal<False>>),
            Messages.ItemIsStock)]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : 
            PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion
        #region DefaultPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Default Price")]
        public virtual Decimal? DefaultPrice { get; set; }
        public abstract class defaultPrice : 
            PX.Data.BQL.BqlDecimal.Field<defaultPrice> { }
        #endregion
        #region Quantity
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Quantity")]
        public virtual Decimal? Quantity { get; set; }
        public abstract class quantity : 
            PX.Data.BQL.BqlDecimal.Field<quantity> { }
        #endregion
        #region ExtPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Ext. Price", Enabled = false)]
        [PXFormula(
            typeof(RSSVWorkOrderLabor.quantity.
                Multiply<RSSVWorkOrderLabor.defaultPrice>),
            typeof(SumCalc<RSSVWorkOrder.orderTotal>))]
        public virtual Decimal? ExtPrice { get; set; }
        public abstract class extPrice : 
            PX.Data.BQL.BqlDecimal.Field<extPrice> { }
        #endregion
    
    // system fields
}
Configuring the Graph
Configure the RSSVWorkOrderEntry graph as follows:


Part 1: Data Entry Form (Repair Work Orders) | 19
1. Define data views in the generated graph and make the full list of standard system actions available by
specifying the second generic type parameter in the base PXGraph class as the following code shows.
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry,
        RSSVWorkOrder>
    {
        #region Views
        //The primary view
        public SelectFrom<RSSVWorkOrder>.View WorkOrders = null!;
        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
            Where<RSSVWorkOrderItem.orderNbr.
                IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            RepairItems = null!;
        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
            Where<RSSVWorkOrderLabor.orderNbr.
                IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            Labor = null!;
        #endregion
        //Automatically generated data views
    }
You need to keep the automatically generated data views until the customization project
contains the ASPX code of the Classic UI, which uses these data views.
2. Build the project in Visual Studio and publish the customization project.
Including Access Rights and Category in the Customization Project
1. On the Access Rights by Screen (SM201020) form, make sure Delete access rights for the Repair Work Orders
(RS301000) form are provided for the Customizer user role.
2. Make sure the access rights for the new form are included in the customization project.
3. Include a link to the form in the Profiles category of the Phone Repair Shop workspace.
4. Update the SiteMapNode item for the Repair Work Orders form in the customization project.
Step 1.1.2: Configuring the Controls of the Summary Area
In this step, you will configure the controls associated with the UI elements of the Summary area of the Repair Work
Orders (RS301000) form. (You’ll find a description of these elements in The Form Elements.)
To configure the UI elements, you will specify the attributes of the fields of the RSSVWorkOrder DAC. You will
create the corresponding UI controls in the RS301000.html page in the next step by using the field tag.
Configuring a Selector Control with an Input Mask
You will configure a selector with an input mask for Order Nbr.:


Part 1: Data Entry Form (Repair Work Orders) | 20
•
By using the InputMask property of the PXDBString attribute, you will specify that users can enter any
symbols in the box. If this symbols are letters, they must be uppercase. For more information about input
masks, see To Configure an Input Mask and a Display Mask for a Field.
•
You’ll specify that the input value cannot be null and cannot contain only spaces by setting the
PersistingCheck property of the PXDefault attribute to PXPersistingCheck.NullOrBlank.
•
In the PXSelector attribute, you won’t specify any fields to be displayed as columns of the lookup
table. Instead, you will use the Visibility property of the PXUIField attribute of the OrderNbr,
Description, ServiceID, and DeviceID fields to include these fields in the selector of the
OrderNbr field.
In the RSSVWorkOrder DAC, configure the attributes of the OrderNbr field and the fields included in the
selector, as shown in the following code:
1. For the OrderNbr field
        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true,
            InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Order Nbr.",
            Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<RSSVWorkOrder.orderNbr>))]
        public virtual string? OrderNbr { get; set; }
        public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion
2. For the Description field
        #region Description
        [PXDBString(60, IsUnicode = true)]
        [PXUIField(DisplayName = "Description",
            Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string? Description { get; set; }
        public abstract class description : 
            PX.Data.BQL.BqlString.Field<description> { }
        #endregion
3. For the DeviceID field
        #region DeviceID
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Device",
            Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<RSSVDevice.deviceID>),
            typeof(RSSVDevice.deviceCD),
            typeof(RSSVDevice.description),
            SubstituteKey = typeof(RSSVDevice.deviceCD),
            DescriptionField = typeof(RSSVDevice.description))]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
        #endregion
4. For the ServiceID field
        #region ServiceID
        [PXDBInt()]
        [PXDefault]


Part 1: Data Entry Form (Repair Work Orders) | 21
        [PXUIField(DisplayName = "Service",
            Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<RSSVRepairService.serviceID>),
            typeof(RSSVRepairService.serviceCD),
            typeof(RSSVRepairService.description),
            SubstituteKey = typeof(RSSVRepairService.serviceCD),
            DescriptionField = typeof(RSSVRepairService.description))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : 
            PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion
Configuring a Selector Control for Customer Records
You will configure a selector control for a segmented key value in Customer ID by using the CustomerActive
attribute, defined in the PX.Objects.AR namespace. This attribute selects only the customer records that have
the Active or One-Time status.
You can find the attribute that can be used for a selector control that retrieves the data from an
Acumatica ERP database table by investigating the source code of a similar selector. For example, if
you want to find an attribute that can be used with a customer selector control in the Summary area
of a document, you can investigate the attributes assigned to the ARInvoice.CustomerID field,
which corresponds to the Customer box on the Invoices and Memos (AR301000) form.
In the RSSVWorkOrder DAC, configure the attributes of the CustomerID field as follows:
1. In the RSSVWorkOrder.cs file, add the using directives as follows.
using PX.Objects.AR;
2. For the CustomerID field, specify the attributes as follows.
        #region CustomerID
        [PXDefault]
        [CustomerActive(DisplayName = "Customer ID",
            DescriptionField = typeof(Customer.acctName))]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
        #endregion
Configuring a Selector Control for the List of Employees
You will configure a selector control for Assignee by using the Owner attribute, defined in the PX.TM namespace.
This attribute shows the list of employees.
To find an attribute that can be used with a employee selector control in the Summary area of a
document, you can investigate the attributes assigned to the CR.Contact.OwnerID field, which
corresponds to the Owner box on the Leads (CR301000) form.
In the RSSVWorkOrder DAC, configure the attributes of the Assignee field as follows:
1. In the RSSVWorkOrder.cs file, add the using directives as follows.
using PX.TM;


Part 1: Data Entry Form (Repair Work Orders) | 22
2. For the Assignee field, specify the attributes as follows.
        #region Assignee
        [Owner(DisplayName = "Assignee")]
        public virtual int? Assignee { get; set; }
        public abstract class assignee : PX.Data.BQL.BqlInt.Field<assignee> { }
        #endregion
Configuring a Read-Only Box
You will configure a box with the invoice number for Invoice Nbr.: You will make this field read-only by setting the
Enabled property of the PXUIField attribute to false.
Define the attributes of the InvoiceNbr field as follows.
        #region InvoiceNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Invoice Nbr.", Enabled = false)]
        public virtual string? InvoiceNbr { get; set; }
        public abstract class invoiceNbr : 
            PX.Data.BQL.BqlString.Field<invoiceNbr> { }
        #endregion
Configuring a Box with a Decimal Number
You will configure a box with a decimal number for Order Total:
•
You will set the default value of the field by using the PXDefault attribute.
•
You’ll make this field read-only by setting the Enabled property of the PXUIField attribute to false.
Define the attributes of the OrderTotal field, as the following code shows.
        #region OrderTotal
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Order Total", Enabled = false)]
        public virtual Decimal? OrderTotal { get; set; }
        public abstract class orderTotal : 
            PX.Data.BQL.BqlDecimal.Field<orderTotal> { }
        #endregion
Configuring Date-and-Time Controls
You will configure date-and-time controls for Date Created and Date Completed:
•
By using PXDBDate, you will configure the date-and-time controls to display and require users to enter
only the date in the control.
•
You’ll set the default value for Date Created as the current business date, which is stored in the
AccessInfo.BusinessDate system field.
Configure the attributes of the DateCreated and DateCompleted fields, as shown in the following code:
•
For the DateCreated field
        #region DateCreated
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]


Part 1: Data Entry Form (Repair Work Orders) | 23
        [PXUIField(DisplayName = "Date Created")]
        public virtual DateTime? DateCreated { get; set; }
        public abstract class dateCreated :
            PX.Data.BQL.BqlDateTime.Field<dateCreated> { }
        #endregion
•
For the DateCompleted field
        #region DateCompleted
        [PXDBDate()]
        [PXUIField(DisplayName = "Date Completed", Enabled = false)]
        public virtual DateTime? DateCompleted { get; set; }
        public abstract class dateCompleted :
            PX.Data.BQL.BqlDateTime.Field<dateCompleted> { }
        #endregion
Configuring Drop-Down Lists
You will configure drop-down lists for Status and Priority:
•
By using the PXStringList attribute, you’ll configure drop-down lists for these boxes.
•
You will use localizable names for the list items.
Configure the attributes of the Status and Priority fields as follows:
1. In the Messages class, define the constants to be used in the Status box, as shown in the following code.
        //Work order statuses
        public const string OnHold = "On Hold";
        public const string PendingPayment = "Pending Payment";
        public const string ReadyForAssignment = "Ready for Assignment";
        public const string Assigned = "Assigned";
        public const string Completed = "Completed";
        public const string Paid = "Paid";
2. Make sure the constants are already defined for the Priority box, as shown in the following code.
        //Complexity of repair
        public const string High = "High";
        public const string Medium = "Medium";
        public const string Low = "Low";
3. In the Constants.cs file, define the constants for the Priority box, as shown in the following code.
    //Constants for the priority of repair work orders
    public static class WorkOrderPriorityConstants
    {
        public const string High = "H";
        public const string Medium = "M";
        public const string Low = "L";
    }
4. Define the constants for the Status box, as shown in the following code.
    //Constants for the statuses of repair work orders
    public static class WorkOrderStatusConstants
    {
        public const string OnHold = "OH";


Part 1: Data Entry Form (Repair Work Orders) | 24
        public const string PendingPayment = "PP";
        public const string ReadyForAssignment = "RA";
        public const string Assigned = "AS";
        public const string Completed = "CM";
        public const string Paid = "PD";
    }
5. For the Status field, specify the attributes as follows.
        #region Status
        [PXDBString(2, IsFixed = true)]
        [PXDefault(WorkOrderStatusConstants.OnHold)]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        [PXStringList(
            new string[]
            {
                WorkOrderStatusConstants.OnHold,
                WorkOrderStatusConstants.PendingPayment,
                WorkOrderStatusConstants.ReadyForAssignment,
                WorkOrderStatusConstants.Assigned,
                WorkOrderStatusConstants.Completed,
                WorkOrderStatusConstants.Paid
            },
            new string[]
            {
                Messages.OnHold,
                Messages.PendingPayment,
                Messages.ReadyForAssignment,
                Messages.Assigned,
                Messages.Completed,
                Messages.Paid
            })]
        public virtual string? Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion
6. For the Priority field, specify the attributes as follows.
        #region Priority
        [PXDBString(1, IsFixed = true)]
        [PXDefault(WorkOrderPriorityConstants.Medium)]
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
            PX.Data.BQL.BqlString.Field<priority> { }


Part 1: Data Entry Form (Repair Work Orders) | 25
        #endregion
Configuring the Hold Field
The RSSVWorkOrder database table also contains the Hold field. This legacy field was used in previous versions
of the course before the workflow was implemented for the form. However, we recommended that you keep this
field in the database table in case you plan to use it alongside the workflow—for example, to trigger a transition
from the On Hold status. At this time, you only need to set the default value of the field by using the PXDefault
attribute.
Do the following:
1. For the Hold field, add the following attribute.
        #region Hold
        [PXDBBool()]
        [PXDefault(true)]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion
2. Build the project in Visual Studio.
Related Links
•
To Configure an Input Mask and a Display Mask for a Field
•
PXSelectorAttribute Class
•
PXStringListAttribute Class
Data Entry Form: General Information
A data entry form is used for the input of records of a particular type. This topic provides recommendations and
guidelines on organizing the layout of a data entry form.
The following screenshot shows an example of a data entry form. As is true of most data entry forms, the displayed
form has a Summary area and a tab area with multiple tabs. The displayed tab shows a table (generally referred to
as a grid).


Part 1: Data Entry Form (Repair Work Orders) | 26
Figure: A data entry form
Applicable Scenarios
You configure a data entry form in the following cases:
•
You are migrating an existing data entry form to the Modern UI.
•
You are creating a new data entry form by using the Modern UI.
Templates and Label Sizes
Data entry forms that display transactional data, such as Invoices and Memos (AR301000), should use three-column
templates for the Summary area. You can select a particular template by the general recommendations below. You
use the default label size.
Data entry forms that display profile data, such as Customers (AR303000), should use the 1-1 template. Label size
should be M.
When selecting a particular template, you can also use the general recommendations that are described in Form
Layout: Predefined Templates.
Recommendations for Organizing the Layout
The following table shows recommendations for organizing the layout of a data entry form.
Correct
Incorrect
If you need to show total numbers, put them in the blue fieldset (class="highlights-section") on the
right side of the screen.
Do not put the blue fieldset between other fieldsets.


Part 1: Data Entry Form (Repair Work Orders) | 27
Correct
Incorrect
 
 
If you have multiple fieldsets with short labels and short values in fields, put them in multiple columns and
stacks by using one of the following templates: 1-1-1, 7-10-7, 17-17-14, or 1-1.
Do not put fields with short labels and short values in fields in the 1 template.
 
 
Make labels with long text longer. Try to make the labels in all fieldsets similar in length (specify class="la-
bel-size-<SIZE>" in qp-template).
 
 


Part 1: Data Entry Form (Repair Work Orders) | 28
Correct
Incorrect
Use a caption instead of showing a single tab.
For grids, add the caption as described in Table with a Title.
For fieldsets in a qp-template, use the qp-caption control.
 
 
Try to occupy slots of the template equally in order to balance the screen.
 
 
Use the multiline Description field because the width of a single slot on the data entry form may not fit the
field properly. For details, see Text Box: Multiline Text Box.
 
 
Show full-width grids without a gray background.


Part 1: Data Entry Form (Repair Work Orders) | 29
Correct
Incorrect
 
 
UX and Functional Guidelines
The form design should be tailored for screens with a resolution of 1280 x 720.
The number of the data entry form should start with 30. For details, see Form and Report Numbering.
Data Entry Form: Definition in TypeScript and HTML
The following topic describes how to configure a data entry form depending on its layout.
View Definition in TypeScript
For a form with a Summary area and multiple tabs, you use the following in the TypeScript file of the form:
•
For the summary view and the views that display one record, the createSingle method.
•
For each property for the view that displays a grid, the createCollection method.
•
For the summary view and each view for a tab, a class that extends PXView with the full list of fields to be
displayed.
•
The headerDescription decorator for fields in the Summary area whose value should be included in
the record title below the form name.
•
The gridConfig and columnConfig decorators for each grid and its columns. For details, see Table
(Grid): Configuration of the Table and Its Columns.
The following code shows an example of the TypeScript configuration for the Sales Orders (SO301000) form.
import
{
    createCollection,
    createSingle,
    PXScreen,
    graphInfo,
    PXActionState
} from "client-controls";
  
@graphInfo({
    graphType: 'PX.Objects.SO.SOOrderEntry',
    primaryView: 'Document'


Part 1: Data Entry Form (Repair Work Orders) | 30
})
export class SO301000 extends PXScreen {
    //Actions that are used in qp-button tags in HTML
    AddInvoiceOK: PXActionState;
    OverrideBlanketTaxZone: PXActionState;
    ...
 
    //Properties for view classes
    Document = createSingle(SOOrderHeader);
    Transactions = createCollection(SOLine);
    Taxes = createCollection(SOTaxTran);
    CurrentDocument = createSingle(SOOrder);
    ...
}
  
//View classes
export class SOOrderHeader extends PXView {
    OrderType: PXFieldState;
    OrderNbr: PXFieldState;
    Status: PXFieldState<PXFieldOptions.Disabled>;
    DontApprove: PXFieldState<PXFieldOptions.Disabled>;
    Approved: PXFieldState<PXFieldOptions.Disabled>;
    OrderDate: PXFieldState<PXFieldOptions.CommitChanges>;
    RequestDate: PXFieldState<PXFieldOptions.CommitChanges>;
    CustomerOrderNbr: PXFieldState<PXFieldOptions.CommitChanges>;
    CustomerRefNbr: PXFieldState;
    CuryInfoID: PXFieldState;
 
    @headerDescription
    CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
    CustomerLocationID: PXFieldState<PXFieldOptions.CommitChanges>;
    ContactID: PXFieldState<PXFieldOptions.CommitChanges>;
    CuryID: PXFieldState<PXFieldOptions.CommitChanges>;
    DestinationSiteID: PXFieldState<PXFieldOptions.CommitChanges>;
    ProjectID: PXFieldState<PXFieldOptions.CommitChanges>;
    OrderDesc: PXFieldState;
 
    OrderQty: PXFieldState<PXFieldOptions.Disabled>;
    CuryDiscTot: PXFieldState<PXFieldOptions.CommitChanges>;
    CuryVatExemptTotal: PXFieldState<PXFieldOptions.Disabled>;
    CuryVatTaxableTotal: PXFieldState<PXFieldOptions.Disabled>;
    CuryTaxTotal: PXFieldState<PXFieldOptions.Disabled>;
    CuryOrderTotal: PXFieldState<PXFieldOptions.Disabled>;
    CuryControlTotal: PXFieldState<PXFieldOptions.CommitChanges>;
    ArePaymentsApplicable: PXFieldState<PXFieldOptions.CommitChanges>;
    IsRUTROTDeductible: PXFieldState<PXFieldOptions.CommitChanges>;
    IsFSIntegrated: PXFieldState<PXFieldOptions.Disabled>;
 
    ShowDiscountsTab: PXFieldState;
    ShowShipmentsTab: PXFieldState;
    ShowOrdersTab: PXFieldState;
}
 
export class SOOrder extends PXView {
    BranchID: PXFieldState<PXFieldOptions.CommitChanges>;
    BranchBaseCuryID: PXFieldState;
    DisableAutomaticTaxCalculation: PXFieldState<PXFieldOptions.CommitChanges>;


Part 1: Data Entry Form (Repair Work Orders) | 31
    ...
}
 
@gridConfig({
    preset: GridPreset.Details,
    initNewRow: true,
    syncPosition: true,
    wrapToolbar: true,
    statusField: "Availability"
})
export class SOLine extends PXView {
    //Table toolbar actions
    AddInvBySite: PXActionState;
    ShowMatrixPanel: PXActionState;
    ...
  
    //Table columns
    Availability: PXFieldState<PXFieldOptions.Hidden>;
    @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    ExcludedFromExport: PXFieldState;
    IsConfigurable: PXFieldState;
    @columnConfig({ hideViewLink: true })
    BranchID: PXFieldState<PXFieldOptions.CommitChanges>;
    ...
}
Layout in HTML
You define the layout of a data entry form by adding the following tags to the HTML code of the form:
•
A qp-template tag. For details on using templates, see Form Layout: Predefined Templates.
•
A qp-tabbar tag with a nested qp-tab element for each tab.
•
For each tab that does not contain a grid, a nested qp-template tag.
The following example shows the HTML template for the Sales Orders (SO301000) form.
<template>
    <qp-template name="7-10-7" id="form-Document" wg-container>
        <qp-fieldset id="fsColumnA" slot="A" view.bind="Document">
            <field name="OrderType"></field>
            <field name="OrderNbr"></field>
            ...
        </qp-fieldset>
        <qp-fieldset id="fsColumnB" slot="B" view.bind="Document">
            <field name="CustomerID" config-allow-edit.bind="true"></field>
            <field name="CustomerLocationID" config-allow-edit.bind="true"></field>
            ...
        </qp-fieldset>
        <qp-fieldset id="fsColumnC-summary" slot="C" view.bind="Document" 
            caption="Summary">
            <field name="OrderQty"></field>
            <field name="CuryDiscTot"></field>
            ...
        </qp-fieldset>
    </qp-template>


Part 1: Data Entry Form (Repair Work Orders) | 32
 
    <qp-tabbar active-tab-id="tabDetails" id="tabs">
        <qp-tab id="tabDetails" caption="Details">
            <qp-grid view.bind="Transactions" topBarConfig.bind="{disableMenu: false}">
            </qp-grid>
        </qp-tab>
        <qp-tab id="tabFinancial" caption="Financial">
            <qp-template id="form-Financial" name="1-1-1">
                <qp-fieldset id="fsColumnA-Financial" slot="A" view.bind="CurrentDocument"
                             caption="Financial Information">
                    <field name="BranchID"></field>
                    <field name="BranchBaseCuryID"></field>
                    ...
                </qp-fieldset>
                <div slot="B">
                    <qp-fieldset id="fsColumnB-Payment"
                                 view.bind="CurrentDocument"
                                 caption="Payment Information">
                        <field name="OverridePrepayment"></field>
                        <field name="PrepaymentReqPct"></field>
                        ...
                    </qp-fieldset>
                    <qp-fieldset id="fsColumnB-Ownership"
                                 view.bind="CurrentDocument"
                                 caption="Ownership">
                        <field name="WorkgroupID"></field>
                        <field name="OwnerID"></field>
                    </qp-fieldset>
                    <qp-fieldset id="fsColumnB-Other" view.bind="CurrentDocument"
                                 caption="Other Information">
                        <field name="OrigOrderType" config-enabled.bind="false"></field>
                        <field name="OrigOrderNbr" config-allow-edit.bind="true"
                               config-enabled.bind="false"></field>
                        ...
                    </qp-fieldset>
                </div>
            </qp-template>
        </qp-tab>
        ...
    </qp-tabbar>
</template>
Step 1.1.3: Creating the Form’s UI
In this step, you will create the UI of the Repair Work Orders (RS301000) form.


Part 1: Data Entry Form (Repair Work Orders) | 33
Defining the Screen Class in TypeScript
You can open a TypeScript file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)
To define the view of the Repair Work Orders (RS301000) form in TypeScript, you define a screen class and a
property for the data views of the form. Do the following:
1. In the RS301000.ts file, add the import directives as follows.
import {
 PXScreen, createCollection, graphInfo,
 viewInfo, createSingle,
} from "client-controls";
2. Define the screen class for the form, as the following code shows. The class name is the ID of the form.
export class RS301000 extends PXScreen {}
3. For the screen class, add the graphInfo decorator, and specify the graph and the primary view of the form
in the decorator properties, as the following code shows.
@graphInfo({
 graphType: "PhoneRepairShop.RSSVWorkOrderEntry",
 primaryView: "WorkOrders"
})
export class RS301000 extends PXScreen {}
4. Define the properties for the data views of the form, as the following code shows. To initialize the data view
of the Summary area of the form, you should use the createSingle method. For the data view that is
used to display a table, you need to initialize the property with the createCollection method. You will
define the view classes whose instances are used as input parameters of the methods in the next step.
The names of the data view properties should be the same as those in the graph. For example,
if the WorkOrders view is declared in the RSSVWorkOrderEntry graph, the property with
the same name should be declared in the RS301000 screen class.
export class RS301000 extends PXScreen {
 @viewInfo({ containerName: "Work Order" })
 WorkOrders = createSingle(RSSVWorkOrder);
 
 @viewInfo({ containerName: "Repair Items" })
 RepairItems = createCollection(RSSVWorkOrderItem);
 
 @viewInfo({ containerName: "Labor" })
 Labor = createCollection(RSSVWorkOrderLabor);
}


Part 1: Data Entry Form (Repair Work Orders) | 34
In the viewInfo decorator, you have specified the names of the containers. These names are used as
object names during the configuration of particular functionality, such as workflows and import and export
scenarios. If this value is not specified, the system displays the name of the data view as the object name.
Defining the Primary View Class in TypeScript
You need to define a view class for the primary data view of the Repair Work Orders (RS30100) form, which is
WorkOrders.
Proceed as follows:
1. In the RS301000.ts file, update the list of import directives, as the following code shows.
import {
 PXScreen, createCollection, graphInfo,
 viewInfo, createSingle,
 PXView, PXFieldOptions, PXFieldState, controlConfig,
} from "client-controls";
2. Define the RSSVRepairWorkOrder view class as follows.
export class RSSVWorkOrder extends PXView {}
3. In the view class, specify the properties for all data fields of the data view that should be displayed in the UI,
as shown below. You use the name of the data field as the property name.
export class RSSVWorkOrder extends PXView {
 OrderNbr: PXFieldState;
 
 @controlConfig({allowEdit: true, })
 CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
 DateCreated: PXFieldState;
 DateCompleted: PXFieldState;
 Status: PXFieldState;
 
 @controlConfig({rows: 2})
 Description : PXFieldState<PXFieldOptions.Multiline>;
 
 @controlConfig({allowEdit: true, })
 ServiceID : PXFieldState<PXFieldOptions.CommitChanges>;
 
 @controlConfig({allowEdit: true, })
 DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;
 OrderTotal: PXFieldState;
 Assignee: PXFieldState;
 Priority: PXFieldState<PXFieldOptions.CommitChanges>;
 InvoiceNbr: PXFieldState;
}
For the CustomerID, ServiceID, DeviceID, and Priority fields, changes should be committed to
the server; therefore, you have used the PXFieldOptions.CommitChanges option for the property
type.
You have defined the links in the selector controls for the CustomerID, ServiceID, and DeviceID
fields by specifying allowEdit: true in the controlConfig decorator.


Part 1: Data Entry Form (Repair Work Orders) | 35
You have used the controlConfig decorator with the specified rows property and the Description
field with the PXFieldOptions.Multiline option to define a multiline text box with two text lines. For
details, see Text Box: Multiline Text Box.
Defining the View Classes for Tables in TypeScript
In the TypeScript file of the form, you need to define view classes for the views that are bound to tables on
the Repair Items and Labor tabs of the Repair Work Orders (RS301000) form: RSSVWorkOrderItem and
RSSVWorkOrderLabor. Proceed as follows:
1. In the RS301000.ts file, add gridConfig and GridPreset to the list of import directives.
2. Define the RSSVWorkOrderItem view class as follows.
export class RSSVWorkOrderItem extends PXView {
 RepairItemType: PXFieldState;
 InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
 InventoryID_description: PXFieldState;
 BasePrice: PXFieldState;
}
For the InventoryID field, changes should be committed to the server; therefore, you have used the
PXFieldOptions.CommitChanges option for the property type.
3. Add the gridConfig decorator to the RSSVWorkOrderItem view class, as the following code shows.
In the gridConfig decorator, you must specify the preset property. Because the table is used on one of
the tabs of the data entry form, you use the Details preset. For information about presets, see Form Layout:
Grid Presets.
@gridConfig({
 preset: GridPreset.Details
})
export class RSSVWorkOrderItem extends PXView {
 RepairItemType: PXFieldState;
 InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
 InventoryID_description: PXFieldState;
 BasePrice: PXFieldState;
}
4. Define the RSSVWorkOrderLabor view class similarly to the way you defined the
RSSVWorkOrderItem view class. The resulting class should be defined as follows.
@gridConfig({
 preset: GridPreset.Details
})
export class RSSVWorkOrderLabor extends PXView {
 InventoryID: PXFieldState;
 InventoryID_description: PXFieldState;
 DefaultPrice: PXFieldState;
 Quantity: PXFieldState<PXFieldOptions.CommitChanges>;
 ExtPrice: PXFieldState;
}
5. Save your changes.


Part 1: Data Entry Form (Repair Work Orders) | 36
Defining the Layout in HTML
You can open an HTML file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)
The Repair Work Orders (RS301000) form contains the Summary area and two tabs below it. The Summary area
has three columns, which you can arrange by using the 7-10-7 template. Each tab contains a table. To define the
layout of the form, do the following:
1. Define the Summary area of the form by adding the qp-template tag with the 7-10-7 template.
For each slot, define a fieldset, as shown in the following code. For the third fieldset, specify
class="highlights-section", which makes the fieldset have blue background.
<template>
  <qp-template
    id="form-Order"
    name="7-10-7"
    class="equal-height"
    qp-collapsible
  >
    <qp-fieldset id="fsColumnA-Order" slot="A" view.bind="WorkOrders">
    </qp-fieldset>
    <qp-fieldset id="fsColumnB-Order" slot="B" view.bind="WorkOrders">
    </qp-fieldset>
    <qp-fieldset id="fsColumnC-Order" slot="C" view.bind="WorkOrders"
      class="highlights-section">
    </qp-fieldset>
  </qp-template>
</template>
Each fieldset has been bound to the same WorkOrders property.
You have defined the whole Summary area to be collapsible by using the qp-collapsible attribute. For
more information, see Collapsible Area: Configuration.
For details about the qp-template tag and slots, see Form Layout: Predefined Templates.
2. In each fieldset, add the field tags for the fields that should be displayed in corresponding fieldset, as the
following code shows.
    <qp-fieldset id="fsColumnA-Order" slot="A" view.bind="WorkOrders">
      <field name="OrderNbr"></field>
      <field name="Status"></field>
      <field name="DateCreated"></field>
      <field name="DateCompleted"></field>
      <field name="Priority"></field>
    </qp-fieldset>
    <qp-fieldset id="fsColumnB-Order" slot="B" view.bind="WorkOrders">
      <field name="CustomerID"></field>
      <field name="ServiceID"></field>
      <field name="DeviceID"></field>
      <field name="Assignee"></field>
      <field name="Description"></field>
    </qp-fieldset>


Part 1: Data Entry Form (Repair Work Orders) | 37
    <qp-fieldset id="fsColumnC-Order" slot="C" view.bind="WorkOrders"
      class="highlights-section">
      <field name="OrderTotal"></field>
      <field name="InvoiceNbr"></field>
    </qp-fieldset>
3. Define the Repair Items tab:
a. Aer the qp-template tag, add the qp-tabbar tag with a nested qp-tab tag, as the following code
shows. In the qp-tab tab, specify the name of the tab in the caption attribute.
  <qp-tabbar id="tabbar">
    <qp-tab id="tab-RepairItems" caption="Repair Items">
    </qp-tab>
  </qp-tabbar>
b. Define the table that should be displayed on the Repair Items tab: In the qp-tab tag, add the qp-grid
tag, which is bound to the RepairItems view, as the following code shows.
  <qp-tabbar id="tabbar">
    <qp-tab id="tab-RepairItems" caption="Repair Items">
      <qp-grid id="grid-RepairItems" view.bind="RepairItems"></qp-grid>
    </qp-tab>
  </qp-tabbar>
4. Define the Labor tab similarly to the way you defined the Repair Items tab: In the qp-tabbar tag, add the
qp-tab tag aer the qp-tab tag that was defined in the previous instruction. In the new qp-tab tag, add
the qp-grid tag, and bind it to the Labor view, as the following code shows.
    <qp-tab id="tab-Labor" caption="Labor">
      <qp-grid id="grid-Labor" view.bind="Labor"></qp-grid>
    </qp-tab>
5. Save your changes.
Building and Viewing the Form
To build the source files for the Repair Work Orders (RS301000) form and view its Modern UI version, do the
following:
1. Either update the files in the customization project and publish it, or run the following command in the
FrontendSources\screen folder of your instance.
npm run build-dev --- --env screenIds=RS301000
2. Aer the source files have been built successfully, launch your Acumatica ERP instance, and open the Repair
Work Orders form.
3. Select a customer record in the Customer ID box, click the link in the box, and make sure the Customers
(AR303000) form opens with the record displayed. Close the Customers form.
4. On the Repair Work Orders form, make sure the Description box is multiline.
Related Links
•
Text Box: Multiline Text Box
•
Collapsible Area: Configuration


Part 1: Data Entry Form (Repair Work Orders) | 38
Step 1.1.4: Adding the Generic Inquiry Form and Filter to the Project
In this step, you will import the Repair Work Orders (RS3010PL) generic inquiry form: the substitute form that has
been developed for the Repair Work Orders (RS301000) entry form.
A shared filter has been developed for and applied to this substitute form. With this filter applied, the substitute
form displays the repair work orders that have the Ready for Assignment status. (For details about shared filters, see
Saving of Filters for Future Use.)
You will then add both the generic inquiry for the substitute form and the shared filter to the customization project.
Importing the Substitute Form and the Shared Filter
Do the following:
1. On the Generic Inquiry (SM208000) form, import the generic inquiry from the RepairWorkOrders.xml
file provided with this course. The file can be found in the Customization\T220\SourceFiles
\ListAsEntryPoint\ folder of the GitHub repository, which you’ve downloaded in Step 1: Preparing the
Environment. The file contains the generic inquiry and the shared filter.
2. Go to the Entry Point tab. In the Entry Screen box, select Repair Work Orders. The Replace Entry Screen
with This Inquiry in Menu check box becomes selected, indicating that when a user clicks Repair Work
Orders in a workspace or search results, the system will open this generic inquiry form instead of the entry
form.
3. Make sure the Enable New Record Creation check box is selected so that a user will be able to create a new
repair work order from the generic inquiry form.
4. Save your changes.
5. Open the Repair Work Orders (RS3010PL) form. Make sure the substitute form with two available filters—All
Records and To Assign (shown below)—is displayed when you open the form.
Figure: Substitute form
6. On the Generic Inquiries page of the Customization Project Editor, add the generic inquiry to the
PhoneRepairShop customization project.


Part 1: Data Entry Form (Repair Work Orders) | 39
7. On the Access Rights page of the Customization Project Editor, add the access rights for the Repair Work
Orders (RS3010PL) generic inquiry form to the customization project.
8. On the Shared Filters page of the Customization Project Editor, add the shared filter to the project as follows:
a. On the page toolbar, click Add New Record.
b. In the Add Shared Filter dialog box, which opens, select the check box in the row for the To Assign filter.
c. Click Add.
Related Links
•
Saving of Filters for Future Use
Additional Information: Configuration of Controls
In this lesson, you have configured various types of controls on the Repair Work Orders (RS301000) form. Particular
scenarios of the configuration of controls—such as the modification of a drop-down list at runtime and the
replacement of the displayed key value at runtime—are outside of the scope of the course but may be useful to
some readers.
Modifying a Drop-Down List at Runtime
You can modify a drop-down list at runtime by using the SetList<>() static method of the PXStringList
attribute. You can do this in the RowSelected event handler or graph constructor.
For details about this scenario and other options to configure a drop-down list, see Combo Box: Configuration in the
documentation.
Replacing the Displayed Key Value
The SubstituteKey property specifies the field whose value should be shown in the control in the UI instead of
the field specified in the Search<> command.
For details about the configuration of selectors in code, see Selector Control: Configuration from Backend. For more
information about how to change the external presentation in run time, see External and Internal Presentation of
Field Values: General Information.
Replacing Attributes of DAC Fields in CacheAttached
The attributes that you add to a data field in the DAC are initialized once, during the startup of the domain. You can
replace attributes for a particular field by defining the CacheAttached event handler for this field in a graph.
These attributes are also initialized once, on the first initialization of the graph where you define this method.
For details about replacement of attributes of DAC fields, see CacheAttached: General Information.
Lesson 1.2: Copying Field Values from One Record to Another
In this lesson, you will implement the business logic for the Repair Work Orders (RS301000) form to meet the
following requirements.


Part 1: Data Entry Form (Repair Work Orders) | 40
Insertion of Default Records
This logic requirement will determine how and when the system inserts default repair item and labor records on
the Repair Work Orders (RS301000) form.
Conditions:
•
Values have been selected in the Service and Device boxes of the Summary area of the form.
•
These values correspond to those in a record that has been entered on the Services and Prices (RS203000)
form.
•
No records have been added on the Repair Items and Labor tabs.
Expected result: The default records that have been defined on the Services and Prices form will be inserted on the
Repair Items and Labor tabs.
Usage of Repair Item Type and Price
For each repair item on the Repair Work Orders (RS301000) form, this logic will cause the stock item’s settings to be
inserted as the repair item type and base price.
Condition: For a particular row on the Repair Items tab, a value is selected in the Inventory ID column.
Expected result: The values in the Repair Item Type and Price columns will be changed to the repair item type
and base price (respectively) of the selected stock item as specified on the Stock Items (IN202500) form.
This business logic replicates the business logic that has been defined for the Repair Items tab of the
Services and Prices form. You will implement this logic on your own.
Lesson Objectives
You will learn how to copy the field values from one record to another record by using event handlers.
Step 1.2.1: Adding Default Rows to Work Orders (with RowUpdated)
In this step, you’ll implement the copying of default records configured on the Services and Prices (RS203000) form
to tabs of the Repair Work Orders (RS301000) form when particular conditions are met.
The Company’s Workflow
Before getting into the details of the logic to be implemented, consider the needed workflow of the Smart Fix
company:
1. Managers enter the prices for particular service–device pairs on the Services and Prices (RS203000) form.
These prices will then be inserted during the creation of work orders.
2. When a customer comes to the Smart Fix shop for a phone repair, a consultant creates a repair work order
on the Repair Work Orders (RS301000) form.
3. When the consultant selects a service and device for a new repair work order, the default information about
the price for the service, which a manager has entered on the Services and Prices form, should be copied to
the new order.


Part 1: Data Entry Form (Repair Work Orders) | 41
Understanding the Logic
In this step, you will implement the copying of the default records defined on the Services and Prices (RS203000)
form to the Repair Items and Labor tabs of the Repair Work Orders (RS301000) form. The default record or records
should be inserted when a user creates a new work order on the form and the following conditions are met:
•
In the Service and Device boxes of the Summary area of the form, the user selects values for which a
service–device record has been defined on the Services and Prices form.
•
No rows have been added on the Repair Items and Labor tabs.
You need to insert rows on the Repair Work Orders form when the values of two fields of one record are specified.
Therefore, you will implement the RowUpdated event handler for the RSSVWorkOrder DAC. In this handler, you
will do the following:
•
To check that a new RSSVWorkOrder record has been inserted, you will use the Cache.GetStatus
method of the WorkOrders data view. An inserted record maintains the Inserted status until it is saved
to the database even if it has been updated. When the inserted record is deleted, it is assigned the specific
InsertedDeleted status.
•
To display the data records in the UI, you will insert them in PXCache by using the Insert method of
the RepairItems data view. The Insert method is invoked on the PXCache object of the first DAC
specified in the data view type (which is the main DAC of the data view). Once you’ve inserted the record, it
has the default values specified for the fields.
In Acumatica ERP, before a new record can be inserted in PXCache, all key fields of this record
must be assigned some values. Since both the OrderNbr field and the InventoryID field
must be the key fields for the RSSVWorkOrderLabor DAC, you need to assign a value to the
InventoryID field.
You don’t need to explicitly assign a value to the OrderNbr field because it has
PXDBDefault attached to it. The PXDBDefault attribute handles generating and assigning
a default value when the FieldDefaulting event is raised for this field, which always
happens before the record is inserted in PXCache.
•
Because you update the values of the inserted records, you need to call the Update method of PXCache to
trigger all the events related to the update of the fields of a detail record.
For details about modifications in PXCache, see Modification of Data in a PXCache Object in the documentation.
Inserting the Default Repair Items and Labor on the Tabs
Do the following:
1. Add the following RowUpdated event handler to the RSSVWorkOrderEntry graph.
        #region Events
        //Copy repair items and labor items from the Services and Prices form.
        protected virtual void _(Events.RowUpdated<RSSVWorkOrder> e)
        {
            if (WorkOrders.Cache.GetStatus(e.Row) != PXEntryStatus.Inserted ||
                e.Cache.ObjectsEqual<RSSVWorkOrder.serviceID, 
                    RSSVWorkOrder.deviceID>(e.Row, e.OldRow))
                return;
            if (e.Row.ServiceID == null || e.Row.DeviceID == null ||
                IsCopyPasteContext || RepairItems.Select().Count != 0 ||
                Labor.Select().Count != 0)
                return;


Part 1: Data Entry Form (Repair Work Orders) | 42
            //Retrieve the default repair items
            var repairItems = SelectFrom<RSSVRepairItem>.
                Where<RSSVRepairItem.serviceID.
                    IsEqual<RSSVWorkOrder.serviceID.FromCurrent>.
                And<RSSVRepairItem.deviceID.
                    IsEqual<RSSVWorkOrder.deviceID.FromCurrent>>>
                .View.Select(this);
            //Insert default repair items
            foreach (RSSVRepairItem item in repairItems)
            {
                RSSVWorkOrderItem orderItem = RepairItems.Insert();
                orderItem.RepairItemType = item.RepairItemType;
                orderItem.InventoryID = item.InventoryID;
                orderItem.BasePrice = item.BasePrice;
                RepairItems.Update(orderItem);
            }
            //Retrieve the default labor items
            var laborItems = SelectFrom<RSSVLabor>.
                Where<RSSVLabor.serviceID.
                    IsEqual<RSSVWorkOrder.serviceID.FromCurrent>.
                And<RSSVLabor.deviceID.
                    IsEqual<RSSVWorkOrder.deviceID.FromCurrent>>>
                .View.Select(this);
            //Insert the default labor items
            foreach (RSSVLabor item in laborItems)
            {
                RSSVWorkOrderLabor orderItem = new RSSVWorkOrderLabor();
                orderItem.InventoryID = item.InventoryID;
                orderItem = Labor.Insert(orderItem);
                orderItem.DefaultPrice = item.DefaultPrice;
                orderItem.Quantity = item.Quantity;
                orderItem.ExtPrice = item.ExtPrice;
                Labor.Update(orderItem);
            }
        }
        #endregion
2. In the RS301000.ts file, make sure that the DeviceID and ServiceID fields have the
PXFieldOptions.CommitChanges option specified for the property type.
3. Build the project.
Testing the Logic
On the Repair Work Orders (RS301000) form, do the following:
1. Create a work order with the following settings:
•
Order Nbr.: 000001
•
Customer ID: C000000001
•
Service: Battery Replacement
•
Device: Nokia 3310
•
Description: Battery replacement, Nokia 3310
During the T210 Customized Forms and Master-Details Relationships course, a price was defined on the
Services and Prices (RS203000) form for this service–device pair.


Part 1: Data Entry Form (Repair Work Orders) | 43
2. Make sure the rows are inserted on the Repair Items and Labor tabs.
3. Change the option in the Service box to Liquid Damage.
4. Make sure the rows on the Repair Items and Labor tabs remain the same. The new rows should be inserted
only if the Repair Items and Labor tabs contain no records.
5. Change the value in the Service box back to Battery Replacement.
6. Save the record.
Related Links
•
Modification of Data in a PXCache Object
Step 1.2.2: Updating Fields of a Record on Update of Its Field (with FieldUpdated
and FieldDefaulting)—Self-Guided Exercise
In this step, you’ll implement the logic used when a user adds a row on the Repair Items tab of the Repair Work
Orders (RS301000) form. In the added row, the user will select a value in the Inventory ID column. To populate the
Repair Item Type and Price columns, the system should use the repair item type and base price of the selected
stock item on the Stock Items (IN202500) form.
In this step, you will independently add code that copies the RSSVWorkOrderItem.BasePrice
and RSSVWorkOrderItem.RepairItemType values from the stock item record when the
RSSVWorkOrderItem.InventoryID value is changed.
Updating Fields of a Record on Update of Its InventoryID Field
As you implement the logic described above, add the FieldUpdated event handler for the
RSSVWorkOrderItem.InventoryID field and the FieldDefaulting event handler for the
RSSVWorkOrderItem.BasePrice field in the RSSVWorkOrderEntry class. In the RS301000.ts
file, the PXFieldOptions.CommitChanges property should be set for the InventoryID field of the
RSSVWorkOrderItem view class.
To guide you through this process, you can use the instructions provided in the T210 Customized Forms and Master-
Details Relationships training course.
Testing the Logic
On the Repair Work Orders (RS301000) form, do the following:
1. Open the 000001 work order.
2. On the Repair Items tab, add a row and select BAT3310EX in the Inventory ID column. Shi the focus away
from the cell. Make sure the system has filled in values in the Repair Item Type and Price columns.
Do not save your changes to the repair work order.
Lesson 1.3: Validating the Field Values
In this lesson, you will implement the validation of particular field values on the Repair Work Orders (RS301000)
form.


Part 1: Data Entry Form (Repair Work Orders) | 44
Lesson Objectives
You will learn how to validate the values of fields that:
•
Do not depend on the values of other fields of the same record
•
Depend on the values of other fields of the same record
Step 1.3.1: Validating an Independent Field Value (with FieldVerifying)
In this step, you’ll implement the validation of the value in the Quantity column on the Labor tab of the Repair
Work Orders (RS301000) form.
For each row on the Labor tab, the value in the Quantity column must:
•
Be greater than or equal to 0.
•
Be greater than or equal to the value in the Quantity column specified for the corresponding record on the
Labor tab of the Services and Prices (RS203000) form. The corresponding record on that form has the same
inventory ID, service ID, and device ID as the current row on the Labor tab of the Repair Work Orders form.
The system must display an error if the quantity in the row is less than 0. It also needs to display a warning and
correct the quantity if it is less than the value specified for the corresponding record on the Labor tab of the
Services and Prices form.
You will implement the FieldVerifying event handler for the Quantity field of the RSSVWorkOrderLabor
DAC. This event handler is intended for field validation that’s independent of other fields in the same data record.
For details about the validation of independent field values, see Data Validation: Validation of Field Values.
In the event handler, you will do the following:
•
When the new value in the Quantity column is negative, you will throw an exception (by using
PXSetPropertyException) to cancel the assignment of the new value to the Quantity field.
•
When the value is not negative but is smaller than the default quantity specified on the Services and
Prices form (in the RSSVLabor.Quantity field), you will attach the exception to the field by using
the RaiseExceptionHandling method and exit the method normally. This method will display a
warning for the validated data field but will not raise an exception, so that the method finishes normally and
e.NewValue is set.
To attach a warning to the control, you will specify PXErrorLevel.Warning in the
PXSetPropertyException constructor.
RaiseExceptionHandling, which is used to prevent the saving of a record or to
display an error or warning on the form, cannot be invoked on a PXCache instance in the
following event handlers: FieldDefaulting, FieldSelecting, RowSelecting, and
RowPersisted.
To select the default data record from the RSSVLabor DAC, you will configure a fluent BQL query with
three required parameters. In the fluent BQL statement, you will refer to each required parameter by
using the P.As[Type] class, where [Type] is Int for an integer parameter. In the Select() method
that executes the query, as the parameters, you will pass the values of RSSVWorkOrder.ServiceID,
RSSVWorkOrder.DeviceID, and RSSVWorkOrderLabor.InventoryID from the row for which the event
is triggered. To use parameters in a fluent BQL query, you need to add the PX.Data.BQL using directive to the
code. For details about the parameters in fluent BQL, see Parameters in Fluent BQL.


Part 1: Data Entry Form (Repair Work Orders) | 45
Validating the Value of the Quantity Field
To validate the value of the Quantity field, do the following:
1. In the Messages.cs file, add the following constants to the Messages class.
        public const string QuantityCannotBeNegative =
            "The value in the Quantity column cannot be negative.";
        public const string QuantityTooSmall = @"The value in the Quantity column
            has been corrected to the minimum possible value.";
2. In the RSSVWorkOrderEntry.cs file, add the following using directive (if it has not been added yet).
using PX.Data.BQL;
3. Add the following FieldVerifying event handler to the RSSVWorkOrderEntry graph.
        //Validate that Quantity is greater than or equal to 0 and
        //correct the value to the default if the value is less than the default.
        protected virtual void _(Events.FieldVerifying<RSSVWorkOrderLabor,
            RSSVWorkOrderLabor.quantity> e)
        {
            if (e.Row == null || e.NewValue == null) return;
            if ((decimal)e.NewValue < 0)
            {
                //Throwing an exception to cancel the assignment
                //of the new value to the field
                throw new PXSetPropertyException(e.Row,
                    Messages.QuantityCannotBeNegative);
            }
            var workOrder = WorkOrders.Current;
            if (workOrder != null)
            {
                //Retrieving the default labor item related to the work order labor
                RSSVLabor labor = SelectFrom<RSSVLabor>.
                    Where<RSSVLabor.serviceID.IsEqual<@P.AsInt>.
                        And<RSSVLabor.deviceID.IsEqual<@P.AsInt>>.
                        And<RSSVLabor.inventoryID.IsEqual<@P.AsInt>>>
                    .View.Select(this, workOrder.ServiceID, workOrder.DeviceID,
                    e.Row.InventoryID);
                if (labor != null && (decimal)e.NewValue < labor.Quantity)
                {
                    //Correcting the LineQty value
                    e.NewValue = labor.Quantity;
                    //Raising the ExceptionHandling event for the Quantity field
                    //to attach the exception object to the field
                    e.Cache.RaiseExceptionHandling<RSSVWorkOrderLabor.quantity>(
                        e.Row, e.NewValue, new PXSetPropertyException(e.Row,
                            Messages.QuantityTooSmall, PXErrorLevel.Warning));
                }
            }
        }
4. Build the project.


Part 1: Data Entry Form (Repair Work Orders) | 46
5. In the RS301000.ts file, make sure that the PXFieldOptions.CommitChanges option is specified
for the property type for the Quantity field of the RSSVWorkOrderLabor view class.
6. Save your changes.
7. Publish the customization project.
Testing the Validation
To check the validation, on the Repair Work Orders (RS301000) form, do the following:
1. Select the work order with the 000001 order number.
2. On the Labor tab, in the row for the CONSULT labor item, change the value in the Quantity column to -1
and click Save. Make sure the error is displayed, as shown in the following screenshot.
Figure: The error for a negative value
3. Change the value to 0.5, which is smaller than the default value of 1. Make sure that the warning message
is generated on the control and the value is corrected to 1 when you click Save, as shown below.


Part 1: Data Entry Form (Repair Work Orders) | 47
Figure: The warning message
4. Change the value to 2 and save the changes. Make sure no warning or error is displayed.
Related Links
•
Data Validation: Validation of Field Values
•
Data Validation: Validation of a Data Record
•
Parameters in Fluent BQL
•
PXCache.RaiseExceptionHandling Method
Step 1.3.2: Validating Dependent Fields of Records (with RowUpdating)
If a repair work order’s service requires a preliminary check, the order’s priority must be at least Medium. The
preliminary check requirement is specified for each service on the Repair Services (RS201000) form. When a user
selects a service in the Summary area of the Repair Work Orders (RS301000) form, the system must check whether
the priority satisfies the requirement. If not, it must display an error and cancel the update of the record.
In this step, you’ll implement validation of the Priority field of a work order record in the RowUpdating event
handler. Because the Priority field depends on the ServiceID field of a work order record, you will use the
RowUpdating event handler. The RowUpdating event happens during the update of a data record aer all field-
related events. At this moment, the modifications haven't been applied to the data record stored in the cache yet,
and you can cancel the update process.
The event arguments give you access to:
•
e.NewRow: The modified version of the work order record, which contains all changes made by field-
related events
•
e.Row: The copy of the original work order record stored in the cache
You will use the ObjectsEqual<>() method of the cache to compare these two records to find out if the
Priority or the ServiceID field has changed.
If the value of the Priority field doesn't pass validation, you’ll mark the field with an error message by calling
the RaiseExceptionHandling<>() method of the cache. You will also assign the proper Priority field
value.


Part 1: Data Entry Form (Repair Work Orders) | 48
If you want to cancel the changes that have not been saved in the cache, set the Cancel property of
the event arguments to true.
Validating a Work Order Record
To validate a work order record, do the following:
1. In the Messages.cs file, add the following constant to the Messages class.
        public const string PriorityTooLow =
            @"The priority must be at least Medium for
 the repair service that requires preliminary check.";
2. Add the following RowUpdating event handler to the RSSVWorkOrderEntry graph.
        //Display an error if the priority is too low for the selected service
        protected virtual void _(Events.RowUpdating<RSSVWorkOrder> e)
        {
            // The modified data record (not in the cache yet)
            RSSVWorkOrder row = e.NewRow;
            // The data record that is stored in the cache
            RSSVWorkOrder originalRow = e.Row;
            if (!e.Cache.ObjectsEqual<RSSVWorkOrder.priority,
                   RSSVWorkOrder.serviceID>(row, originalRow))
            {
                if (row.Priority == WorkOrderPriorityConstants.Low)
                {
                    //Obtain the service record
                    RSSVRepairService service = SelectFrom<RSSVRepairService>.
                        Where<RSSVRepairService.serviceID.IsEqual<@P.AsInt>>.
                            View.Select(this, row.ServiceID);
                    if (service != null && service.PreliminaryCheck == true)
                    {
                        //Display the error for the Priority field
                        WorkOrders.Cache.RaiseExceptionHandling<
                            RSSVWorkOrder.priority>(row, originalRow.Priority,
                            new PXSetPropertyException(row, Messages.PriorityTooLow));
                        //Assign the proper priority
                        e.NewRow.Priority = WorkOrderPriorityConstants.Medium;
                    }
                }
            }
        }
3. Build the project.
4. In the RS301000.ts file, make sure that the PXFieldOptions.CommitChanges option is specified
for the property type of the Priority field of the RSSVWorkOrder view class. Also, verify that the
ServiceID field has the same setting.
5. Publish the customization project.


Part 1: Data Entry Form (Repair Work Orders) | 49
Testing the Logic
To check the validation, on the Repair Work Orders (RS301000) form, do the following:
1. Select the work order with the 000001 order number.
2. In the Priority box, select Low.
3. In the Service box, select Liquid Damage, which requires a preliminary check. Make sure the error is
displayed, as shown below.
Figure: The error on the page
4. Click Cancel on the form toolbar and Confirm on the Confirm Action dialog box, which opens. The changes
are discarded and the error is no longer displayed.
5. In the Service box, select Liquid Damage without changing the priority.
6. Save your changes. The repair work order is saved without errors.
7. In the Service box, select Battery Replacement, and in the Priority box, select Low.
8. Save your changes. The repair work order is saved without errors.
Related Links
•
Data Validation: Validation of a Data Record
•
PXStringListAttribute
•
PXCache.RaiseExceptionHandling Method
•
PXCache.ObjectsEqual Method


Part 2: Setup Form (Repair Work Order Preferences) | 50
Part 2: Setup Form (Repair Work Order Preferences)
In Acumatica ERP, administrators use setup forms to provide configuration parameters for the application, most
commonly in the beginning of the functional area’s implementation and use. The configuration parameters in a
setup form are stored in a single record in the corresponding setup table of the database. By using a setup form,
an administrator can edit this record—for example, turn on or oﬀ particular functionality, specify settings that
determine default system behavior, and specify the numbering settings to be used to number documents of
particular types.
In this part of the course, you’ll create the Repair Work Order Preferences (RS101000) setup form, which is
described in Company Story and Customization Description.
The names of setup forms start with a two-letter abbreviation (indicating the functional area of the form) followed
by 10 and then four additional digits. For example, RS101000 will be the ID of the Repair Work Order Preferences
form.
The names of the graphs for setup forms have the Maint suﬀix (as maintenance forms do).
Lesson 2.1: Configuring the Auto-Numbering of a Field Value
In this lesson, you will create a setup form—Repair Work Order Preferences (RS101000). In the process, you’ll learn
how to configure the auto-numbering of a field value on a data entry form by using the AutoNumber attribute
defined in the PX.Objects.CS namespace. This setup will enable the automatic insertion of repair work order
numbers on the Repair Work Orders (RS301000) form.
The Form Elements
The setup form will contain the following boxes (shown below):
•
Numbering Sequence: The numbering sequence that the system will use to auto-number repair work order
records. By default, the value will be set to RSSVWORDER; this numbering sequence has been preconfigured
for this course on the Numbering Sequences (CS201010) form.
•
Walk-In Customer: The customer ID that will be inserted by default into work orders for walk-in repair
services—that is, repair services that have the Walk-In Service check box selected on the Repair Services
(RS201000) form. (You will not implement this logic in this training course.)
•
Default Employee: The default assignee to be inserted into repair work orders. (You won’t implement this
logic in this training course.)
•
Prepayment Percent: The percent of prepayment that a customer must pay for a service that requires
prepayment—that is, a service that has the Requires Prepayment check box selected on the Repair
Services (RS201000) form. (You won’t implement this logic in this training course.)
Figure: Repair Work Order Preferences form


Part 2: Setup Form (Repair Work Order Preferences) | 51
Configuration of Auto-Numbering
The PX.Objects.CS.AutoNumberAttribute attribute inserts a new number into each new document
created by a user before the record is saved to the database. This attribute is designed to use a numbering
sequence that has been defined on the Numbering Sequences (CS201010) form. You’ll configure the attribute to
retrieve the numbering sequence ID from the RSSVSetup table, which you’ve created in Initial Configuration.
The RSSVSetup DAC consists of data fields that represent configuration parameters and standard system fields.
The corresponding columns in the database should not be null.
The RSSVSetup class in the application does not contain a primary key field, because each setup form is
configured so that it always works with the only record retrieved from the corresponding setup table. However, to
support multitenant configurations, the setup table should contain the CompanyID column as the primary key in
the database. On the application level, this CompanyID field is handled automatically by Acumatica Framework.
For more information on the CompanyID column in the database, see Multitenancy Support (CompanyID,
CompanyMask).
Lesson Objectives
In this lesson, you will learn how to do the following:
•
Create and use setup forms where users enter the configuration settings of the application
•
Configure the auto-numbering of a field value
Step 2.1.1: Creating the Form (Self-Guided Exercise)
In this step, you will create the Repair Work Order Preferences (RS101000) form on your own. Although this is a self-
guided exercise, you can use the details and suggestions in this topic as you create the form. The creation of a form
is described in detail in the T200 Maintenance Forms training course.
Creating the Form and Graph
Create the form and graph as follows:
1. On the toolbar of the Screens page of the Customization Project Editor, click Create New Screen.
2. In the Create New Screen dialog box, which opens, specify the following values:
•
Screen ID: RS.10.10.00
•
Graph Name: RSSVSetupMaint
•
Graph Namespace: PhoneRepairShop
•
Page Title: Repair Work Order Preferences
•
Template: Form (FormView)
•
Create Modern UI Files: Selected
3. Move the generated RSSVSetupMaint graph to the extension library.
Generating and Configuring the DAC
Create and configure the RSSVSetup DAC as specified below:
1. In Code Editor, generate the RSSVSetup DAC and move it to the extension library.


Part 2: Setup Form (Repair Work Order Preferences) | 52
2. In Visual Studio, in the Helper\Messages.cs file, define the user-friendly name for the RSSVSetup
DAC as follows.
        public const string RSSVSetup = "Repair Work Order Preferences";
3. Specify the user-friendly name for the RSSVSetup DAC as follows.
    [PXCacheName(Messages.RSSVSetup)]
4. Define attributes for the system fields. (For details about the definition of the attributes of the system fields,
see the T200 Maintenance Forms training course or see Audit Fields, Concurrent Update Control (TStamp), and
Attachment of Additional Objects to Data Records (NoteID) in the documentation.)
5. Define the attributes for the WalkInCustomerID field as shown below.
        #region WalkInCustomerID
        [CustomerActive(DisplayName = "Walk-In Customer",
            DescriptionField = typeof(Customer.acctName))]
        [PXDefault]
        public virtual int? WalkInCustomerID { get; set; }
        public abstract class walkInCustomerID :
            PX.Data.BQL.BqlInt.Field<walkInCustomerID> { }
        #endregion
The CustomerActive attribute is defined in the PX.Objects.AR namespace.
6. Define the attributes for the DefaultEmployee field as follows.
        #region DefaultEmployee
        [Owner(DisplayName = "Default Employee")]
        [PXDefault]
        public virtual int? DefaultEmployee { get; set; }
        public abstract class defaultEmployee :
            PX.Data.BQL.BqlInt.Field<defaultEmployee> { }
        #endregion
The Owner attribute is defined in the PX.TM namespace.
7. Define the attributes for the PrepaymentPercent field as shown in the following code.
        #region PrepaymentPercent
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Prepayment Percent", Required = true)]
        public virtual Decimal? PrepaymentPercent { get; set; }
        public abstract class prepaymentPercent :
            PX.Data.BQL.BqlDecimal.Field<prepaymentPercent> { }
        #endregion
You will configure the attributes of the NumberingId field and the attributes of the DAC
further in this lesson.
Configuring the Graph
Configure the RSSVWorkOrderEntry graph as follows:


Part 2: Setup Form (Repair Work Order Preferences) | 53
1. Define a data view in the generated RSSVSetupMaint graph and make the Save and Cancel standard
system buttons available on the form toolbar as the following code shows.
    public class RSSVSetupMaint : PXGraph<RSSVSetupMaint>
    {
        public PXSave<RSSVSetup> Save = null!;
        public PXCancel<RSSVSetup> Cancel = null!;
        public SelectFrom<RSSVSetup>.View Setup = null!;
        //Automatically generated data views
    }
You need to keep the automatically generated data views until the customization project
contains the ASPX code of the Classic UI, which uses these data views.
2. Build the project in Visual Studio and publish the customization project.
Including Access Rights and Category in the Customization Project
1. On the Access Rights by Screen (SM201020) form, make sure Delete access rights for the Repair Work Order
Preferences (RS101000) form are provided for the Customizer user role.
2. Make sure the access rights for the new form are included in the customization project.
3. Include a link to the form in the Preferences category of the Phone Repair Shop workspace.
4. Update the SiteMapNode item for the Repair Work Order Preferences form in the customization project.
UI of a Setup Form: General Information
On a setup form, an administrator provides configuration or maintenance settings for some functionality in the
instance. In the Acumatica ERP workspaces, setup forms are typically listed under Preferences. This topic provides
recommendations and guidelines on organizing the layout of a setup form.
The following screenshot shows an example of a setup form with tabs.


Part 2: Setup Form (Repair Work Order Preferences) | 54
Figure: A setup form with tabs
Applicable Scenarios
You configure the setup form in the following cases:
•
You are migrating an existing setup form to the Modern UI.
•
You are creating a new setup form by using the Modern UI.
Template and Label Sizes
The recommended template for a setup form is 1-1. For details about the available templates, see Form Layout:
Predefined Templates.
The recommended size for labels on setup forms is xm.
Recommendations for Organizing the Layout
The following table shows recommendations for organizing the layout of a setup form.
Correct
Incorrect
When most labels are long, make all labels long (with class="label-size-<SIZE>" in qp-template).
They should be as long as is needed to make most labels visible without a user needing to hover over the label to
see the full name.


Part 2: Setup Form (Repair Work Order Preferences) | 55
Correct
Incorrect
 
 
UX and Functional Design Guidelines
The form design should be tailored for screens with a resolution of 1280 x 720.
The number of the setup form should start with 10. For details, see Form and Report Numbering.
Related Links
•
Creating Setup Forms
•
Form Types
•
Form and Report Numbering
UI of a Setup Form: A Form with Only a Summary Area
The following topic describes how to configure a form that contains only a Summary area. (In the Classic UI, these
forms were based on the FormView template.) The following screenshot shows an example of a form with this
layout.


Part 2: Setup Form (Repair Work Order Preferences) | 56
Figure: A form with only a Summary area
View Definition in TypeScript
To configure a form with only a Summary area in the TypeScript file, you do the following:
•
For the view of the Summary area, you use the createSingle method.
•
You use a class that extends PXView with the full list of the fields of the Summary area.
The following code shows an example of the TypeScript configuration for the form in the screenshot above.
import {
    PXScreen, createSingle, graphInfo, PXView,
    PXFieldState, PXFieldOptions, controlConfig
} from 'client-controls';
 
export class FASetup extends PXView {
    FAAccrualAcctID: PXFieldState<PXFieldOptions.CommitChanges>;
    FAAccrualSubID: PXFieldState<PXFieldOptions.CommitChanges>;
    ProceedsAcctID: PXFieldState<PXFieldOptions.CommitChanges>;
    ProceedsSubID: PXFieldState<PXFieldOptions.CommitChanges>;
    DeprHistoryView: PXFieldState;
    DepreciateInDisposalPeriod: PXFieldState;
    AccurateDepreciation: PXFieldState;
    ReconcileBeforeDisposal: PXFieldState;
    AllowEditPredefinedDeprMethod: PXFieldState;
    @controlConfig({allowEdit: true, }) 
    RegisterNumberingID: PXFieldState;
    @controlConfig({allowEdit: true, }) 
    AssetNumberingID: PXFieldState;
    @controlConfig({allowEdit: true, }) 
    BatchNumberingID: PXFieldState;


Part 2: Setup Form (Repair Work Order Preferences) | 57
    @controlConfig({allowEdit: true, }) 
    TagNumberingID: PXFieldState;
    CopyTagFromAssetID: PXFieldState;
    AutoReleaseAsset: PXFieldState;
    AutoReleaseDepr: PXFieldState;
    AutoReleaseDisp: PXFieldState;
    AutoReleaseTransfer: PXFieldState;
    AutoReleaseReversal: PXFieldState;
    AutoReleaseSplit: PXFieldState;
    UpdateGL: PXFieldState;
    AutoPost: PXFieldState;
    SummPost: PXFieldState;
    SummPostDepreciation: PXFieldState;
}
 
@graphInfo({ 
    graphType: 'PX.Objects.FA.SetupMaint', 
    primaryView: 'FASetupRecord' 
})
export class FA101000 extends PXScreen {
    FASetupRecord = createSingle(FASetup);
}
Layout in HTML
You define the layout of a setup form with only a Summary area by adding one qp-template control to the HTML
code of the form with slots for each column. For details on the available templates, see Form Layout: Predefined
Templates.
The following code shows the HTML template for the form in the screenshot above.
<template>
    <qp-template name="1-1" id="formDocument" wg-container="FASetupRecord_form">
        <div slot="A">
            <qp-fieldset id="groupAccountSettings" view.bind="FASetupRecord" 
                caption="Account Settings">
                <field name="FAAccrualAcctID"></field>
                <field name="FAAccrualSubID"></field>
                <field name="ProceedsAcctID"></field>
                <field name="ProceedsSubID"></field>
            </qp-fieldset>
            <qp-fieldset id="groupOther" view.bind="FASetupRecord" caption="Other">
                <field name="DeprHistoryView"></field>
                <field name="DepreciateInDisposalPeriod"></field>
                <field name="AccurateDepreciation"></field>
                <field name="ReconcileBeforeDisposal"></field>
                <field name="AllowEditPredefinedDeprMethod"></field>
            </qp-fieldset>
        </div>
        <div slot="B">
            <qp-fieldset id="groupNumberingSettings" view.bind="FASetupRecord" 
                caption="Numbering Settings">
                <field name="RegisterNumberingID"></field>
                <field name="AssetNumberingID"></field>
                <field name="BatchNumberingID"></field>
                <field name="TagNumberingID"></field>
                <field name="CopyTagFromAssetID"></field>


Part 2: Setup Form (Repair Work Order Preferences) | 58
            </qp-fieldset>
            <qp-fieldset id="groupPostingSettings" view.bind="FASetupRecord"
                caption="Posting Settings" class="no-label">
                <field name="AutoReleaseAsset"></field>
                <field name="AutoReleaseDepr"></field>
                <field name="AutoReleaseDisp"></field>
                <field name="AutoReleaseTransfer"></field>
                <field name="AutoReleaseReversal"></field>
                <field name="AutoReleaseSplit"></field>
                <field name="UpdateGL"></field>
                <field name="AutoPost"></field>
                <field name="SummPost"></field>
                <field name="SummPostDepreciation"></field>
            </qp-fieldset>
        </div>
    </qp-template>
</template>
Step 2.1.2: Creating the Form’s UI
In this step, you will create the UI of the Repair Work Order Preferences (RS101000) form.
Defining the Screen Class in TypeScript
You can open a TypeScript file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)
To define the view of the Repair Work Order Preferences (RS101000) setup form in TypeScript, you define a screen
class and a property for the data view of the form. Do the following:
1. In the RS101000.ts file, add the import directives as follows.
import {
 PXScreen,
 createSingle,
 graphInfo,
} from "client-controls";
2. Define the screen class for the form as follows. The class name is the ID of the form.
export class RS101000 extends PXScreen {}
3. For the screen class, add the graphInfo decorator, and specify the graph and the primary view of the form
in the decorator properties, as shown in the following code.
@graphInfo({
 graphType: "PhoneRepairShop.RSSVSetupMaint",
 primaryView: "Setup",
})
export class RS101000 extends PXScreen {}


Part 2: Setup Form (Repair Work Order Preferences) | 59
4. Define the property for the data view of the form by using the following code. Because the data view is used
to display only a Summary area, you need to initialize the property with the createSingle method. The
input parameter of this method is an instance of the view class that you will define in the next step.
export class RS101000 extends PXScreen {
 Setup = createSingle(RSSVSetup);
}
Defining the View Class in TypeScript
You need to define a view class for the single data view of the Repair Work Order Preferences (RS101000) setup
form. Proceed as follows:
1. In the RS101000.ts file, update the list of import directives, as the following code shows.
import {
 PXScreen,
 createSingle,
 graphInfo,
 PXView,
 PXFieldState,
 controlConfig,
} from "client-controls";
2. Define the view class as follows.
export class RSSVSetup extends PXView {}
3. In the view class, specify the properties for all data fields of the data view, as shown below. You use the
name of the data field as the property name.
export class RSSVSetup extends PXView {
 @controlConfig({allowEdit: true, })
 NumberingID: PXFieldState;
 @controlConfig({allowEdit: true, })
 WalkInCustomerID: PXFieldState;
 DefaultEmployee: PXFieldState;
 PrepaymentPercent: PXFieldState;
}
You have used the controlConfig decorator to display the values in the Numbering Sequence and
Walk-In Customer boxes as links to the records whose identifiers are displayed in the selector control.
4. Save your changes.
Defining the Layout in HTML
You can open an HTML file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)


Part 2: Setup Form (Repair Work Order Preferences) | 60
The Repair Work Order Preferences (RS101000) setup form contains only a Summary area with a single column.
Therefore, to define the layout of the form, you need to add one qp-template element in the HTML file. You also
need to add one qp-fieldset element to group the fields of the column in a slot for the column, as the following
code shows.
<template>
  <qp-template id="form-RepairWorkOrderPreferences" name="1-1"
    class="label-size-xm">
    <qp-fieldset id="fsPreferences-RepairWorkOrder" slot="A" view.bind="Setup">
      <field name="NumberingID"></field>
      <field name="WalkInCustomerID"></field>
      <field name="DefaultEmployee"></field>
      <field name="PrepaymentPercent"></field>
    </qp-fieldset>
  </qp-template>
</template>
You have specified an ID for the qp-fieldset control and bound the control to the Setup view, which you have
defined in the RS101000.ts file.
You have used the following recommended settings:
•
The 1-1 template for the form
•
The label-size-xm class for the qp-template element
Building and Viewing the Form
To build the source files for the Repair Work Order Preferences (RS101000) and view its Modern UI version,
either update the files in the customization project and publish it, or run the following command in the
FrontendSources\screen folder of your instance.
npm run build-dev --- --env screenIds=RS101000
Step 2.1.3: Configuring the DAC for the Setup Form (with PXPrimaryGraph and
PXCacheName)
In this step, you’ll configure the RSSVSetup DAC with the PXPrimaryGraph and PXCacheName attributes.
You’ll need these attributes to create the link to the setup form; this link is displayed when no setup data record
exists (see below).
You use the PXPrimaryGraph attribute to specify the graph that corresponds to the default editing form for
records of the DAC. The attribute enables links to the Repair Work Order Preferences form from other forms.
You use the PXCacheName attribute to specify a user-friendly name for a DAC. This name will be used in an error
message that is displayed when no setup data records exist. Without the PXCacheName attribute, the error
message would use the DAC name, RSSVSetup, for the link.


Part 2: Setup Form (Repair Work Order Preferences) | 61
Figure: The form when the setup data has not been specified
Configuring the RSSVSetup DAC
Perform the following instructions to configure the RSSVSetup DAC:
1. In the RSSVSetup.cs file, add the PXPrimaryGraph attribute to the RSSVSetup DAC as shown in the
following code.
    [PXCacheName(Messages.RSSVSetup)]
    [PXPrimaryGraph(typeof(RSSVSetupMaint))]
    public class RSSVSetup : PXBqlTable, IBqlTable
2. Build the project in Visual Studio and publish the customization project.
Related Links
•
PXPrimaryGraphAttribute
•
Configuration Parameters of the Application (Setup Forms)
Step 2.1.4: Configuring the Auto-Numbering of Records (with
CS.AutoNumberAttribute)
In this step, you will configure the auto-numbering of repair work orders.
Configuration of the Numbering Sequence Box on the Setup Form
The numbering sequences for the auto-numbering of records are defined on the Numbering Sequences (CS201000)
form. This form displays the data of the Numbering DAC from the PX.Objects.CS namespace. You’ll use this
DAC to configure the selector for the Numbering Sequence box on the Repair Work Order Preferences (RS101000)
form.
For the auto-numbering of repair work orders, you will use the RSSVWORDER numbering sequence, which has been
preconfigured for this course. You will specify this numbering sequence as the default value for the Numbering
Sequence box. In the previous step, you’ve used the controlConfig decorator to display the value in the
Numbering Sequence box as a link. When a user clicks this link, the system opens the Numbering Sequences form,
where the user can view and possibly edit the settings of the numbering sequence.
The Attribute for Auto-Numbering Repair Work Orders
You will assign the AutoNumber attribute from PX.Objects.CS to the OrderNbr field of the
RSSVWorkOrder DAC. In the first parameter of the attribute constructor, you will pass the numbering sequence
that should be used to auto-number work orders.


Part 2: Setup Form (Repair Work Order Preferences) | 62
Acumatica ERP includes a number of attributes derived from
PX.Objects.CS.AutoNumberAttribute. In your application, you can use a predefined
attribute that suits your needs or implement your own attribute, as described in Custom Attributes.
Changes in the Graph of the Data Entry Form
To make the RSSVWorkOrderEntry graph use the numbering sequence specified on the Repair Work Order
Preferences (RS101000) form, you will add a data view of the PXSetup type and the graph constructor to retrieve
setup data from the database. If the current record in this view is null, the server returns the specific error with the
link to the setup form. (Acumatica Framework defines the form based on the PXPrimaryGraph attribute on the
RSSVSetup DAC.)
Instead of checking the current record in the graph constructor, you can assign the
PXCheckCurrent attribute to the data view of the PXSetup type in the graph.
Setup of the Auto-Numbering of Repair Work Orders
To set up the automatic numbering of repair work orders, complete the following instructions:
1. In the RSSVSetup.cs file, define the attributes of the NumberingID field as follows:
a. Specify the PXDBString and PXUIField attributes as follows.
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Numbering Sequence")]
b. Add the using PX.Objects.CS; directive and define the selector for the field as shown in the
following code.
        [PXSelector(typeof(Numbering.numberingID),
            DescriptionField = typeof(Numbering.descr))]
c. Specify the RSSVWORDER numbering sequence as the default value for the field, as shown in the
following code.
        [PXDefault("RSSVWORDER")]
        public virtual string? NumberingID { get; set; }
        public abstract class numberingID : 
            PX.Data.BQL.BqlString.Field<numberingID> { }
2. In the RSSVWorkOrder.cs file, add the using PX.Objects.CS; directive and specify the
AutoNumber attribute for the OrderNbr field, as shown in the following code.
        [AutoNumber(typeof(RSSVSetup.numberingID),
            typeof(RSSVWorkOrder.dateCreated))]
3. In the RSSVWorkOrderEntry graph, add the AutoNumSetup data view and the constructor as follows.
        #region Views
        ...
        //The view for the auto-numbering of records
        public PXSetup<RSSVSetup> AutoNumSetup = null!;
        #endregion
        #region Graph constructor
        public RSSVWorkOrderEntry()


Part 2: Setup Form (Repair Work Order Preferences) | 63
        {
            RSSVSetup setup = AutoNumSetup.Current;
        }
        #endregion
4. Build the project.
5. Publish the customization project.
Testing of the Auto-Numbering
Do the following to test the auto-numbering:
1. Open the Repair Work Orders (RS301000) form and see the error displayed on the form, as shown below.
Figure: The error
2. Click the link to open the Repair Work Order Preferences (RS101000) form.
3. On the form, make sure the RSSVWORDER numbering sequence is selected in the Numbering Sequence
box, and click it.
4. On the Numbering Sequences (CS201000) form, which opens for the RSSVWORDER numbering sequence, do
the following:
a. Type 000001 in the Last Number column. (You have already manually created a work order with number
000001.)
b. Make sure the other settings of the sequence are the same as those shown below.


Part 2: Setup Form (Repair Work Order Preferences) | 64
Figure: The RSSVWORDER numbering sequence
c. Save your changes.
5. Close the Numbering Sequences form.
6. On the Repair Work Order Preferences form, specify the following settings and save your changes:
•
Walk-in Customer: C000000001
•
Default Employee: Becher, Joseph
•
Prepayment Percent: 10
7. Open the Repair Work Orders form. Now the error is no longer displayed for the form, and you can create a
new record.
8. On the form toolbar, click Add New Record. Notice that the <NEW> placeholder appears in the Order Nbr.
box.
9. Specify the following settings for the new work order:
•
Customer ID: C000000001
•
Service: Screen Repair
•
Device: iPhone 6
•
Description: Screen repair, iPhone 6
10.Save your changes. Make sure the new repair work order has the 000002 order number assigned to it, as
shown below.


Part 2: Setup Form (Repair Work Order Preferences) | 65
Figure: A work order with an automatically assigned number
Related Links
•
Configuration Parameters of the Application (Setup Forms)
Additional Information: Custom Feature Switches
In this training course, you have created two custom forms. You may want to make custom forms available for a
user only if a custom feature is enabled on the Enable/Disable Features (CS100000) form.
The creation of custom feature switches is outside of the scope of this course, but if you need details about this
functionality, see To Add a Custom Feature Switch.


Additional Information | 66
Additional Information
In the following topics, you can find additional information related to the initial configuration of the instance you’ll
need to complete the activities of this course. You can also find a list of the scenarios in which particular event
handlers have been used in this course.
Appendix A: Initial Configuration
If you cannot complete the instructions in Step 2: Preparing an Acumatica ERP Instance for the Training Course, you
can create an Acumatica ERP instance and manually publish the needed customization project. This topic walks
you through this initial configuration.
Step 1: Deploying the Needed Acumatica ERP Instance for the Training Course
You deploy and configure an Acumatica ERP instance as follows:
1. To deploy a new application instance, open the Acumatica ERP Configuration wizard and do the following:
a. On the Database Configuration page, type the name of the database: SmartFix_T220.
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
Load the customization project with the results of the T210 Customized Forms and Master-Details Relationships
training course and publish this project as follows:


Additional Information | 67
1. On the Customization Projects (SM204505) form, create a project with the name PhoneRepairShop, and
open it.
2. In the menu of the Customization Project Editor, click Source Control > Open Project from Folder.
3. In the dialog box that opens, specify the path to the Customization\T210\PhoneRepairShop folder,
which you’ve downloaded from Acumatica GitHub, and click OK.
4. Bind the customization project to the source code of the extension library as follows:
a. Copy the Customization\T210\PhoneRepairShop_Code folder to the App_Data\Projects
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
5. In the menu of the Customization Project Editor, click Publish > Publish Current Project.
The Modified Files Detected dialog box opens before publication because you’ve rebuilt
the extension library in the PhoneRepairShop_Code Visual Studio project. The Bin
\PhoneRepairShop_Code.dll file has been modified, and you need to update it in the
project before the publication of the project.
The published customization project contains all changes to the Acumatica ERP website and database that have
been performed in the T200 Maintenance Forms and T210 Customized Forms and Master-Details Relationships
training courses. This project also contains the customization plug-ins that fill in the tables created in the T200
Maintenance Forms and T210 Customized Forms and Master-Details Relationships training courses with the custom
data entered in these training courses. For details about the customization plug-ins, see To Add a Customization
Plug-In to a Project. The creation of customization plug-ins is outside of the scope of this course.
Appendix B: Use of Event Handlers
This topic lists the scenarios in which particular event handlers have been used in this course.
Table: Use of Event Handlers
Event
Scenario
Examples in the Guide
FieldDefault-
ing
Set a default value of a field depending on
other field values
Step 1.2.2: Updating Fields of a Record on
Update of Its Field (with FieldUpdated and
FieldDefaulting)—Self-Guided Exercise


Additional Information | 68
Event
Scenario
Examples in the Guide
FieldUpdated
Update of a field of a data record when an-
other field of this record is updated
Step 1.2.2: Updating Fields of a Record on
Update of Its Field (with FieldUpdated and
FieldDefaulting)—Self-Guided Exercise
FieldVerifying
Validation of an independent field value
Step 1.3.1: Validating an Independent Field
Value (with FieldVerifying)
RowUpdated
Insertion of detail lines when particular
fields of the master record are updated
Step 1.2.1: Adding Default Rows to Work Or-
ders (with RowUpdated)
RowUpdating
Validation of a field value that depends on
another field of the same record
Step 1.3.2: Validating Dependent Fields of
Records (with RowUpdating)
