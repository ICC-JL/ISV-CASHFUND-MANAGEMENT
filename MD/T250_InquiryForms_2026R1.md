Developer Course
Customization
T250 Inquiry Forms
2026 R1
Revision: 4/15/2026


Contents | 2
Contents
Copyright...............................................................................................................................................3
How to Use This Course.......................................................................................................................... 4
Company Story and Customization Description........................................................................................ 6
Initial Configuration............................................................................................................................... 9
To Deploy an Instance for the Training Course................................................................................................9
Part 1: The Open Payment Summary Form............................................................................................. 11
Lesson 1.1: Configure the Inquiry Form......................................................................................................... 11
Inquiry Forms: General Information......................................................................................................11
Activity 1.1.1: To Set Up an Inquiry Form..............................................................................................12
Activity 1.1.2: To Create the UI of an Inquiry Form with Only a Grid................................................... 16
UI Definition in HTML and TypeScript: Joined Fields...........................................................................22
Lesson 1.2: Configure a Filter for the Inquiry Form.......................................................................................23
Filtering Parameters: General Information........................................................................................... 23
Activity 1.2.1: To Add a Filter for an Inquiry Form................................................................................ 25
Activity 1.2.2: To Display the Filter Values in the URL.......................................................................... 28
Lesson 1.3: Dynamically Add Filtering Conditions.........................................................................................30
Data View Delegates: General Information........................................................................................... 30
Activity 1.3.1: To Add a Filtering Query Dynamically............................................................................32
Lesson 1.4: Retrieve Aggregated Data............................................................................................................ 37
Data Aggregation: General Information................................................................................................ 37
Activity 1.4.1: To Retrieve Aggregated Data.......................................................................................... 37
Lesson 1.5: Add Redirection Links to the Grid............................................................................................... 41
Redirection to Webpages: General Information................................................................................... 41
Selector Control: Configuration of a Link............................................................................................. 43
Activity 1.5.1: To Add Redirection Links to the Grid by Using the PXSelector Attribute...................... 44
Activity 1.5.2: To Add Redirection Links to a Grid by Using an Action................................................. 46
Part 2: The Payment Info Tab................................................................................................................ 49
Lesson 2.1: Add the Payment Info Tab........................................................................................................... 49
Use of PXProjection: General Information............................................................................................ 49
Activity 2.1.1: To Display Multiple DAC Data on a Tab.......................................................................... 51
Appendix: Initial Configuration..............................................................................................................56


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
Last Updated: 04/15/2026


How to Use This Course | 4
How to Use This Course
The T250 Inquiry Forms training course shows you how to create inquiry forms by using Acumatica Framework and
the customization tools of Acumatica ERP. Inquiry forms, like reports, show data narrowed by the selection criteria
users specify. But inquiry forms and reports diﬀer in key regards:
•
Reports are generated in a printable and easy-to-share format. They’ve been designed to show the data in a
uniform, polished way.
•
Inquiry forms, designed for online viewing, display the data in a table. To further tailor the data they’re
viewing, users can dynamically alter the selection criteria and configure the table.
This course is intended for application developers who are starting to learn how to customize Acumatica ERP.
The course is based on a set of examples that demonstrate the general approach to customizing Acumatica ERP.
It’s designed to give you ideas about how to develop your own embedded applications through the customization
tools. As you go through the course, you’ll continue the development of the customization for the cell phone repair
shop, which was performed in the previous training courses of the T series. (We recommend that you take these
courses before completing the current course).
Aer you complete all the lessons of the course, you’ll be familiar with the programming techniques that are used
to define the Acumatica ERP inquiry forms.
We recommend that you complete the examples in the presented order because some examples use
the results of previous ones.
What the Course Prerequisites Are
To complete this course, you should be familiar with the basic concepts of the Acumatica Framework and the
Acumatica Customization Platform. Before you begin this course, we recommend that you complete the following
training courses:
•
T200 Maintenance Forms
•
T210 Customized Forms and Master-Details Relationships
•
T220 Data Entry and Setup Forms
•
T230 Actions
•
T240 Processing Forms
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


How to Use This Course | 5
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
•
Configuring and deploying ASP.NET websites
•
Configuring and securing IIS
What Is in a Part
The first part of the course explains how to create an inquiry form with diﬀerent kinds of filtering.
The second part of the course explains how to display information from diﬀerent DACs on a single tab by using the
PXProjection attribute.
Each part of the course consists of lessons you should complete.
What Is in a Lesson
Each lesson focuses on a particular development scenario that you can implement by using Acumatica ERP
customization tools and the Acumatica Framework. Each lesson consists of a brief description of the scenario and
an example of its implementation.
Where the Source Code Is
You can find the source code of the customization described in this course and code snippets for the course in the
Customization\T250 folder of the Help-and-Training-Examples repository in Acumatica GitHub.
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
In this course, you will continue the development to support the cell phone repair shop of the Smart Fix company;
you began and expanded on this development while completing the previous training courses of the T series.
You will load and publish the customization project with the results of these courses in Initial
Configuration.
In the previous training courses of the T series, you have created the following custom forms:
•
The Repair Services (RS201000) maintenance form, which the Smart Fix company uses to manage the lists of
repair services that the company provides
•
The Serviced Devices (RS202000) maintenance form, which the Smart Fix company uses to manage the lists
of devices that can be serviced
•
The Services and Prices (RS203000) maintenance form, where users can define and maintain the price for
each provided repair service
•
The Repair Work Orders (RS301000) data entry form, where users create and manage individual work orders
for repairs
•
The Repair Work Order Preferences (RS101000) setup form, which an administrator uses to specify the
company's preferences for the repair work orders
You have also customized the Stock Items (IN202500) form of Acumatica ERP so that users can mark particular stock
items as repair items—that is, items that are used for repair services.
In this course, you will create the Open Payment Summary (RS401000) custom inquiry form. On this form, users
will view all repair work orders and sales orders that haven’t yet been paid in full, along with information about the
invoices that have been created for these orders. You will implement the functionality of the form in stages. First,
you will implement this form as an inquiry form without filters. Then you’ll add filtering by using a data view and a
data view delegate. You will also learn how to aggregate data in a data view. Finally, you will add redirection links in
diﬀerent columns of the inquiry form.
You’ll implement a new tab on the Repair Work Orders form that will be shown only if the selected order has been
paid in full; this tab will display information about the invoice and the last payment for the repair work order.
Open Payment Summary Form
Below you can see how the Open Payment Summary (RS401000) form will look once you have completed the
course.


Company Story and Customization Description | 7
Figure: Open Payment Summary form
The form will contain the following parts:
•
The Selection area with these elements:
•
Customer ID box: Can be used to filter the listed repair work orders and sales orders by customer
•
Service box: Can be used to filter the listed repair work orders by service
•
Show Unpaid Subtotals check box: Indicates whether subtotals for all orders with the same status
should be displayed in the Balance column, along with individual balances.
•
The table, which displays the list of repair work orders and sales orders that have not yet been paid in full.
Payment Info Tab
Below you can see how the Payment Info tab of the Repair Work Orders (RS301000) data entry form will look at the
end of the course.


Company Story and Customization Description | 8
Figure: Payment Info tab
The tab will contain the following boxes:
•
Invoice Nbr.: The number of the invoice created for the order
•
Due Date: The due date of the invoice
•
Latest Payment: The number of the payment that was the most recent payment applied to the invoice
•
Latest Amount Paid: The amount of the most recently applied payment


Initial Configuration | 9
Initial Configuration
You need to perform prerequisite actions before you start to complete the course.
To Deploy an Instance for the Training Course
The following activity will walk you through the process of preparing and deploying an Acumatica ERP instance that
you can use to perform the steps in the lessons of this training course.
Story
You need to deploy an instance of Acumatica ERP with the PhoneRepairShop customization project published and
then configure the instance.
Process Overview
In this activity, you’ll prepare the environment and install tools that will help you to perform customization tasks.
You will then deploy the instance of Acumatica ERP with the PhoneRepairShop customization project published and
the dataset from the T270 Workflow API course. Finally, in Acumatica ERP, you’ll configure the instance that you’ve
deployed.
Step 1: Preparing the Environment
If you’ve completed any training course of the T series and are using the same environment for the
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
To perform customization tasks, you need to deploy an instance of Acumatica ERP for the T250 Inquiry Forms
training course in the instance.


Initial Configuration | 10
You deploy an Acumatica ERP instance and configure it as follows:
1. Open the Acumatica ERP Configuration wizard, and do the following:
a. Click Deploy a New Acumatica ERP Instance for T-Series Developer Courses.
b. On the Instance Configuration page, do the following:
a. In the Training Course box, select T250 Inquiry Forms.
b. In the Local Path to the Instance box, select a folder that’s outside of the C:\Program Files
(x86), C:\Program Files, and C:\Users folders. (We recommend that you store the website
folder outside of these folders to avoid an issue with permission to work in these folders when you
customize the website.)
c. On the Database Configuration page, make sure the name of the database is SmartFix_T250.
d. On the Website Configuration page, make sure the Install Node.js and Use Modern UI as Default check
boxes are selected.
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


Part 1: The Open Payment Summary Form | 11
Part 1: The Open Payment Summary Form
In this part, you will first learn how to create an inquiry form without filtering parameters. You’ll then learn
how to modify an inquiry form so that it has filtering parameters, add a filter dynamically in code, and add the
functionality to view aggregated data on the inquiry form. Finally, you will learn how to add redirection links to a
form.
Lesson 1.1: Configure the Inquiry Form
In this lesson, you will learn how to create an inquiry form without filtering parameters. To learn about this,
you’ll begin to develop the Open Payment Summary (RS401000) custom inquiry form, which will initially display
information about invoices and payments for repair work orders that have been currently not paid in full.
Inquiry Forms: General Information
On an inquiry form, you can view data narrowed by the selection criteria that you’ve specified. These forms are
similar to reports but designed for the flexible analysis of data online rather than for printing.
You can create an inquiry form from scratch or based on an existing inquiry form that’s available out of the box with
Acumatica ERP, such as the Account Summary (GL401000) form and the Account Details (GL404000) form.
Also, you can create an inquiry form in a customization project or directly in Acumatica ERP by using the Generic
Inquiry (SM208000) form. For more information on generic inquiry forms, see Managing Generic Inquiries.
Learning Objectives
In this lesson, you’ll learn how to do the following:
•
Create an inquiry form without any custom filtering parameters on the Selection area of the form
•
Define the DAC for the grid view of the inquiry form
•
Define the data view of the inquiry form
Applicable Scenarios
You develop an inquiry form without filtering parameters in the following cases:
•
You want to be able to view records from a single entity or multiple entities in the same table on the form
•
You want to view data narrowed down by reusable filters without specifying any custom filtering parameters
on the Selection area of the form
•
You want to be able to flexibly analyze data online without having to print a report
Conventions of Inquiry Forms
The IDs of inquiry forms follow this format:
•
A two-letter abbreviation indicating the functional area. For example, RS indicates repair services.
•
40, which is used system-wide for inquiry forms
•
Four additional numbers. For instance, RS401000 indicates an inquiry form in the repair services functional
area.


Part 1: The Open Payment Summary Form | 12
The names of the graphs for inquiry forms have the Inq suﬀix.
All inquiry forms have a table (grid), and some have a Selection area with elements for filtering the data. In some
cases, you don’t need to give users the ability to specify any custom selection criteria, so you don’t need to define
any custom filtering parameters.
Because users don’t edit any records on an inquiry form, you use the ReadOnly view type when defining the
data view for the grid, which defines the selection of records in read-only mode. In the UI, Acumatica Framework
automatically disables the editing of data records that were retrieved through a read-only data view.
DAC for the Grid View of an Inquiry Form
When defining the DAC for the grid view of an inquiry form, you should derive a new DAC from the data entry form's
DAC (whose data is being displayed on the inquiry form) and extend the new class with additional DAC fields that
are specific to the inquiry form.
For the DAC fields that aren’t specific to the inquiry form but are defined in the data entry form's DAC, you’ll add
abstract classes with the new modifier in the derived DAC. This step is required because you’ll use the data fields
of the derived class in BQL statements (for example, in the data view of the inquiry form and in attributes). If you
don’t define the abstract classes for the original fields in the derived DAC, these fields will be referred to in the SQL
statement that corresponds to the BQL query as the fields of the original DAC. Data inconsistency issues can result
when the original and the derived DACs are used in the same BQL statement.
UI of an Inquiry Form
For the UI of an inquiry form without filtering parameters, you should follow similar guidelines to those for a similar
processing form in Processing Form: A Form with Only a Grid. The only diﬀerence is that you need to use the Inquiry
preset for the table on the form.
Reusable Filters on an Inquiry Form
You can create an inquiry form without any custom filtering parameters and give users the ability to define reusable
filters for the form’s table. You enable these reusable filters in the graph code by adding the PXFilterable
attribute to the data view that provides this table’s data. The attribute enables the Filter Settings button and the
filtering area for the table.
In the filtering area, a user can define and save filters and then use them every time they open the form. Reusable
filters are frequently used in the tables on inquiry and processing forms, so that users can customize these forms to
show the data that’s most relevant to their needs and responsibilities.
For more information on reusable filters, see Saving of Filters for Future Use and To Filter the Data in a Table.
For details about configuring custom filtering parameters on an inquiry form, see Filtering Parameters: General
Information.
Activity 1.1.1: To Set Up an Inquiry Form
This activity will walk you through the process of creating an inquiry form without any filtering parameters.
Story
Suppose that you need to create an inquiry form in the PhoneRepairShop customization project that will display a
table showing all repair work orders that have not yet been paid in full. Each row should show information about
the invoices that have been created for these orders.


Part 1: The Open Payment Summary Form | 13
Process Overview
In this activity, you will create the Open Payment Summary (RS401000) custom inquiry form and define and
configure its components by performing the following steps:
1. Creating the inquiry form
2. Defining the DAC for the grid view of the inquiry form
3. Calculating a value of a field in the RowSelecting event handler
4. Defining the data view for the inquiry form
Step 1: Creating the Form—Self-Guided Exercise
In this self-guided exercise, you will create the Open Payment Summary (RS401000) form on your own. Although
this is a self-guided exercise, you can use the details and suggestions in this topic as you create the form. (Form
creation is described in detail in the T200 Maintenance Forms training course.)
If you’re using the Customization Project Editor to complete the self-guided exercise, you can perform the following
instructions:
1. On the Customization Projects (SM204505) form, click the name of your customization project.
The Screens page of the Customization Project Editor opens.
2. On the page toolbar of the Screens page, click Create New Screen.
3. In the Create New Screen dialog box, which opens, specify the following settings:
•
Screen ID: RS.40.10.00
•
Graph Name: RSSVPaymentPlanInq
•
Graph Namespace: PhoneRepairShop
•
Page Title: Open Payment Summary
•
Template: FormGrid (FormDetail)
•
Create Modern UI Files: Selected
4. Move the generated RSSVPaymentPlanInq graph to the extension library.
•
Don’t make any standard system actions available.
•
Don’t define any data views. You’ll define the data view later in this activity.
5. Make sure that the RSSVWorkOrder DAC is defined in the PhoneRepairShop_Code Visual Studio
project.
Don’t define any new DACs; you will define a new DAC in the next step.
6. Build the project in Visual Studio.
7. Update the customization project with a new version of PhoneRepairShop_Code.dll, and publish the
customization project.
8. Add a link to the Open Payment Summary form to the Inquiries category of the Phone Repair Shop
workspace, and make it available in the workspace’s quick menu.
9. In the Customization Project Editor, update the SiteMapNode item for the Open Payment Summary form.


Part 1: The Open Payment Summary Form | 14
Step 2: Defining the DAC for the Grid View of the Form
The Open Payment Summary (RS401000) form displays information about repair work orders (including the details
of the invoice created for each order). All fields on this form are unbound, and you don’t need to work with the
fields on the Repair Work Orders (RS301000) form, which works with the RSSVWorkOrder DAC.
In this step, for the grid view of the Open Payment Summary form, you will derive the new
RSSVWorkOrderToPay class from RSSVWorkOrder and extend the new class with additional DAC fields
that are specific to the inquiry form. In the derived DAC, you’ll add the OrderNbr, InvoiceNbr, and Status
abstract classes (which are defined in the base RSSVWorkOrder DAC) with the new modifier. You need to define
new abstract classes because you’ll use the data fields of the derived class in BQL statements, such as the BQL
statements in the data view of a processing form and in attributes.
To define the RSSVWorkOrderToPay DAC, do the following:
1. In the Helper/Messages.cs file, add the RSSVWorkOrderToPay string to the Messages class, as
shown below. This message will be used in the PXCacheName attribute for the new DAC.
        public const string RSSVWorkOrderToPay = "Repair Work Order to Pay";
2. In the RSSVWorkOrder.cs file, declare the RSSVWorkOrderToPay DAC: Derive the
RSSVWorkOrderToPay class from RSSVWorkOrder, as shown below.
    [PXCacheName(Messages.RSSVWorkOrderToPay)]
    public class RSSVWorkOrderToPay : RSSVWorkOrder
    {
    }
3. In the RSSVWorkOrderToPay class, define the OrderNbr, InvoiceNbr, and Status abstract classes
with the new modifier, as shown below.
        #region InvoiceNbr
        public new abstract class invoiceNbr :
            PX.Data.BQL.BqlString.Field<invoiceNbr>
        { }
        #endregion
        #region Status
        public new abstract class status :
            PX.Data.BQL.BqlString.Field<status>
        { }
        #endregion
        #region OrderNbr
        public new abstract class orderNbr :
            PX.Data.BQL.BqlString.Field<orderNbr>
        { }
        #endregion
4. In the RSSVWorkOrderToPay class, define the PercentPaid field, as shown below.
        #region PercentPaid
        [PXDecimal]
        [PXUIField(DisplayName = "Percent Paid")]
        public virtual Decimal? PercentPaid { get; set; }
        public abstract class percentPaid :
            PX.Data.BQL.BqlDecimal.Field<percentPaid>
        { }


Part 1: The Open Payment Summary Form | 15
        #endregion
Step 3: Calculating the PercentPaid Field in RowSelecting
In the derived DAC, you’ve added the PercentPaid field. During the retrieval of each RSSVWorkOrder record,
the value of the PercentPaid field will be calculated from the database as the percentage of the invoice amount
that has been paid. Add this logic as follows:
1. In the RSSVPaymentPlanInq graph of the RSSVPaymentPlanInq.cs file, add the calculation of the
PercentPaid field value in the RowSelecting event, as shown in the following code.
        protected virtual void _(Events.RowSelecting<RSSVWorkOrderToPay> e)
        {
            if (e.Row == null) return;
            if (e.Row.OrderTotal == 0) return;
            RSSVWorkOrderToPay order = e.Row;
            var invoices = 
                SelectFrom<ARInvoice>.
                Where<ARInvoice.refNbr.IsEqual<@P.AsString>>.
                View.Select(this, order.InvoiceNbr);
            if (invoices.Count == 0)
                return;
            ARInvoice first = invoices[0];
            e.Row.PercentPaid = (order.OrderTotal - first.CuryDocBal) /
                order.OrderTotal * 100;
        }
In the event handler, you are selecting the invoice with the same number as the one specified in the repair
work order; you’re then calculating the percentage.
You need to use an event handler instead of attributes because you can’t check for values of 0 by using
attributes.
If you’ve generated the RSSVPaymentPlanInq graph from the Code Editor, you can remove
the Save and Cancel actions defined in the graph.
2. In the RSSVPaymentPlanInq.cs file, add the required using directives, which are shown in the
following code.
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Objects.AR;
3. Build the project.
Step 4: Defining the Data View of the Form
In this step, you will add the data view to the RSSVPaymentPlanInq graph, which works with the Open Payment
Summary (RS401000) form. In this data view, which provides data for the grid (table) of the inquiry form, you’ll
select only those repair work orders that are not yet paid and the invoices for these orders.
To define the data view of the form in the RSSVPaymentPlanInq graph, do the following:
1. In the RSSVPaymentPlanInq.cs graph, add the following member. (Replace the automatically
generated DetailsView member if you’ve used the Customization Project Editor to create the graph.)
        [PXFilterable]


Part 1: The Open Payment Summary Form | 16
        public 
            SelectFrom<RSSVWorkOrderToPay>.
              InnerJoin<ARInvoice>.On<ARInvoice.refNbr.
                IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
            Where<RSSVWorkOrderToPay.status.
                IsNotEqual<RSSVWorkOrderEntry_Workflow.States.paid>>.
            View.ReadOnly DetailsView = null!;
The InnerJoin clause adds information from the invoice that was created for the repair work order so
that you can display the invoice’s due date and balance on the page.
The Where clause excludes all orders with the Paid status from the results of the query.
Because users don’t need to edit any records on the inquiry form, you’ve used the ReadOnly view type,
which defines the selection of records in read-only mode. In the UI, Acumatica Framework automatically
disables the editing of data records that were retrieved through a read-only data view.
2. If you’ve generated the RSSVPaymentPlanInq graph from the Code Editor, remove the MasterView
view and the MasterTable and DetailsTable classes.
3. Build the project.
Activity 1.1.2: To Create the UI of an Inquiry Form with Only a Grid
This activity will walk you through the process of developing the UI of an inquiry form.
Story
Suppose that you need to develop the Open Payment Summary (RS401000) form in the Modern UI. The form will
have a table, as shown below.
Figure: The Open Payment Summary form
You’ve already implemented the backend for the form, which includes the RSSVPaymentPlanInq graph and the
RSSVWorkOrderToPay data access class (DAC).
Process Overview
You will modify the TypeScript and HTML files for the Open Payment Summary (RS401000) form as follows:
•
In the TypeScript file, you’ll define the screen class and view class for the form. You will also define the
DetailsView view, which is bound to the RSSVWorkOrderToPay DAC.


Part 1: The Open Payment Summary Form | 17
•
In the HTML file, you’ll define the layout of the form.
You will then test the form.
System Preparation
To be able to create and pay invoices, you need to configure the deployed instance as follows:
1. On the Enable/Disable Features (CS100000) form, enable the Advanced SO Invoices feature.
2. On the Item Classes (IN201000) form, select the STOCKITEM class. On the General tab (General Settings
section), select the Allow Negative Quantity check box. On the form toolbar, click Save.
3. On the Accounts Receivable Preferences (AR101000) form, on the General tab (Data Entry Settings section),
clear the Validate Document Totals on Entry and Require Payment Reference on Entry boxes to simplify
the process of releasing an invoice. On the form toolbar, click Save.
Step 1: Defining the Screen Class of the Form
To define the view of the Open Payment Summary (RS401000) form in the TypeScript file of the form, you define a
screen class and a property for the data view of the form. Do the following:
1. Open the RS401000.ts file.
You can open a TypeScript file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)
2. In the RS401000.ts file, make sure the following import directives as included.
import { createCollection, PXScreen, graphInfo, viewInfo,
 PXView, PXFieldState, gridConfig, PXFieldOptions, GridPreset
} from "client-controls";
3. In the RS401000 screen class, modify the graphInfo decorator, and specify the graph and the primary
view of the form in the decorator properties, as the following code shows.
@graphInfo({
 graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
 primaryView: "DetailsView",
})
export class RS401000 extends PXScreen {
}
4. Define the property for the data view of the form, as the following code shows. For the data view that’s used
to display a table, you need to initialize the property with the createCollection method. The method
takes as the input parameter an instance of the view class, which you will define in the next step.
export class RS401000 extends PXScreen {
    @viewInfo({containerName: "Work Orders with Open Payments"})
    DetailsView = createCollection(RSSVWorkOrderToPay);
}
In the viewInfo decorator, you’ve specified the name of the container for the table.


Part 1: The Open Payment Summary Form | 18
Step 2: Defining the View Class of the Form
In the TypeScript file of the form, you need to define a view class for the table on the Open Payment Summary
(RS401000) form, which is RSSVWorkOrderToPay. Proceed as follows:
1. Define the RSSVWorkOrderToPay view class as follows.
export class RSSVWorkOrderToPay extends PXView {
 OrderNbr: PXFieldState;
 Status: PXFieldState;
 InvoiceNbr: PXFieldState;
 PercentPaid: PXFieldState;
 ARInvoice__DueDate: PXFieldState;
 ARInvoice__CuryDocBal: PXFieldState;
}
To add a joined field to the UI of the form, you’ve separated the name of the joined DAC and the field name
in this DAC with two underscores.
2. Add the gridConfig decorator to the RSSVWorkOrderToPay view class, as the following code shows.
In the gridConfig decorator, you must specify the preset property. Because the table is used on the
inquiry form, you use the Inquiry preset. For details about presets, see Form Layout: Grid Presets.
@gridConfig({
 preset: GridPreset.Inquiry
})
export class RSSVWorkOrderToPay extends PXView {
... 
}
3. Remove the MasterView = createSingle(MasterViewClass); and DetailsView =
createCollection(DetailsViewClass); properties from the screen class. Also, remove the
MasterViewClass and DetailsViewClass classes. These properties and classes are part of the
boilerplate code that was generated by the system.
4. Save your changes.
Step 3: Defining the Layout in HTML
The Open Payment Summary (RS401000) form contains only a table. To define the layout of the form, do the
following:
1. Open the RS401000.html file.
You can open an HTML file of a form from one of the following locations:
•
On the Modern UI Files page of the Customization Project Editor
•
In the FrontendSources\screen\src\development\screens folder of
your Acumatica ERP instance. (The files appear in the file system if you click Export to
Development Folder on the toolbar of the Modern UI Files page.)
2. Remove the existing boilerplate code from the file. Add the qp-grid tag and bind it to the DetailsView
view, as the following code shows.
<template>
    <qp-grid id="gridDetailsView" view.bind="DetailsView"></qp-grid> 
</template>


Part 1: The Open Payment Summary Form | 19
3. Save your changes.
If you used the development folder to modify the TypeScript and HTML files of the form,
you need to update these files in the customization project before publishing it. You do this by
using the Detect Modified Files button on the Modern UI Files page.
4. Publish the customization project.
Step 4: Preparing Data for Testing
In this step, you will add some repair work orders, invoices, and payments to the database. To add these invoices
and payments, do the following:
1. On the Repair Work Orders (RS301000) form, create a new repair work order with the following settings and
save your changes:
•
Customer ID: C000000001
•
Service: Battery Replacement
•
Device: Nokia 3310
•
Description: Battery replacement, Nokia 3310
•
Price (Repair Items tab, Battery repair item type) : 25.00
•
Default Price (Labor tab, CONSULT item): 10.00
2. On the Repair Work Orders (RS301000) form, remove all existing repair work orders from hold. Then assign
the work orders, complete them, and create invoices for them.
3. Open the 000001 work order and do the following:
a. Open the invoice for the 000001 work order: Note the invoice number in the Invoice Nbr. box and open
this invoice on the Invoices (SO303000) form.
b. On the form toolbar, click Remove Hold, Release, and then Pay. The Payments and Applications
(AR302000) form opens.
c. On the Documents to Apply tab, type 10 in the Amount Paid column.
d. On the form toolbar, click Remove Hold and then Release.
4. Open the 000003 work order, and do the following:
a. Open the invoice for the 000003 work order: Note the invoice number in the Invoice Nbr. box and open
this invoice on the Invoices (SO303000) form.
b. Change the invoice’s Due Date to tomorrow's date and save your changes.
Step 5: Testing the Form
In this step, you will test the Open Payment Summary (RS401000) inquiry form with the added invoices and
payments. Do the following:
1. Open the Open Payment Summary inquiry form.
The form should look similar to the one shown below. Notice that the table has a toolbar with standard
buttons and the Filter Settings button.


Part 1: The Open Payment Summary Form | 20
Figure: The basic Open Payment Summary form
2. Change the current business date to the day aer tomorrow. For details about how to change the business
date, see Your Working Environment: To Change the Business Date.
3. On the table toolbar, click the Filter Settings button. The filtering area appears.
4. In the filtering area, click the arrow button and click Due Date (shown below). A Quick Filter button appears
for the Due Date column.
Figure: Adding a quick filter for Due Date
5. Click the Quick Filter button for the Due Date column. The Quick Filter drop-down menu opens.
6. In the menu, do the following:
a. Click Is Less Than.
b. Click the Calendar button in the Value box.
c. In the Calendar dialog box, click @Today (see below).


Part 1: The Open Payment Summary Form | 21
Figure: Specifying quick filter parameters
d. Click Apply.
7. With these filter settings, the form displays overdue payments, as shown in the example below.
Figure: The Open Payment Summary form with overdue payments
8. To clear the filter, in the Quick Filter drop-down menu, click Clear Filter.
9. Change the business date to the current date.


Part 1: The Open Payment Summary Form | 22
UI Definition in HTML and TypeScript: Joined Fields
To add a field from a joined data access class (DAC) of the data view to the UI of an Acumatica ERP form, you can
use one of the approaches described in this topic.
Using Two Underscores
In this approach, you use two underscores to separate the name of the joined DAC and the field name in this DAC.
The following example shows the declaration of a joined field in TypeScript.
Customer__AcctName: PXFieldState;
The following HTML code uses this joined field.
<field name="Customer__AcctName"></field>
Using Periods
In this approach to adding a joined field, you use periods to separate the view class name, the name of the joined
DAC, and the field name.
In the TypeScript code of the form, you do the following:
1. Declare a class with any name, such as the name of the joined DAC.
2. In the class, declare the properties for the joined fields that you’re going to use. The names of the properties
are the names of the fields.
For example, suppose that you need to add fields from the joined Customer class. The new class in
TypeScript should look as follows.
export class Customer {
  AcctName: PXFieldState;
  ClassID: PXFieldState;
}
3. In the view class that corresponds to the data view with the joined DAC, declare a property for the joined
DAC. The name of the property is the name of the joined DAC, and the type of the property is the class that
you’ve just declared. The following example shows an example of the property for the joined DAC.
export class DiscountCustomer extends PXView {
  ...
  Customer : Customer;
}
export class RS209500 extends PXScreen {
  Discount = createSingle(DiscountCustomer);
}
As a result of adding the joined fields in this way, you can use them in HTML. You can specify the full name of the
field, which includes the following parts separated by periods: the view class name, the name of the joined DAC,
and the field name. Alternatively, if the fieldset that contains the field has the data view specified, you can use a
shorter name that omits the view class name. The following code shows both of these approaches.
<qp-fieldset id="columnOne" view.bind="Discount">


Part 1: The Open Payment Summary Form | 23
  <field name="DiscountCustomer.Customer.AcctName"></field>
  <field name=".Customer.AcctName"></field>
</qp-fieldset>
Lesson 1.2: Configure a Filter for the Inquiry Form
In this lesson, you will modify the Open Payment Summary (RS401000) inquiry form, which you have developed, so
that it has filtering parameters. By using the UI elements associated with the filtering parameters, a user can filter
the repair work orders listed in the form’s table.
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


Part 1: The Open Payment Summary Form | 24
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


Part 1: The Open Payment Summary Form | 25
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
Activity 1.2.1: To Add a Filter for an Inquiry Form
This activity will walk you through the process of implementing custom filtering parameters for an inquiry form.
Story
Suppose that you need to add custom filtering parameters to the Open Payment Summary (RS401000) inquiry
form, which you created in the PhoneRepairShop customization project, so that users can filter the repair work
orders listed in the table on the inquiry form.
The form will contain UI elements in the Selection area that can be used to filter the listed repair work orders by the
customer and service type, and to filter the listed sales orders by the customer. You will define filtering parameters
for these UI elements.
Process Overview
In this activity, you’ll add filtering parameters to the Open Payment Summary (RS401000) inquiry form by
performing the following steps:
1. Defining the DAC with only unbound fields that will be used as filtering parameters
2. Configuring the PXFilter data view and the PXCancel action as graph members used for filtering data
for an inquiry form
3. Adding the Selection area to the inquiry form by adjusting the TypeScript and HTML files of the form
Step 1: Defining a DAC with Filtering Parameters for the Inquiry Form
In this step, you will define the RSSVWorkOrderToPayFilter DAC, which will be used to display the selection
criteria (filtering parameters) on the Open Payment Summary (RS401000) form. The DAC will contain two fields
(CustomerID and ServiceID) that correspond to the filtering parameters. To define the DAC with filtering
parameters, do the following:
1. In the RSSVPaymentPlanInq graph, define the RSSVWorkOrderToPayFilter data access class by
using the following code.
    [PXHidden]
    [PXVirtual]


Part 1: The Open Payment Summary Form | 26
    public class RSSVWorkOrderToPayFilter : PXBqlTable, IBqlTable
    {   
        #region CustomerID
        [CustomerActive(DisplayName = "Customer ID")]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID :
            PX.Data.BQL.BqlInt.Field<customerID>
        { }
        #endregion
        
        #region ServiceID
        [PXInt()]
        [PXUIField(DisplayName = "Service")]
        [PXSelector(
            typeof(Search<RSSVRepairService.serviceID>),
            typeof(RSSVRepairService.serviceCD),
            typeof(RSSVRepairService.description),
            DescriptionField = typeof(RSSVRepairService.description),
            SelectorMode = PXSelectorMode.DisplayModeText)]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID :
            PX.Data.BQL.BqlInt.Field<serviceID>
        { }
        #endregion
    }
2. For the filtering parameters to be used in a BQL query, add the serviceID and customerID fields to the
RSSVWorkOrderToPay DAC by using the following code.
        public new abstract class serviceID :
            PX.Data.BQL.BqlInt.Field<serviceID>
        { }
        public new abstract class customerID :
            PX.Data.BQL.BqlInt.Field<customerID>
        { }
3. Build the project.
Step 2: Configuring the Graph Members Used for Filtering Data on the Inquiry Form
In this step, you will prepare the graph members to be used for filtering data on the form. To define the graph
members, in the RSSVPaymentPlanInq graph, do the following:
1. Define a PXFilter data view (as shown in the following code), which provides filtering parameters for the
inquiry form.
        public PXFilter<RSSVWorkOrderToPayFilter> Filter = null!;
2. Define the PXCancel action, which adds the Cancel button to the form toolbar, as shown in the following
code. The action clears the filter.
        public PXCancel<RSSVWorkOrderToPayFilter> Cancel = null!;
3. Replace the definition of the DetailsView data view with the following code, which not only selects
repair work orders that do not have the Paid status but also matches the filtering criteria based on the
values of the serviceID and customerID fields.


Part 1: The Open Payment Summary Form | 27
     [PXFilterable]
       public 
        SelectFrom<RSSVWorkOrderToPay>.
        InnerJoin<ARInvoice>.On<
            ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
        Where<
            RSSVWorkOrderToPay.status.IsNotEqual<
                RSSVWorkOrderEntry_Workflow.States.paid>.
            And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                 Or<RSSVWorkOrderToPay.customerID.IsEqual<
                      RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
            And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
                 Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                      RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>.
        View.ReadOnly DetailsView = null!;
4. Override the IsDirty property of the graph, as the following code shows.
        public override bool IsDirty => false;
5. Build the project.
Step 3: Adding Filtering Elements for the TypeScript File of the Form
In this step, you will define TypeScript code for the Selection area, which has the elements to be used for filtering.
Do the following:
1. In the RS401000.ts file, add createSingle and PXFieldOptions to the list of import directives (if
they are not already added).
2. In the RS401000 screen class, add the view property for the Selection area before the DetailsView
property, as shown in the following code.
 @viewInfo({ containerName: "Selection Area" })
 Filter = createSingle(RSSVWorkOrderToPayFilter);
3. In the primaryView parameter of the graphInfo decorator, change the value to Filter, as shown
below.
@graphInfo({
 graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
 primaryView: "Filter",
})
4. Add the view class with the fields of the Selection area of the form, as shown in the code below.
export class RSSVWorkOrderToPayFilter extends PXView {
 CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
 ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
}
You've specified the CommitChanges option for each field to immediately refresh data records as soon a
user updates a filtering parameter.
5. Save your changes.


Part 1: The Open Payment Summary Form | 28
Step 4: Adding Filtering Elements for the HTML File of the Form
In this step, you will define HTML code for the Selection area. Do the following:
1. In the RS401000.html file, before the qp-grid tag, add qp-template tag with a single qp-
fieldset tag inside it. In the qp-fieldset tag, add two field tags for the filtering parameters, as
shown in the following code.
    <qp-template name="17-17-14" id="formFilter">
        <qp-fieldset slot="A" id="columnFirst" view.bind="Filter">
            <field name="CustomerID"></field>
            <field name="ServiceID"></field>
        </qp-fieldset>
    </qp-template>
You've used the 17-17-14 template, which organizes the controls in three columns. You have only two
controls to be displayed in the Selection area; therefore, you’ve placed all of them in the first column of the
template.
2. Save your changes.
Activity 1.2.2: To Display the Filter Values in the URL
When a form contains filtering parameters, it can be useful to have the selected parameter values in the form URL
so that the same form (that is, the form with the same selections made) can be opened elsewhere without the filter
parameter values needing to be entered again.
Story
Suppose that the users of the custom Open Payment Summary (RS401000) inquiry form would like to easily share
filtered inquiry results with other users by just sharing a link to the form without specifying which values need to be
selected. You need to implement this functionality for the form.
Process Overview
You will implement this behavior by adding the PageLoadBehavior property and setting its value to
PopulateSavedValues in the graphInfo decorator in the TypeScript file of the form. The filter values of the
primary view will be placed in the form URL. You’ll then test the implemented behavior.
Step 1: Displaying the Filter Values in the URL of the Inquiry Form
To display the filter values in the URL of the form, do the following:
1. In the RS401000.ts file, add PXPageLoadBehavior to the list of import directives.
2. Add the PageLoadBehavior property in the graphInfo decorator, as the following code shows.
@graphInfo({
 graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
 primaryView: "Filter",
 pageLoadBehavior: PXPageLoadBehavior.PopulateSavedValues,
})


Part 1: The Open Payment Summary Form | 29
The system inserts into the URL the filter values of the primary view only (in this case, the values of the
PXFilter<RSSVWorkOrderToPayFilter> Filter view).
If you used the development folder to modify the TypeScript and HTML files of the form,
you need to update these files in the customization project before publishing it. You do this by
using the Detect Modified Files button on the Modern UI Files page.
3. Publish the customization project.
Step 2: Testing the Filtering Parameters of the Inquiry Form
In this step, you’ll test the Open Payment Summary (RS401000) inquiry form with the filtering parameters. To test
the filtering parameters, do the following:
1. In Acumatica ERP, open the Open Payment Summary (RS401000) form.
The form should look as shown below. Notice that the form contains a form toolbar, the Selection area with
UI elements that correspond to the filtering parameters, and a table with a toolbar.
Figure: The revised Open Payment Summary form
2. On the Repair Work Orders (RS301000) form, do the following:
a. Create a repair work order and specify the following settings:
•
Customer ID: C000000003
•
Service: Battery Replacement
•
Device: Nokia 3310
•
Description: Battery replacement, Nokia 3310
b. On the form toolbar, click Remove Hold, Assign, Complete, and Create Invoice.
3. On the Open Payment Summary form, in the Customer ID box, select C000000001.
The table displays work orders for the C000000001 customer. Notice that the page URL (shown below)
includes the form ID and customer ID values.
http://localhost/SmartFix_T250/Main?ScreenId=RS401000&CustomerID=C000000001
4. In the Service box, select the Battery Replacement service.


Part 1: The Open Payment Summary Form | 30
The table now displays the work orders for the C000000001 customer and the Battery Replacement service.
Notice that the page URL (shown below) contains the form ID, customer ID, and service ID values.
http://localhost/SmartFix_T250/Main?
ScreenId=RS401000&CustomerID=C000000001&ServiceID=1
This URL can be copied and shared with other users.
5. On the form toolbar, click Cancel.
Notice that the boxes in the Selection area have been cleared and that the URL no longer includes the filter
values.
Lesson 1.3: Dynamically Add Filtering Conditions
In this lesson, you will modify the Open Payment Summary (RS401000) form so that it displays sales orders as well
as repair work orders.
Data View Delegates: General Information
A data view is a PXSelect BQL expression declared in a graph for accessing and manipulating data. By default,
when a data view object is requested by the UI or you invoke the Select() method on the object, the system
executes the query specified in the data view declaration. However, you can define a dynamic query, which
is an optional graph method (called the data view delegate) executed when the data view is requested. If no
dynamic query is defined in the graph, the Acumatica Framework executes the BQL statement from the data view
declaration.
Learning Objectives
In this chapter, you’ll learn how to do the following:
•
Define a data view delegate
•
Define a dynamic query in a data view delegate
Applicable Scenarios
You use data view delegates in the following cases:
•
You’re constructing the query dynamically at run time by adding Where<> and Join<> clauses that
depend on some condition, typically a filter.
•
The query retrieves data fields that can’t be calculated declaratively by attributes—for instance, if you are
retrieving values that are aggregated by calculated data fields.
•
The result set has data records that are not retrieved from the database and are composed dynamically in
code.
If a data view delegate has been created for a data view in a graph, you can’t then retrieve the list of
records for a REST API entity that uses this data view in its endpoint mapping. However, the list of
records can still be retrieved via the REST API for entities that use other data views (which do not have
a data view delegate defined for them) in the same graph.


Part 1: The Open Payment Summary Form | 31
Definition of a Data View Delegate
To define the delegate for a data view, you should declare the data view and add a method that has the same name
as the data view, except with a diﬀerent case for the first letter. (For example, if the data view name is MyDataView,
the name of the delegate must be myDataView.)
The delegate returns an IEnumerable object, as shown in the code below.
// The SupplierProducts data view
[PXFilterable]
public 
    SelectFrom<SupplierProduct>.
    InnerJoin<Supplier>.On<
        Supplier.supplierID.IsEqual<SupplierProduct.supplierID>>.
    OrderBy<SupplierProduct.productID.Asc, SupplierProduct.supplierPrice.Asc, 
            SupplierProduct.lastPurchaseDate.Desc>.
    View.ReadOnly SupplierProducts;
// The delegate for the SupplierProducts data view
protected virtual IEnumerable supplierProducts()
{
    // Implement the dynamic query
}
The framework automatically adds the delegate by its name and invokes the method when the data view object is
requested.
When you declare or alter a data view delegate within a graph extension, the new delegate is attached
to the corresponding data view. To query a data view declared within the base graph or lower-level
extension from the data view delegate, you should again declare the data view within the graph
extension. You don’t need to again declare a generic PXSelect<Table> data member in the graph
extension when it won’t be used from the data view delegate. For details, see Data View Delegates:
Customization of a Data View.
Implementation of a Data View Delegate
When you’re implementing a data view delegate, we recommend that you store the results of its dynamic query in
an object of the PXDelegateResult class and specify whether the query results are already filtered, truncated
(to fit the page), and sorted. Repeated operations are not applied to the PXDelegateResult objects. So if you
do not set IsResultFiltered, IsResultTruncated, or IsResultSorted to true, the platform will,
respectively, filter, truncate, or sort the retrieved data aer it’s obtained from the database.
The following code shows an example of how to use the PXDelegateResult class in a data view delegate.
The PXSelectBase<>.SelectWithViewContext method, used in this example, retrieves the data
from a database under the user context, including the sorting, filtering, and pagination specified by the user.
Therefore, because this method is used, we recommend that you use a PXDelegateResult object with the
IsResultFiltered, IsResultTruncated, and IsResultSorted properties set to true.
protected virtual IEnumerable ardocumentlist()
{
    PXSelectBase<BalancedARDocument> cmd =
        new PXSelectJoinGroupBy<BalancedARDocument,
            ...
            //Set the necessary sorting fields: use the key fields
            OrderBy<Asc<BalancedARDocument.docType, 


Part 1: The Open Payment Summary Form | 32
            Asc<BalancedARDocument.refNbr>>>> 
            (this);
 
    PXDelegateResult delegResult = new PXDelegateResult
    {
        IsResultFiltered = true,
        IsResultTruncated = true,
        IsResultSorted = true
    };
 
    foreach (PXResult<BalancedARDocument> res_record 
        in cmd.SelectWithViewContext())
    {
        // add the code to process res_record
        delegResult.Add(res_record);
    }
    return delegResult;
}
Don’t use the PXSelectBase<>.SelectWithViewContext or
PXSelectBase<>.SelectWindowed method to define your dynamic query if your data view
delegate will filter the list of records returned from the database.
Execution of a Data View Delegate
A data view executes the delegate by using the following rules:
•
If a delegate is not defined, the data view executes the BQL command.
•
If a delegate is defined, the data view invokes the delegate and then does the following:
•
If the delegate returns null, executes the BQL command.
•
If the delegate returns an object, reorders the result according to the OrderBy clause of the BQL
command. However, this reordering of results is avoided if an object of the PXDelegateResult class
is present within the data view delegate, its IsResultSorted property is set to true, and the result is
returned by using this object.
In the delegate, you can execute any queries to get the needed data records. The result set (PXResultset<>
object) returned by the delegate must consist of objects of the same DACs and in the same order as the classes are
specified in the data view type. Thus, in the example above, you can return a PXResult<SupplierProduct>
object, but you can’t return a PXResult<Supplier> object. To return a Supplier object from the delegate,
you have to return the PXResult<SupplierProduct, Supplier> object from the method.
Activity 1.3.1: To Add a Filtering Query Dynamically
This activity will walk you through the process of dynamically adding a filtering query in code for an inquiry form.
Story
Suppose that you need to display both repair work orders and sales orders on the Open Payment Summary
(RS401000) inquiry form. You cannot use a single BQL query of a data view to implement the displaying of two
diﬀerent types of entities on a single form. You need to compose a query for the form dynamically in code rather
than specify the query in the definition of a data view.


Part 1: The Open Payment Summary Form | 33
Process Overview
In this activity, you will dynamically add a filtering query in code for the Open Payment Summary (RS401000)
inquiry form by performing the following steps:
1. Adding a new field to the grid of the inquiry form to indicate whether the record is a repair work order or a
sales order
2. Defining the data view delegate to dynamically add a filtering query on the inquiry form
3. Testing the dynamically added query on the inquiry form
Step 1: Adding a New Column to the Filtered Results
To distinguish between the sales orders and repair work orders that are listed on the Open Payment Summary
(RS401000) form, you need to add a new column to the grid. This column will contain the identifier of the order
type: SO if the order in the row is a sales order; and WO if the order in the row is a repair work order. To add this new
column, do the following:
1. In the Constants.cs file, add the class, as shown below.
    public static class OrderTypeConstants
    {
        public const string SalesOrder = "SO";
        public const string WorkOrder = "WO";
    }
2. In the Messages.cs file, add the following strings to the Messages class.
        // Order types
        public const string SalesOrder = "SO";
        public const string WorkOrder = "WO";
3. Add the following field to the RSSVWorkOrderToPay DAC.
        #region OrderType
        [PXString(IsKey = true)]
        [PXUIField(DisplayName = "Order Type")]
        [PXUnboundDefault(OrderTypeConstants.WorkOrder)]
        [PXStringList(
          new string[]
          {
              OrderTypeConstants.SalesOrder,
              OrderTypeConstants.WorkOrder
          },
          new string[]
          {
              Messages.SalesOrder,
              Messages.WorkOrder
          })]
        public virtual string? OrderType { get; set; }
        public abstract class orderType :
            PX.Data.BQL.BqlString.Field<orderType>
        { }
        #endregion
4. Build the project.


Part 1: The Open Payment Summary Form | 34
5. In the RS401000.ts file, add the field indicated in the following code before the OrderNbr field.
 OrderType: PXFieldState;
Because you’ve added the field to the view class for the grid, you don’t need to edit the HTML file of the
form. By default, all fields specified in the data view for the grid are displayed in the table on the form.
6. Publish the customization project.
Step 2: Defining the Data View Delegate
To display both sales orders and repair work orders in one grid, you need to define a data view delegate in which
you use two separate queries: a query to select sales orders, and a query to select repair work orders. To define this
data view delegate, do the following:
1. To compose a query that selects the sales orders and the invoices created for these sales orders, learn the
names of the DACs that you’ll use in the query.
In Acumatica ERP, an invoice cannot be created directly for a sales order; a user first creates a shipment and
then creates an invoice for the shipment. The invoice number is stored in the shipment record. Therefore,
you need to use the DAC that contains information about shipments. To learn the DAC name, do the
following:
a. In Acumatica ERP, open the Sales Orders (SO301000) form.
b. Open the Shipments tab, which contains information about shipments and the corresponding invoices.
c. On the Settings menu, click Element Inspector and then click the Invoice Nbr. column.
In the Element Properties dialog box, which opens, note that the DAC name is SOOrderShipment
and that the field name of the Invoice Nbr. column is InvoiceNbr.
2. Use the DAC Schema Browser or open the source code of the SOOrderShipment DAC to investigate
its fields. You can see that the DAC contains both the sales order number (in the OrderNbr field) and the
invoice number (in the InvoiceNbr field). You will use this information later to construct a fluent BQL
statement.
3. In the RSSVPaymentPlanInq graph, define a method (as shown in the following code) that converts an
object of the SOOrderShipment DAC to an object of the RSSVWorkOrderToPay DAC.
        public static RSSVWorkOrderToPay ToRSSVWorkOrderToPay
            (SOOrderShipment shipment) =>
        new RSSVWorkOrderToPay
        {
            OrderNbr = shipment.OrderNbr,
            InvoiceNbr = shipment.InvoiceNbr
        };
4. Add the following delegate method. The method has the same name as the data view, except that it uses a
diﬀerent case for the first letter.
        protected virtual IEnumerable detailsView()
        {
            PXDelegateResult delegResult = new PXDelegateResult
            {
                IsResultFiltered = true,
                IsResultTruncated = true,
                IsResultSorted = true
            };
            var workOrderSelect = new
                SelectFrom<RSSVWorkOrderToPay>.


Part 1: The Open Payment Summary Form | 35
                InnerJoin<ARInvoice>.On<
                    ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
                Where<
                    RSSVWorkOrderToPay.status.IsNotEqual<
                        RSSVWorkOrderEntry_Workflow.States.paid>.
                    And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                          Or<RSSVWorkOrderToPay.customerID.IsEqual<
                               RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                    And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
                          Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                               RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>.
                View.ReadOnly(this);
            var workOrders = workOrderSelect.SelectWithViewContext();
            delegResult.AddRange(workOrders);
            var sorderSelect = new
                SelectFrom<SOOrderShipment>.
                InnerJoin<ARInvoice>.On<
                    ARInvoice.refNbr.IsEqual<SOOrderShipment.invoiceNbr>>.
                Where<
                    RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                    Or<SOOrderShipment.customerID.IsEqual<
                         RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                View.ReadOnly(this);
            var sorders = sorderSelect.SelectWithViewContext();
            foreach (PXResult<SOOrderShipment, ARInvoice> order in sorders)
            {
                SOOrderShipment soshipment = order;
                ARInvoice invoice = order;
                RSSVWorkOrderToPay workOrder = ToRSSVWorkOrderToPay(soshipment);
                workOrder.OrderType = OrderTypeConstants.SalesOrder;
                var result = new PXResult<RSSVWorkOrderToPay, ARInvoice>(
                    workOrder, invoice);
                delegResult.Add(result);
            }
            return delegResult;
        }
In the code above, you have used the SelectWithViewContext method to select work
orders and information about sales orders from shipments. You’ve returned the result by using
a PXDelegateResult object with the IsResultFiltered, IsResultTruncated, and
IsResultSorted properties set to true.
5. Add the required using directives, which are shown in the following code.
using PX.Objects.SO;
using System.Collections;
6. Build the project.
Step 3: Testing the Dynamically Added Filter
To test the dynamically added filter on the Open Payment Summary (RS401000) form, do the following:


Part 1: The Open Payment Summary Form | 36
1. In Acumatica ERP, open the Open Payment Summary form.
The form should list both repair work orders and sales orders, as shown in the Order Type column (shown
below).
Figure: The form with repair work orders and sales orders
2. In the Customer ID box, select the C000000003 customer.
The results should look as shown below.
Figure: The Open Payment Summary form for a particular customer


Part 1: The Open Payment Summary Form | 37
Lesson 1.4: Retrieve Aggregated Data
In this lesson, you will add a filtering parameter: the Show Unpaid Subtotals check box. You’ll also update
dynamic queries in the data view delegate so that they change when a user selects or clears this check box.
Data Aggregation: General Information
You can group data on a form by adding an aggregation clause to the query that’s used to retrieve data for the form.
Learning Objectives
In this chapter, you will learn how to add an aggregation clause and function to display aggregated data.
Applicable Scenarios
You group or aggregate data in the following cases:
•
You need to display the maximum or minimum value for a group of records.
•
You need to display the sum of values for a group of records.
•
You need to display the average value for a group of records.
•
You need to display the number of records in a group.
Data Aggregation
To group or aggregate records, you append the AggregateTo<> clause to the statement that’s defining a query.
You can specify the grouping condition and the aggregation function by using the GroupBy clause and the
appropriate aggregation function. To calculate a value for each group, you can use any of the following aggregation
functions: Min, Max, Sum, Avg, and Count.
For details on the use of aggregation functions, see To Select Records by Using Fluent BQL or To Group and
Aggregate Records in Traditional BQL. You can find equivalents between aggregation functions in fluent BQL and
those in traditional BQL in Fluent BQL and Traditional BQL Equivalents.
Activity 1.4.1: To Retrieve Aggregated Data
This activity will walk you through the process of retrieving aggregated data in a fluent BQL query for an inquiry
form.
Story
Suppose that you need to give users the ability to view subtotals for orders of each status on the Open Payment
Summary (RS401000) inquiry form. You need to add a filtering parameter that’s represented by the Show Unpaid
Subtotals check box on the inquiry form. If the check box is selected, the results in the grid need to be grouped by
order status, and the subtotal amounts need to be calculated and shown for each status. You also need to update
the dynamic queries in the data view delegate so that the appropriate query is triggered, depending on the state of
the filtering parameter.


Part 1: The Open Payment Summary Form | 38
Process Overview
In this activity, for the Open Payment Summary (RS401000) inquiry form, you’ll add the ability to view grouped data
and subtotal amounts in the grid by performing the following steps:
1. Adding a new filtering parameter to the inquiry form (the Show Unpaid Subtotals check box)
2. Modifying the dynamic queries in the data view delegate
3. Testing the aggregation of data on the inquiry form
Step 1: Adding a Field for the Check Box to the DAC
To add a field for the Show Unpaid Subtotals check box to the DAC, do the following:
1. In the RSSVWorkOrderToPayFilter DAC, add the field, as shown in the following code.
        #region GroupByStatus
        [PXBool]
        [PXUIField(DisplayName = "Show Unpaid Subtotals")]
        public bool? GroupByStatus { get; set; }
        public abstract class groupByStatus :
            PX.Data.BQL.BqlBool.Field<groupByStatus>
        { }
        #endregion
2. Build the project.
Step 2: Changing UI Files of the Form
Now you need to add the field to the Selection area of the form. Proceed as follows:
1. In the RS401000.ts file, in the RSSVWorkOrderToPayFilter view class, add the GroupByStatus
field aer the ServiceID field. Specify the CommitChanges property for the GroupByStatus field, as
shown in the following code.
export class RSSVWorkOrderToPayFilter extends PXView {
 CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
 ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
 GroupByStatus: PXFieldState<PXFieldOptions.CommitChanges>;
}
2. In the RS401000.html file, in the qp-fieldset element, add the GroupByStatus field aer the
ServiceID field, as shown in the following code.
        <qp-fieldset slot="A" id="columnFirst" view.bind="Filter">
            <field name="CustomerID"></field>
            <field name="ServiceID"></field>
            <field name="GroupByStatus"></field>
        </qp-fieldset>
3. Publish the customization project.


Part 1: The Open Payment Summary Form | 39
Step 3: Modifying the Data View Delegate
In the detailsView delegate, you need to use diﬀerent queries, depending on the state of the Show Unpaid
Subtotals check box. If the check box is selected, you need to group all the selected data by status and calculate
the subtotal amounts to be paid for each status.
To group or aggregate records, you will append the AggregateTo<> clause to the statement and specify the
grouping condition and aggregation function by using the GroupBy clause and the Sum aggregation function. To
modify the data view delegate, do the following:
1. In the RSSVPaymentPlanInq graph, replace the detailsView delegate with the following code.
        protected virtual IEnumerable detailsView()
        {
            PXDelegateResult delegResult = new PXDelegateResult
            {
                IsResultFiltered = true,
                IsResultTruncated = true,
                IsResultSorted = true
            };
            var filter = Filter.Current;
            BqlCommand workOrderQuery = new
                SelectFrom<RSSVWorkOrderToPay>.
                InnerJoin<ARInvoice>.On<
                    ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
                Where<RSSVWorkOrderToPay.status.
                        IsNotEqual<RSSVWorkOrderEntry_Workflow.States.paid>.
                    And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                        Or<RSSVWorkOrderToPay.customerID.IsEqual<
                            RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                    And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
                        Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                            RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>();
            if (filter.GroupByStatus == true)
                workOrderQuery = workOrderQuery.AggregateNew<
                    Aggregate<
                    GroupBy<RSSVWorkOrderToPay.status,
                    Sum<ARInvoice.curyDocBal>>>>();
            var workOrderView = new PXView(this, true, workOrderQuery);
            var workOrders = workOrderView.SelectWithViewContext(null);
            foreach (PXResult<RSSVWorkOrderToPay, ARInvoice> order in workOrders)
            {
                if (filter.GroupByStatus == true)
                {
                    ((RSSVWorkOrderToPay)order[0]).OrderNbr = "";
                    ((RSSVWorkOrderToPay)order[0]).PercentPaid = null;
                    ((RSSVWorkOrderToPay)order[0]).InvoiceNbr = "";
                    ((ARInvoice)order[1]).DueDate = null;
                }
                delegResult.Add(order);
            }


Part 1: The Open Payment Summary Form | 40
            var sorderSelect = new
                SelectFrom<SOOrderShipment>.
                InnerJoin<ARInvoice>.On<
                    ARInvoice.refNbr.IsEqual<SOOrderShipment.invoiceNbr>>.
                Where<
                    RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                    Or<SOOrderShipment.customerID.IsEqual<
                            RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                View.ReadOnly(this);
            var sorders = sorderSelect.SelectWithViewContext();
            foreach (PXResult<SOOrderShipment, ARInvoice> order in sorders)
            {
                SOOrderShipment soshipment = order;
                ARInvoice invoice = order;
                RSSVWorkOrderToPay workOrder = ToRSSVWorkOrderToPay(soshipment);
                workOrder.OrderType = OrderTypeConstants.SalesOrder;
                var result = new PXResult<RSSVWorkOrderToPay, ARInvoice>(
                    workOrder, invoice);
                delegResult.Add(result);
            }
            return delegResult;
        }
In the code above, you’ve assigned a value to the workOrderQuery variable that depends on whether the
Show Unpaid Subtotals check box is selected. If it’s selected, you have constructed a query in which you
group data by the Status field value by using the AggregateTo clause.
In the AggregateTo clause, you have grouped data by the RSSVWorkOrderToPay.status value, and
calculated the sum for each group. Then you’ve returned the results of the query.
In the grid, you have not displayed values in the Order Nbr, Percent Paid, Invoice Nbr, and Due Date
columns for aggregated data, because these values are not relevant to the subtotal rows.
2. Build the project.
No sales orders are grouped because the Status column displays no data for sales orders. This
happens because the sets of statuses for sales orders and repair work orders are diﬀerent, and a
status of a sales order cannot be converted to a status of a repair work order.
Step 4: Testing the Aggregation of Data
To test how data is grouped and calculated on the Open Payment Summary (RS401000) form, do the following:
1. In the Selection area of the Open Payment Summary form, select the Show Unpaid Subtotals check box.
The form should look as shown below.


Part 1: The Open Payment Summary Form | 41
Figure: The filtered and aggregated data on the Open Payment Summary form
Notice that the Balance column now displays the total amount to be paid for all orders of the status
displayed in the Status column. The values of the Balance column will remain unaﬀected for the rows that
don’t have a value in the Status column.
Lesson 1.5: Add Redirection Links to the Grid
In this lesson, you will replace the numbers displayed as plain text in the Order Nbr. and Invoice Nbr. columns with
links. A user can click a link to open the data entry form for the order or invoice identified by the number. The user
can then view the data and make any needed modifications.
You’ll add these links in two ways:
•
By using the PXSelector attribute and modifying the TypeScript file
•
By declaring an action
Redirection to Webpages: General Information
This lesson describes two types of redirection to webpages you can implement on an Acumatica ERP form:
•
Adding a simple redirection link to the grid of a form by using the PXSelector attribute.
•
Adding a redirection link by using an action. By using this approach, you can also add conditional logic to
the form so that a redirection link opens diﬀerent forms, depending on whether a specified condition is met.
Applicable Scenarios
You add redirection to a webpage on a form when you need to give users the ability to open one of the following:
•
A record's data entry form with the record displayed, so that a user can get detailed information about that
record
•
A report or a generic inquiry related to the record


Part 1: The Open Payment Summary Form | 42
•
Any destination URL
Redirection Link from a Selector Control
A redirection link from a selector control is oen used for opening the data entry form on which the record was
created. The user can then view the selected record and make any needed edits. You can add redirection from a
selector control declaratively by doing the following:
•
Adding the PXSelector attribute to a field in its data access class
•
Adding the PXPrimaryGraph attribute to the DAC that corresponds to the record in the selector control
•
Configuring the selector control as described in Selector Control: Configuration of a Link
Redirection in an Action
To implement redirection in an action, you need to throw one of the exceptions provided by the Acumatica
Framework. Once an exception is thrown, it interrupts the current context and propagates up the call stack until
it’s handled by the Acumatica Framework, which performs the redirection. (This mechanism doesn’t aﬀect the
performance of the application.)
The following exceptions are used for redirection:
•
PXRedirectRequiredException opens the specified application page in the same window (the
default behavior) or a new one.
•
PXPopupRedirectException opens the specified application page in a pop-up window.
•
PXReportRequiredException opens the specified report in the same window (the default behavior)
or a new one.
•
PXRedirectWithReportException opens two pages: the specified report in a new window, and the
specified application page in the same window.
•
PXRedirectToUrlException opens the webpage with the specified external URL in a new window.
This exception is also used for opening an inquiry page that’s loaded into the same window by default.
If you want to add a redirection link that’s implemented by using an action, you need to configure the selector
control as described in the Link Behavior section of Selector Control: Configuration of a Link.
Handling of Redirection Exceptions
In most cases, you don’t need to implement the handling of exceptions that are used for redirection.
However, in cases where certain scopes are used that need to distinguish between their own successful
and failed closures in conjunction with a redirect operation, you should implement the exception handler
and explicitly close all these scopes. For example, if a PXTransactionScope object is being used and
redirection is being performed, you should implement the exception handler and explicitly close the scope. For a
PXTransactionScope object, you should do this by calling its Complete method in the catch block of the
exception handler. The following code example shows how this is implemented.
using PXTransactionScope tranScope = new();
try
{
  // Do something that may throw any kind of redirect exception
  tranScope.Complete();
}
catch(PXBaseRedirectException redirect){
  tranScope.Complete();
  throw;
}


Part 1: The Open Payment Summary Form | 43
Note that in the above code, a PXBaseRedirectException has been used. All the exceptions listed in the
preceding section are derived from this base class.
Selector Control: Configuration of a Link
A selector control can display a value as a link to the record whose identifier is displayed in the selector control. The
link is configured diﬀerently depending on the location of the selector control.
Selector Control in a Fieldset
In a fieldset, you add the link by specifying allowEdit: true in the controlConfig decorator for the field in
TypeScript, as shown in the following example.
@controlConfig({allowEdit: true, })
CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
@controlConfig({allowEdit: true, })
CustomerLocationID: PXFieldState;
@controlConfig({allowEdit: true, })
ContactID: PXFieldState;
The code above displays links, as shown below.
Figure: Links in the selector controls
The allowEdit: true setting also adds the + (Add Row) button to the lookup table of the selector
control.
Selector Control in a Table
In a table, a link is displayed by default for a selector control. To remove the link, specify hideViewLink: true
in the columnConfig decorator in TypeScript, as shown in the following example.
@columnConfig({ hideViewLink: true })
BranchID: PXFieldState<PXFieldOptions.CommitChanges>;
The code above removes the link from the Branch column, as shown below.
Figure: Link in the grid


Part 1: The Open Payment Summary Form | 44
Link Behavior
You can configure which action is executed when a user clicks the link in the selector control. By default, the system
opens the record that is selected by the user in the selector control. To specify a custom action, you use one of the
following:
•
For a selector control located in a fieldset, the editCommand property in the controlConfig decorator,
as shown in the following code
export class EPAssignmentMap extends PXView {
  @controlConfig({editCommand: "OpenForm"})
  GraphType: PXFieldState<PXFieldOptions.CommitChanges>;
}
•
For a selector control located in a grid, the linkCommand decorator
@gridConfig({
 preset: GridPreset.Inquiry
})
export class RSSVWorkOrderToPay extends PXView {
 @linkCommand<RS401000>("ViewOrder")
 OrderNbr: PXFieldState;
}
To use a custom action for the link in the selector control, you also need to do the following:
1. In the graph, define an action that opens the entered form with the new record.
2. In the TypeScript file of the form, declare a property of the PXActionState type for this action in the
screen class.
Activity 1.5.1: To Add Redirection Links to the Grid by Using the PXSelector
Attribute
This activity will walk you through the process of adding redirection links to the grid of a form by using the
PXSelector attribute.
Story
Suppose that you want to improve the navigation between the rows of the grid on the Open Payment Summary
(RS401000) form and the invoices listed in these rows. You plan to do this by adding redirection links so users can
navigate to the entry form with the detailed information about the invoice.
You need to replace the text numbers in the Invoice Nbr. column with links that a user can click to open the data
entry form for the invoice whose number the user has clicked. The user can then view the settings and make any
needed modifications. You’ll add these links by configuring the TypeScript file of the form.
Process Overview
In this activity, you will add redirection links to the Invoice Nbr. column of the Open Payment Summary (RS401000)
inquiry form by performing the following steps:
1. Adding redirection links by using the PXSelector attribute for the InvoiceNbr DAC field


Part 1: The Open Payment Summary Form | 45
2. On the Open Payment Summary (RS401000) form, testing the redirection links to make sure they redirect
the user to the Invoices (SO303000) form with the record selected
Step 1: Adding a Link by Using the PXSelector Attribute
In this step, you will add the redirection link to the Open Payment Summary (RS401000) form.
To add a redirection link by using the PXSelector attribute, do the following:
1. Make sure that the DAC that holds invoice records has the PXPrimaryGraph attribute or its descendants
declared on it. This DAC can be either ARInvoice or SOInvoice (which extends ARInvoice).
2. In the RSSVWorkOrder DAC, add the PXSelector attribute to the InvoiceNbr field, as shown in the
following code.
        [PXSelector(typeof(SearchFor<SOInvoice.refNbr>.
            Where<SOInvoice.docType.IsEqual<ARDocType.invoice>>))]
The RSSVWorkOrderToPay DAC, which is used on the Open Payment Summary form, inherits the
InvoiceNbr field, including its attributes. Because the InvoiceNbr field of the RSSVWorkOrder DAC
has the PXSelector attribute assigned, you can configure the same link for other forms where this DAC is
used, such as the Repair Work Orders (RS301000) form.
You can also add the PXSelector attribute in an extension of the RSSVWorkOrder DAC by
using the PXMergeAttributes attribute. For details, see Customization of Field Attributes in
DAC Extensions.
3. Add the necessary using directives.
4. Build the project.
5. In the RS401000.ts file, add the CommitChanges property for the InvoiceNbr field.
export class RSSVWorkOrderToPay extends PXView {
 OrderType: PXFieldState;
 OrderNbr: PXFieldState;
 Status: PXFieldState;
 InvoiceNbr: PXFieldState<PXFieldOptions.CommitChanges>;
 PercentPaid: PXFieldState;
 ARInvoice__DueDate: PXFieldState;
 ARInvoice__CuryDocBal: PXFieldState;
}
6. Publish the customization project.
Step 2: Testing the Redirection Links
To test the redirection links that you’ve implemented, do the following:
1. In Acumatica ERP, open the Open Payment Summary (RS401000) form.
The form should look similar to the one shown below, with links shown in the Invoice Nbr. column.


Part 1: The Open Payment Summary Form | 46
Figure: The links in the Invoice Nbr. column
2. In the row with the 000004 repair work order, click the invoice number.
The Invoices (SO303000) form should open in a new window with the invoice displayed.
The type of invoice determines which form may be opened:
•
Sales invoice: Invoices (SO303000)
•
AR invoice: Invoices and Memos (AR301000)
The logic that determines which form to open is embedded in Acumatica ERP.
Activity 1.5.2: To Add Redirection Links to a Grid by Using an Action
This activity will walk you through the process of adding redirection links to the grid of a form by using an action.
Story
Suppose that you want to improve the navigation between the rows of the grid on the Open Payment Summary
(RS401000) form and the repair work orders and sales orders listed in these lines. You need to implement
redirection links, which will give users the ability to navigate to the form that has detailed information about the
repair work order or the sales order.
Two types of orders are displayed on the Open Payment Summary (RS401000) form: work orders and sales orders.
Thus, when a user clicks an order number in the Order Nbr. column of the grid, the form corresponding to the
order number should be opened:
•
Work order: The Repair Work Orders (RS301000) form
•
Sales order: The Sales Orders (SO301000) form
You cannot implement this behavior by using the approach described in Activity 1.5.1: To Add Redirection Links to
the Grid by Using the PXSelector Attribute. To implement this logic, you will declare an action, and inside the action
method, you’ll implement a redirection link to the corresponding form.


Part 1: The Open Payment Summary Form | 47
Process Overview
In this activity, you will add redirection links to the Order Nbr. column of the Open Payment Summary (RS401000)
inquiry form by performing the following steps:
1. Defining an action in the graph of the form, and defining an action method with the logic to open the
corresponding data entry form, based on the order type
2. Adjusting the TypeScript file of the form
3. On the Open Payment Summary (RS401000) form, testing the redirection links to make sure they open the
appropriate data entry form: Sales Orders (SO301000) or Repair Work Orders (RS301000)
Step 1: Adding an Action to the Graph
In this step, you’ll implement an action that redirects the user to the Repair Work Orders (RS301000) or Sales Orders
(SO301000) form, depending on the order type of the selected line. To add a redirection link by using an action, do
the following:
1. In the RSSVPaymentPlanInq graph, define the ViewOrder action as follows.
        public PXAction<RSSVWorkOrderToPay> ViewOrder = null!;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField]
        protected virtual void viewOrder()
        {
            RSSVWorkOrderToPay order = DetailsView.Current;
            // if this is a repair work order
            if (order.OrderType == OrderTypeConstants.WorkOrder)
            {
                // create a new instance of the graph
                var graph = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
                // set the current property of the graph
                graph.WorkOrders.Current = graph.WorkOrders.
                  Search<RSSVWorkOrder.orderNbr>(order.OrderNbr);
                // if the order is found by its ID,
                // throw an exception to open the order in a new tab
                if (graph.WorkOrders.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true,
                      "Repair Work Order Details");
                }
            }
            // if this is a sales order
            else
            {
                // create a new instance of the graph
                var graph = PXGraph.CreateInstance<SOOrderEntry>();
                // set the current property of the graph
                graph.Document.Current = graph.Document.
                  Search<SOOrder.orderNbr>(order.OrderNbr);
                // if the order is found by its ID,
                // throw an exception to open the order in a new tab
                if (graph.Document.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true,
                      "Sales Order Details");


Part 1: The Open Payment Summary Form | 48
                }
            }
        }
In the action method, you have created a new instance of the RSSVWorkOrderEntry or SOOrderEntry
graph, depending on the type of the order. In the graph, you’ve set the Current property of the primary
view’s PXCache to the order if the system has found it by using the specified ID.
If the current data record is set for the PXCache object, you throw the
PXRedirectRequiredException to open the form with the current data record displayed.
To learn the primary view name, you can use the Element Inspector to explore the Summary
area of the entry form.
2. Build the project.
Step 2: Adjusting the TypeScript File of the Form
To add a redirection link by using an action, you need to adjust the TypeScript file of the form. Do the following:
1. In the RS401000.ts file, add the linkCommand and PXActionState import directives.
2. In the RS401000 screen class, add a property of the PXActionState type for the viewOrder action, as
shown below.
 ViewOrder: PXActionState;
3. In the RSSVWorkOrderToPay view class, add the linkCommand decorator for the OrderNbr field, as
shown in the following code. As the type parameter of the linkCommand decorator, you specify the screen
class in which the property for the action is defined.
 @linkCommand<RS401000>("ViewOrder")
 OrderNbr: PXFieldState;
4. Publish the customization project.
Step 3: Testing the Redirection Links
To test the redirection links that you’ve implemented, do the following:
1. In Acumatica ERP, open the Open Payment Summary (RS401000) form.
2. In the Order Nbr. column of the table, click any sales order number—that is, the number of any order with
an Order Type of SO.
In a new tab, the Sales Orders (SO301000) form should open with the selected sales order.
3. On the Open Payment Summary form, in the Order Nbr. column, click any repair work order number—that
is, any order with an Order Type of WO.
In a new tab, the Repair Work Orders (RS301000) form should open with the selected repair work order.


Part 2: The Payment Info Tab | 49
Part 2: The Payment Info Tab
In the previous part, you’ve implemented an inquiry form that displays payment information about repair work
orders and sales orders that haven’t yet been paid. In this part, you will add a tab, Payment Info, to the Repair
Work Orders (RS301000) form that will display information on the invoice related to the repair work order and the
most recent payment. This tab will be displayed on the form only if the repair work order has been paid.
Lesson 2.1: Add the Payment Info Tab
In this lesson, you will add and configure a new tab on the Repair Work Orders (RS301000) form. On this tab,
you’ll display fields from the ARInvoice and ARPayment DACs. To display these fields, you will use the
PXProjection attribute.
Use of PXProjection: General Information
The PXProjection attribute binds a DAC to an arbitrary dataset defined by the Select command. The
Acumatica Framework does not bind this DAC to a database table—that is, it doesn’t select data from the table with
the same name as the DAC. Instead, you specify a fluent BQL Select command (or its traditional BQL equivalent)
as the data source in your query. This command can select data from one or more DACs and can include most BQL
clauses. Thus, you can think of PXProjection entities as the Acumatica Framework's version of SQL views.
The PXProjection attribute is mainly used to perform complex Select operations by using a BQL query. If you
need to join a BQL query that’s also a complex joined select query, you should use the PXProjection attribute.
You can also use PXProjection to display data from multiple tables on a form or a tab. To do this, you need to
declare a DAC with the PXProjection attribute that defines how data from one or more tables is projected into a
single DAC.
Learning Objectives
In this lesson, you will learn how to do the following:
•
Define the DAC for a new tab with the PXProjection attribute on an existing form
•
Define the data view for the new tab
•
Create a new tab item on an existing form
Applicable Scenarios
You use the PXProjection attribute in the following cases:
•
You want to join a BQL query that’s a complex joined select query.
•
You want to display data from multiple tables on a form or a tab.
•
You want to retrieve a subset of fields from one or more DACs.
•
You want to return filtered data from a DAC.
•
You want to persist data to any number of database tables.
Configuration of the PXProjection Attribute
The following example shows the use of the PXProjection attribute.


Part 2: The Payment Info Tab | 50
[PXProjection(typeof(
    SelectFrom<Supplier>.
    InnerJoin<SupplierProduct>.On<
     SupplierProduct.accountID.IsEqual<Supplier.accountID>>))]
    public class SupplierAccounts: PXBqlTable, IBqlTable 
    {}
You can use the PXProjection attribute with both traditional and fluent BQL.
Your projection query must contain only Select queries derived from the PX.Data.SelectBase
classes that implement the IBqlSelect<Table> interface. These queries should be derived from
the BqlCommand class. Don’t pass derived types of the PXSelectBase class to a projection query
because this will result in a runtime error.
Field Mapping by Using the BqlField and BqlTable Properties
In the projection DAC (the DAC on which you declare the PXProjection attribute), you should explicitly map
each projection field to the corresponding database column retrieved by the Select command. To map a field, set
the BqlField property of the field-binding attribute (such as PXDBString and PXDBDecimal) to the type that
represents the column, as shown in the following code.
[PXDBString(15, IsUnicode = true, BqlField = typeof(Supplier.accountID))]
public virtual string AccountID { get; set; }
Alternatively, for the code example above, you can use the BqlTable property to map the field. The field binds by
its name implicitly since the field has the same name in the Supplier table. Thus, the above code example can be
rewritten as follows.
[PXDBString(15, IsUnicode = true, BqlTable = typeof(Supplier))]
public virtual string AccountID { get; set; }
Note that a projection DAC does not need to map all the available DAC fields. Unbound DAC fields and DAC fields
marked with the PXDBScalar and PXDBCalced attributes don’t need to be mapped because they’re calculated
fields.
Automatic Field Mapping Through Inheritance
To avoid listing all DAC fields, you can inherit the projection from one of the DACs in the Select command. In
this case, don’t override fields from this DAC or add mapping by using BqlField. The following code shows an
example.
[PXProjection(typeof(
    SelectFrom<Supplier>.
    InnerJoin<SupplierProduct>.On<
     SupplierProduct.accountID.IsEqual<Supplier.accountID>>))]
public class SupplierAccounts : Supplier //inherit from Supplier
{
    //You do not have to list fields from the Supplier DAC.
    ...
}


Part 2: The Payment Info Tab | 51
Additional Configuration of the PXProjection Attribute
You can further configure a projection in the following ways:
•
Reduce the field count by using a projection: In many cases, such as when generating reports, you need
only a small subset of the corresponding DAC fields to be returned from the database. To optimize your
query and avoid retrieving all the DAC fields, you can configure a projection that retrieves only the data for a
specific set of fields.
•
Filter rows with the projection: You can configure a projection to return filtered data from a DAC.
•
Declare the projection to be mutable: A projection is read-only by default —that is, it doesn't save data to
the database. However, you can configure a projection to be mutable by setting the Persistent property
of the PXProjection attribute to true.
The Acumatica Framework doesn't support setting the Persistent property to true if your
projection's query includes a Union or UnionAll operation. This ensures that data changes
aren't inadvertently persisted to the underlying tables if the projection extends one or more
DACs used in the Union or UnionAll operation.
•
Use the projection in another projection query: To do this, you reference the existing projection in the
query of the other projection.
•
Use parameterized elements in the projection query: You can write a projection query that contains
parameterized elements, such as the current value of one of the DAC fields.
•
Use the CurrentMatch BQL operator in the projection query: You can add this operator to provide row-
level security.
For details on how to configure a projection in the ways listed above, see Use of PXProjection: Additional
Configuration of the PXProjection Attribute.
Activity 2.1.1: To Display Multiple DAC Data on a Tab
This activity will walk you through deriving a set of data from multiple DACs by using the PXProjection
attribute, and displaying that data on a single tab.
Story
Suppose that in the PhoneRepairShop customization project, you want to display information about the invoice
related to a repair work order and the most recent payment that was made for it. You need to add a tab to the
Repair Work Orders (RS301000) form that will display this information.
The tab will have the following elements:
•
Invoice Nbr.: The number of the invoice that has been created for the repair work order
•
Due Date: The due date for the invoice
•
Latest Payment: The number of the most recent payment applied to the invoice
•
Latest Amount Paid: The amount paid in the payment that was applied to the invoice most recently
This set of data is derived from diﬀerent DACs. To display the UI elements on a single tab, you’ll use the
PXProjection attribute.
Process Overview
In this activity, you will add a new tab to the Repair Work Orders (RS301000) form by performing the following
steps:


Part 2: The Payment Info Tab | 52
1. Defining the DAC for the new tab by using the PXProjection attribute
2. Defining the data view for the new tab
3. Adding the new tab to the form
4. Testing the new tab
Step 1: Learning the DAC Names for the Fluent BQL Query
To retrieve the needed set of data, you’ll find out which DACs you need to use in a fluent BQL query of the
PXProjection attribute. To learn the names of the required DACs, do the following:
1. On the Invoices (SO303000) form, use the Element Inspector on the Summary area of the form to learn the
DAC name for the invoice, and on the Applications tab to learn the DAC name for payments that have been
applied to the invoice. Notice that these are the ARInvoice and ARAdjust2 DACs, respectively.
2. Learn the key fields of the ARInvoice DAC, which you’ll need to know to select records in a fluent BQL
query. The key fields you need to select an invoice are ARInvoice.refNbr and ARInvoice.docType.
3. Analyze the code of the ARAdjust2 DAC. It is an alias of the ARAdjust DAC, so you can use the
ARAdjust DAC.
4. Analyze the code of the ARInvoice and ARAdjust DACs and the fields defined in them. You’ll need the
following fields:
•
For the invoice number, ARInvoice.refNbr
•
For the invoice due date, ARINvoice.dueDate
•
For the payment number, ARAdjust.adjgRefNbr
•
For the payment amount, ARAdjust.curyAdjdAmt
Step 2: Defining the DAC for the Tab
To define the DAC for the tab, do the following:
1. In the Helper/Messages.cs file, add the RSSVWorkOrderPayment string to the Messages class, as
shown in the following code. This message will be used in the PXCacheName attribute for the new DAC.
        public const string RSSVWorkOrderPayment = 
            "Invoice and Payment of the Repair Work Order";
2. In the DAC folder of the PhoneRepairShop_Code project, create the RSSVWorkOrderPayment.cs
file.
3. Add the following using directives.
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
4. Add the RSSVWorkOrderPayment DAC, as shown in the following code.
namespace PhoneRepairShop
{
    [PXCacheName(Messages.RSSVWorkOrderPayment)]
    [PXProjection(typeof(
      SelectFrom<ARInvoice>.
        InnerJoin<ARAdjust>.On<
          ARAdjust.adjdRefNbr.IsEqual<ARInvoice.refNbr>.
          And<ARAdjust.adjdDocType.IsEqual<ARInvoice.docType>>>.
        AggregateTo<


Part 2: The Payment Info Tab | 53
          Max<ARAdjust.adjgDocDate>,
          GroupBy<ARAdjust.adjdRefNbr>,
          GroupBy<ARAdjust.adjdDocType>>))]
    public class RSSVWorkOrderPayment : PXBqlTable, IBqlTable
    {    }
}
In the query of the PXProjection attribute, you’ve selected an invoice and all payments applied to
the invoice. To sort the payments by the date, you’ve used the AggregateTo clause. Inside the clause,
you’ve grouped all payments by their invoice number and document type (which are the same because all
payments selected are applied to the same invoice) and selected the payment with the latest document
date.
5. Add to the RSSVWorkOrderPayment DAC the fields you learned in Instruction 1, as the following code
shows.
        #region InvoiceNbr
        [PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "",
          BqlField = typeof(ARInvoice.refNbr))]
        [PXUIField(DisplayName = "Invoice Nbr.", Enabled = false)]
        public virtual string? InvoiceNbr { get; set; }
        public abstract class invoiceNbr :
            PX.Data.BQL.BqlString.Field<invoiceNbr> { }
        #endregion
        #region DueDate
        [PXDBDate(BqlField = typeof(PX.Objects.AR.ARInvoice.dueDate))]
        [PXUIField(DisplayName = "Due Date", Enabled = false)]
        public virtual DateTime? DueDate { get; set; }
        public abstract class dueDate :
            PX.Data.BQL.BqlDateTime.Field<dueDate> { }
        #endregion
        #region AdjgRefNbr
        [PXDBString(BqlField = typeof(ARAdjust.adjgRefNbr))]
        [PXUIField(DisplayName = "Latest Payment", Enabled = false)]
        public virtual string? AdjgRefNbr { get; set; }
        public abstract class adjgRefNbr :
            PX.Data.BQL.BqlString.Field<adjgRefNbr> { }
        #endregion
        #region CuryAdjdAmt
        [PXDBDecimal(BqlField = typeof(ARAdjust.curyAdjdAmt))]
        [PXUIField(DisplayName = "Latest Amount Paid", Enabled = false)]
        public virtual Decimal? CuryAdjdAmt { get; set; }
        public abstract class curyAdjdAmt :
            PX.Data.BQL.BqlDecimal.Field<curyAdjdAmt> { }
        #endregion
Note that each field has the PXDB<type> attribute with the BqlField parameter specified to set up the
projection.
Although the RSSVWorkOrderPayment DAC has a master-detail relationship with the RSSVWorkOrder
DAC, you don’t need to add any PXDBDefault and PXParent attributes to the fields because all field
values are determined by the query in the PXProjection attribute.
6. Build the project.


Part 2: The Payment Info Tab | 54
Step 3: Defining the Data View for the Tab
To define the data view for the tab, do the following:
1. In the RSSVWorkOrderEntry class, add the following member to the Views region of the class.
        public 
            SelectFrom<RSSVWorkOrderPayment>.
            Where<RSSVWorkOrderPayment.invoiceNbr.
                IsEqual<RSSVWorkOrder.invoiceNbr.FromCurrent>>
            .View Payments = null!;
In the view, you select data from the RSSVWorkOrderPayment DAC with same invoice number (stored in
the RSSVWorkOrder DAC) as in the Summary area of the form.
2. Build the project.
Step 4: Adding a New Tab (Self-Guided Exercise)
To add the new tab, do the following:
1. In the RS301000.ts file, define the Payments property for the RSSVWorkOrderPayment view class
and then define this view class (see the code below).
 @viewInfo({ containerName: "Payment Info" })
 Payments = createSingle(RSSVWorkOrderPayment);
2. In the RSSVWorkOrderPayment view class, specify properties for all fields of the
RSSVWorkOrderPayment DAC, as shown in the code below.
export class RSSVWorkOrderPayment extends PXView {
 InvoiceNbr: PXFieldState;
 DueDate: PXFieldState;
 AdjgRefNbr: PXFieldState;
 CuryAdjdAmt: PXFieldState;
}
3. In the RS301000.html file, add qp-tab in the qp-tabbar container.
4. In the qp-tab tag, add the qp-template tag with a single qp-fieldset tag inside it. Use the 7-10-7
template.
5. Bind qp-fieldset to the Payments view property in the TypeScript class.
6. In the qp-fieldset container, include the field tags for the fields you’ve added in the view class.
  <qp-tab id="tab-PaymentInfo" caption="Payment Info">
   <qp-template
    id="form-PaymentInfo"
    name="7-10-7"
   >
    <qp-fieldset id="fsColumnA-PaymentInfo" slot="A" view.bind="Payments">
     <field name="InvoiceNbr"></field>
     <field name="DueDate"></field>
     <field name="AdjgRefNbr"></field>
     <field name="CuryAdjdAmt"></field>
    </qp-fieldset>
   </qp-template>


Part 2: The Payment Info Tab | 55
        </qp-tab>
7. Publish the customization project.
For details on how to define a tab, see the T210 Customized Forms and Master-Details Relationships course.
Step 5: Testing the New Tab
To test the Payment Info tab of the Repair Work Orders (RS301000) form, do the following:
1. Open the 000001 repair work order.
2. Open the Payment Info tab, which looks as follows.
Figure: The Payment Info tab


Appendix: Initial Configuration | 56
Appendix: Initial Configuration
If for some reason you can’t complete the instructions in To Deploy an Instance for the Training Course, you can
create an Acumatica ERP instance and manually publish the needed customization project as described in this
topic.
Step 1: Deploying the Needed Acumatica ERP Instance for the Training Course
You deploy an Acumatica ERP instance and configure it as follows:
1. To deploy a new application instance, open the Acumatica ERP Configuration wizard and do the following:
a. On the Database Configuration page, type the name of the database: SmartFix_T250.
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
To be able to create and pay invoices, configure the instance as follows:
1. On the Enable/Disable Features (CS100000) form, enable the Advanced SO Invoices feature.
2. On the Item Classes (IN201000) form, for the STOCKITEM class, select the Allow Negative Quantity box and
click Save.
3. On the Accounts Receivable Preferences (AR101000) form, clear the Validate Document Totals on Entry
and Require Payment Reference on Entry check boxes to simplify the process of releasing an invoice. Click
Save.


Appendix: Initial Configuration | 57
Step 2: Publishing the Required Customization Project
You load the customization project with the results of the T270 Workflow API training course and publish this project
as follows:
1. On the Customization Projects (SM204505) form, create a project with the name PhoneRepairShop, and open
it.
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
b. Open the solution and build the PhoneRepairShop_Code project.
c. In the menu of the Customization Project Editor, click Extension Library > Bind to Existing.
d. In the dialog box that opens, specify the path to the App_Data\Projects
\PhoneRepairShop_Code folder, and click OK.
5. In the menu of the Customization Project Editor, click Publish > Publish Current Project.
The published customization project contains all changes to the Acumatica ERP website and database that have
been performed in the previous training courses of the T series. This project also contains the customization
plug-in that fills in the tables created in these training courses with the custom data entered in these training
courses. For details about the customization plug-ins, see To Add a Customization Plug-In to a Project; the creation
of customization plug-ins is outside of the scope of this course.
