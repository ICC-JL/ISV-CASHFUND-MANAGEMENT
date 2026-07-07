# Developer Course Customization: T200 Maintenance Forms

**2026 R1**

**Revision:** 3/18/2026

---

## Contents

- Copyright
- How to Use This Course
- Company Story and Customization Description
- Getting Started
  - Initial Configuration
  - Runtime Architecture of an Application Based on the Acumatica Framework
  - Querying of the Data
- Part 1: Creating a Form with the Customization Project Editor
  - Lesson 1.1: Prepare a Customization Project
  - Lesson 1.2: Create a Form
  - Lesson 1.3: Make the New Form Visible in the UI
  - Lesson 1.4: Configure the Data Access Class
  - Lesson 1.5: Configure the UI of the Form
  - Lesson 1.6: Update Modern UI Files in the Customization Project
  - Lesson 1.7: Add an Event Handler to a Check Box
  - Lesson 1.8: Debug the Code Included in a Code Item
  - Lesson 1.9: Move the Customization Code to an Extension Library
  - Lesson 1.10: Add an Event Handler In Visual Studio
- Part 2: Creating a Form with Visual Studio
  - Lesson 2.1: Create a Graph and a DAC in Visual Studio
  - Lesson 2.2: Build the Form's UI
  - Lesson 2.3: Add the Form to the Site Map and Workspace
  - Lesson 2.4: Create a Substitute Form
- Appendix: Customization Projects

---

## Copyright

(c) 2026 Acumatica, Inc.

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

**Last Updated:** 03/18/2026

---

## How to Use This Course

The Acumatica Cloud xRP Platform is the foundation for building Acumatica ERP and its customizations, the Acumatica mobile app, and applications integrated with Acumatica ERP through the web services API.

The Acumatica Framework provides the platform API and web controls for the development of the UI and business logic of an ERP application. The platform API is used for the development of Acumatica ERP and any embedded applications (that is, customizations of Acumatica ERP). You can also use the Acumatica Framework to develop an ERP application from scratch.

The Acumatica Customization Platform provides customization tools you can use to develop applications embedded in Acumatica ERP. As you work with the Acumatica Customization Platform, you use the platform API provided by Acumatica Framework.

The T200 Maintenance Forms course introduces the main concepts of the Acumatica Framework and the Acumatica Customization Platform through examples where you'll create simple Acumatica ERP forms.

The course is intended for application developers who are starting to learn how to customize Acumatica ERP. It's based on a set of examples that demonstrate the general approach to customizing Acumatica ERP. As you complete the examples, you'll gain ideas about developing your own embedded applications by using the customization tools. As you go through the course, you'll start to customize a cell phone repair shop, which you'll continue in later T-series courses.

After you complete the course, you'll be familiar with the basic programming techniques for customizing Acumatica ERP.

> We recommend that you complete the examples in the presented order because some examples use the results of previous ones.

### What the Course Prerequisites Are

To complete the course successfully, you should have the following knowledge:

- Proficiency with C#, including:
  - Class structure
  - Object-oriented programming (inheritance, interfaces, and polymorphism)
  - Usage and creation of attributes
  - Generics
  - Delegates, anonymous methods, and lambda expressions
- Knowledge of the following concepts of ASP.NET and web development:
  - Application states
  - The debugging of ASP.NET applications by using Visual Studio
  - The process of attaching to IIS by using Visual Studio debugging tools
- Familiarity with the basics of Node.js
- Understanding of the Model-View-ViewModel (MVVM) pattern
- Basic knowledge of JavaScript
- Understanding of TypeScript concepts, such as interface, type, classes, enum, generic, and union
- Knowledge of HTML fundamentals
- Knowledge of debugging in a browser
- Experience with SQL Server, including:
  - Writing and debugging complex SQL queries (WHERE clauses, aggregates, and subqueries)
  - Understanding the database structure (primary keys, data types, and denormalization)
- The following experience with IIS:
  - Configuring and deploying ASP.NET websites
  - Configuring and securing IIS

### What's in a Part

The first part of the course explains how to create a custom Acumatica ERP form by using the Customization Project Editor and how to move the code to an extension library.

The second part shows how to create a new form in Visual Studio and configure a substitute form.

Each part consists of lessons you should complete.

### What's in a Lesson

Each lesson focuses on a particular development scenario that you can implement by using Acumatica ERP customization tools and the Acumatica Framework. Each lesson consists of a brief description of the scenario and an example of its implementation.

### Where the Source Code Is

You can find the source code of the customization described in this course and code snippets for the course in the `Customization\T200` folder of the Help-and-Training-Examples repository in Acumatica GitHub.

### What the Documentation Resources Are

The complete Acumatica ERP documentation is available at https://help.acumatica.com/ and is included in the Acumatica ERP instance.

While viewing any form used in the course, you can click the Open Help button in the top pane of the Acumatica ERP screen to bring up the form-specific Help menu. You can use the links on this menu to quickly access form-related information and activities and to open a reference topic with detailed descriptions of the form elements.

### Which License You Should Use

For this course, you'll use Acumatica ERP under a trial license, which doesn't require activation and provides all available features. For the production use of this functionality, you have to activate the license your organization has purchased. Some features are subject to additional licensing; consult the Acumatica ERP licensing policy for details.

---

## Company Story and Customization Description

This topic describes the company story and explains what should be customized to meet the company's needs.

### Company Story

The Smart Fix company specializes in repairing cell phones of several types. The company provides the following services:

- **Battery replacement:** This service is provided on customer request and doesn't require any preliminary diagnostic checks.
- **Repair of damage from liquids:** This service requires a preliminary diagnostic check and a prepayment.
- **Screen repair:** This service is provided on customer request and doesn't require any preliminary diagnostic checks.

To manage the list of devices serviced by the company and the list of services the company provides, the Acumatica ERP instance of the Smart Fix company needs to be complemented with two maintenance forms: Repair Services and Serviced Devices. In this course, you'll customize Acumatica ERP by developing these maintenance forms.

### Database Schema

For the customization task, two new tables are required: a table containing information about repair services, and a table containing information about the serviced devices. You'll add these tables to the database when you complete the Initial Configuration.

> The design of database tables is outside of the scope of this course. For details, see Designing the Database Structure and DACs.

The table containing information about the provided services is called **RSSVRepairService** and contains the following custom columns:

| Column | Description |
|--------|-------------|
| ServiceID | Serves as a primary key identifying a service. |
| ServiceCD | Contains a service code. In Acumatica ERP, CD is used for natural keys (such as ServiceCD), which means keys that are human-readable and can have an additional meaning. ID is used for surrogate keys (such as ServiceID), which are pure identifiers. For details, see Naming Conventions for Tables (DACs) and Columns (Fields). |
| Description | Contains a description of a repair service. |
| Active | Indicates whether a service is active at the moment. |
| WalkInService | Indicates whether a service is provided immediately after a customer requested it. |
| PreliminaryCheck | Indicates whether a service is provided after a preliminary diagnostic check. |
| Prepayment | Indicates whether a service requires prepayment. |

The table containing information about devices is called **RSSVDevice** and contains the following custom columns:

| Column | Description |
|--------|-------------|
| DeviceID | Serves as a primary key identifying the device. |
| DeviceCD | Contains the device code. |
| Description | Contains a description of the device. |
| Active | Indicates whether the device is being serviced. |
| AvgComplexityOfRepair | Contains an option indicating the level of complexity of the repair: Low, Medium, or High. |

### The Repair Services Form

The Repair Services (RS201000) form, which you will develop, will be used to view the list of services provided by the Smart Fix company. By clicking buttons on the form toolbar, users will be able to add a new service, edit an existing service, and delete a service.

The Repair Services form will use the RSSVRepairService table.

### The Serviced Devices Form

You will also develop the Serviced Devices (RS202000) form, which will be used to view the list of devices that are serviced by the company. When a user brings up the form, the user will initially see a list of devices displayed in a grid. When the user selects a device in the grid, a detail view of the record will be displayed.

The Serviced Devices form will use the RSSVDevice table.

---

## Getting Started

In this part of the course, you will get an overview of application programming with the Acumatica Framework.

### Initial Configuration

You need to perform prerequisite actions before you start to complete the course.

#### Step 1: Preparing the Environment

Prepare the environment for the training course as follows:

1. Make sure that the environment you're going to use conforms to the System Requirements for the Acumatica ERP Installation.
2. Make sure that the Web Server (IIS) features listed in Configuration of IIS Web Server Features are turned on.
3. Install the Acuminator extension for Visual Studio.
4. Clone or download the customization project and the source code of the extension library from the Help-and-Training-Examples repository in Acumatica GitHub to a folder on your computer.
5. Install Acumatica ERP. On the Main Software Configuration page of the Acumatica ERP Setup wizard, select the Install Acumatica ERP and Install Debugger Tools check boxes.

> If you've already installed Acumatica ERP without the debugger tools, you should uninstall it and install it again with the Install Debugger Tools check box selected. The reinstallation of Acumatica ERP doesn't affect existing Acumatica ERP instances. You can also install the Acumatica ERP Tools separately. For details, see Acumatica ERP Installation On-Premises: To Install the Acumatica ERP Tools (Optional).

#### Step 2: Deploying the Needed Acumatica ERP Instance for the Training Course

You deploy an Acumatica ERP instance and configure it as follows:

1. Open the Acumatica ERP Configuration wizard and do the following:
   a. Click Deploy a New Acumatica ERP Instance for T-Series Developer Courses.
   b. On the Instance Configuration page, do the following:
      - In the Training Course box, select T200 Maintenance Forms.
      - In the Local Path to the Instance box, select a folder that's outside of the `C:\Program Files (x86)`, `C:\Program Files`, and `C:\Users` folders. (We recommend that you store the website folder outside of these folders to avoid an issue with permission to work in these folders when you customize the website.)
   c. On the Database Configuration page, make sure the name of the database is SmartFix_T200.
   d. On the Website Configuration page, make sure the Install Node.js and Use Modern UI as Default check boxes are selected.

   The system creates a new Acumatica ERP instance, adds a new tenant, and loads the data to it. The system also installs Node.js and adds the NodeJs:NodeJsPath key in the appSettings section of the Web.config file of the instance.

2. Sign in to the new tenant by using the following credentials:
   - **Username:** admin
   - **Password:** setup

   Change the password when the system prompts you to do so.

3. In the top right corner of the Acumatica ERP screen, click the username and then My Profile. The User Profile (SM203010) form opens. On the General Info tab, under Personal Settings, select YOGIFON in the Default Branch box; then click Save on the form toolbar.

   In subsequent sign-ins to this account, you'll be signed in to this branch.

4. Optional: Add the Customization Projects (SM204505), Site Map (SM200520), and Generic Inquiry (SM208000) forms to your favorites.

#### Step 3: Creating the Database Tables

Next, add the RSSVRepairService and RSSVDevice tables to the instance's database by executing the `Customization\T200\SourceFiles\DBScripts\T200_DatabaseTables.sql` script, which you've downloaded from Acumatica GitHub.

Before you can customize Acumatica ERP, tables for the instance's database need to be designed and added to the database. For this course, the database scripts have been prepared in advance--so you only needed to add them to the instance's database.

> The design of database tables is outside the scope of this course. For details on designing database tables for Acumatica ERP, see Designing the Database Structure and DACs.

---

### Runtime Architecture of an Application Based on the Acumatica Framework

The Acumatica Framework provides the platform and tools for developing cloud business applications. This topic explains the runtime structure of the Acumatica Framework and introduces its main components.

#### Runtime Structure and Components

An application written with the Acumatica Framework has an n-tier architecture with a clear separation of the presentation, business, and data access layers.

#### Data Access Layer

The data access layer of an application created with the Acumatica Framework is implemented as a set of data access classes (DACs) that wrap data from database tables or data received through external sources (such as Amazon Web Services).

The instances of DACs are maintained by the business logic layer. Between requests, these instances are stored in a session. On a standalone Acumatica ERP server, session data is stored in the server's memory. In a cluster of application servers, session data is serialized and stored in a high-performance remote server through a custom optimized serialization mechanism.

For details about data storage in a session, see Session. To learn about working with the data access layer, see Accessing Data.

#### Business Logic Layer

The business logic is implemented through business logic controllers (also called graphs). Graphs are classes derived from the special API class PXGraph and tied to one or more DACs.

Each graph conceptually consists of two parts:

- **Data views**, which include references to the required DACs, their relationships, and other meta information
- **Business logic**, which consists of actions and events associated with the modified data

Each graph can be accessed from the presentation layer or from application code implemented within another graph. When the graph receives an execution request, it:

1. Extracts the data required for request execution from the DACs included in the data views
2. Triggers the business logic execution
3. Returns the execution's result to the requesting party
4. Updates the data access classes' instances with the modified data

For details on working with the business logic layer, see Implementing Business Logic.

#### Presentation Layer

The presentation layer provides access to the application's business logic through the UI, web services, and the Acumatica mobile app. The presentation layer is completely declarative and contains no business logic.

The Modern UI is a .NET-compatible product that delivers updated UI capabilities without relying on ASPX pages. On the server side, the Modern UI is represented by web services. On the client side, it's represented by a template-based single-page application (SPA) framework based on Aurelia.

The application code is written in TypeScript. The framework transcribes this code into JavaScript code for execution in the web browser. This approach simplifies code maintenance. Developers use HTML and CSS to design form layouts.

For details on the configuration of the UI, see Modern UI Development: General Information.

The UI also includes reports created with the Acumatica Report Designer.

---

### Querying of the Data

The Acumatica Framework provides a custom language, business query language (BQL), that developers can use for writing database queries. BQL is written in C# and based on generic class syntax but is very similar to SQL syntax.

The framework provides two BQL dialects: traditional BQL and fluent BQL. We recommend using fluent BQL because statements written in fluent BQL are simpler and shorter than those written in traditional BQL. In this topic, all examples are written in fluent BQL.

You can also use LINQ to select records from the database or to apply additional filtering to the data of a BQL query. For details on which approach to use, see Comparison of Fluent BQL, Traditional BQL, and LINQ.

#### Business Query Language

BQL has almost the same keywords as SQL does, placed in the same order, as shown in the following BQL example.

```csharp
SelectFrom<Product>.Where<Product.availQty.IsNotNull.
    And<Product.availQty.IsGreater<Product.bookedQty>>>
```

If the database provider is Microsoft SQL Server, the framework translates this expression into the following SQL query.

```sql
SELECT * FROM Product
WHERE Product.AvailQty IS NOT NULL
AND Product.AvailQty > Product.BookedQty
```

BQL gives you the following benefits:

- It's independent of the database provider.
- It's object-oriented and extendable.
- It provides compile-time syntax validation, which helps prevent SQL syntax errors.

#### Data Access Classes

Because BQL is implemented on top of generic classes, you need data types that represent database tables: data access classes (DACs) in the Acumatica Framework. For example, you can define the Product DAC (shown below) to execute the SQL query from the previous code example.

```csharp
using System;
using PX.Data;

[PXCacheName("Product")]
public class Product : PX.Data.PXBqlTable, PX.Data.IBqlTable
{
    // The property holding the ProductID value in a record
    [PXDBIdentity(IsKey = true)]
    public virtual int? ProductID { get; set; }
    // The type used in BQL statements to reference the ProductID column
    public abstract class productID : PX.Data.BQL.BqlInt.Field<productID> { }

    // The property holding the AvailQty value in a record
    [PXDBDecimal(2)]
    public virtual decimal? AvailQty { get; set; }
    // The type used in BQL statements to reference the AvailQty column
    public abstract class availQty : PX.Data.BQL.BqlDecimal.Field<availQty> { }

    // The property holding the BookedQty value in a record
    [PXDBDecimal(2)]
    public virtual decimal? BookedQty { get; set; }
    // The type used in BQL statements to reference the BookedQty column
    public abstract class bookedQty : PX.Data.BQL.BqlDecimal.Field<bookedQty> { }
}
```

Each table field is declared in a DAC in two ways, each for a different purpose:

- As a **public virtual property** (also referred to as a property field) to hold the field's data
- As a **public abstract class** (also referred to as a class field or BQL field) to reference a field in BQL commands

If the DAC is bound to the database, it must have the same class name as the database table has. DAC fields are bound to the database by means of data mapping attributes (such as PXDBIdentity and PXDBDecimal), using the same naming convention as fields in the database.

**Related Links**

- Querying Data in Acumatica Framework

---

## Part 1: Creating a Form with the Customization Project Editor

In this part of the course, you'll start by creating the first simple form of the application: a maintenance form. Maintenance forms are used to enter and manage data that will be used on the application's data entry, inquiry, and processing forms.

At the Smart Fix company, when a user enters an order, they need to specify the repair services being requested. For the entry of these services:

- A text box wouldn't be suitable because typed descriptions are error-prone, and a user can select a repair service more quickly than typing a description of it. Also, typed descriptions can't be used in inquiry or processing forms.
- A simple drop-down list would be inflexible to changes to the set of available services. While the company currently offers only a few services, it expects to offer more as the company expands and devices evolve.

If a selector control is used, authorized users can maintain its values on another form. This makes this control the optimal approach: Users can quickly select services, which can be changed without further customization.

To support the use of the selector control, the company needs a dedicated maintenance form for managing the list of repair services. On this form, authorized users can add, update, or remove services as needed--keeping the list up to date without needing to modify the entry form where orders are entered.

In this part of the course, you'll use the Customization Project Editor to design the Repair Services maintenance form. The form will be used to manage the list of the services the repair shop provides, along with their basic settings.

### Maintenance Forms

Maintenance forms are forms where data can be entered about records of a particular type, which are then available for selection on other forms. Compared with data entry forms, maintenance forms are generally used rarely to define fewer records.

When records of a particular type have been defined on a maintenance form, users can select them rather than type them on data entry forms. However, unlike predefined options in a drop-down box, items defined on a maintenance form and selected on other forms can be added by any authorized user--and they're immediately available for selection. The records can also be selected on other types of forms, so that users can view (on an inquiry form or report) and process (on a processing form) data filtered or organized by particular records of the type.

For instance, in Acumatica ERP, users enter AR invoices on a data entry form. Some of the invoices' settings can be defined on a maintenance form, such as the credit terms used by customers to pay the company. These maintenance records are entered less frequently and are fewer in number than AR invoices are.

---

### Lesson 1.1: Prepare a Customization Project

In this lesson, you'll create a customization project in which you'll create maintenance forms as you complete this course. You'll also add the first item--a database script--to the customization project.

#### Lesson Objectives

As you complete this lesson, you'll learn the following:

- What a customization project is
- How to create a customization project
- How to add a database script for a database table to the customization project

#### Customization Projects

A customization project is a set of changes to the user interface, configuration data, and functionality of Acumatica ERP. The customization project holds the changes that have been made for a particular purpose. It might include a wide variety of changes--such as changes to the mobile site map, generic inquiries, and customized or new forms and reports.

To apply the content of a customization project to an instance of Acumatica ERP, you have to publish the project. Before the project is published, the changes exist only in the project and aren't yet applied to an instance.

For details on customization projects, see Customization Project.

##### Step 1.1.1: Create the Customization Project

The creation of a customization project is the first step in the customization of Acumatica ERP. To create the customization project you'll use in this course, do the following:

1. In Acumatica ERP, open the Customization Projects (SM204505) form.
2. On the form toolbar, click Add Row.
3. In the Project Name column, enter the customization project's name: **PhoneRepairShop**.
4. On the form toolbar, click Save.

You've created the customization project. In the next step, you'll open the Customization Project Editor and begin the customization.

##### Step 1.1.2: Add a Database Table Schema

In this step, you'll add a table schema for the RSSVRepairService table, which you added to the instance database as part of the course prerequisite steps. When you publish the customization project on a different instance of Acumatica ERP, the RSSVRepairService table will be created in the instance database based on the schema provided in the customization project.

For details on database script items, see Database Scripts.

> The design of database tables is outside the scope of this course. For details on designing database tables for Acumatica ERP, see Designing the Database Structure and DACs.

To add a table schema, do the following:

1. On the Customization Projects (SM204505) form, open the PhoneRepairShop customization project. The system opens the project in the Customization Project Editor.
2. In the navigation pane, click Database Scripts.
3. On the Database Scripts page, click Add Custom Table Schema.

> When you need to add a custom table to the instance database, we recommend adding a custom table schema, not a custom table script, to the customization project. A custom SQL script could cause the loss of the integrity and consistency of the application data. For details, see Changes in the Database Schema.

4. In the Add Custom Table Schema dialog box, start typing RSSVRepairService in the Table box and select the option with this name.
5. Click OK.

The script for the RSSVRepairService table has been added to the customization project.

In the project, the schema is kept in XML format. When the customization project is published, the Acumatica Customization Platform will execute a procedure to create the table according to the schema, while meeting all the requirements of Acumatica ERP.

---

### Lesson 1.2: Create a Form

In this lesson, you'll use the Customization Project Editor to create a simple form with a toolbar and a grid.

The Repair Services maintenance form will have a grid listing the services that the repair shop provides. The grid's columns are listed below, along with the data type and description of each.

| Column Name | Data Type | Description |
|-------------|-----------|-------------|
| Service ID | String | The identifier of the service |
| Description | String | The description of the service |
| Active | Boolean | An indicator of whether the service is currently provided by the shop |
| Walk-In Service | Boolean | An indicator of whether this is a walk-in service |
| Requires Preliminary Check | Boolean | An indicator of whether the service requires diagnostic checks |
| Requires Prepayment | Boolean | An indicator of whether this service should be prepaid |

#### Lesson Objective

As you complete this lesson, you'll learn how to create a form of the application by generating the needed items.

##### Step 1.2.1: Create a Form Template

To simplify the process of creating a new form, the Acumatica Customization Platform provides a dialog box you can use to create a template for a new form. You open this dialog box from the Screens page of the Customization Project Editor.

To create a form template for the Repair Services form, do the following:

1. Open the PhoneRepairShop customization project in the Customization Project Editor: Click the PhoneRepairShop project name on the Customization Projects (SM204505) form.
2. On the navigation pane, click the Screens node. The Screens page opens with a blank table.
3. On the page toolbar, click Create New Screen.
4. In the Create New Screen dialog box, enter the following settings:
   - **Screen ID:** RS.20.10.00
     - This ID complies with the Acumatica Framework conventions as follows:
       - RS is a two-letter identifier indicating the part of the functional area (for Acumatica ERP) or subject area (which in this case is repair service).
       - 20 indicates a maintenance form.
       - 10 is the number of the maintenance form in RS, which is generally sequential.
     - For more information on naming conventions, see Form and Report Numbering.
   - **Graph Name:** RSSVRepairServiceMaint
     - Every page must be associated with a graph, and the graph's name should start with a prefix and end with a suffix. The prefix consists of the two-letter identifier indicating the part of the functional area (in this case, RS) and a two-letter prefix of the application area (in this case, SV to indicate service). The suffix indicates the type of the form the graph is used for--in this case, Maint. For details, see Graph Naming.
   - **Graph Namespace:** PhoneRepairShop (filled automatically)
   - **Page Title:** Repair Services
   - **Template:** Grid (GridView)
     - A form template determines which basic containers the form will have: a form with boxes, a grid, a tab, or a combination of these containers.
   - **Create Modern UI Files:** Selected

5. Click Create to:
   - Create the form with the specified settings. The dialog box opens the Code Editor page with the generated code of the RSSVRepairServiceMaint class.
   - Create the form template and add the following items to the customization project:

| Item | Description |
|------|-------------|
| Screens > RS201000 | This Page item includes the contents of the new form. |
| Code > RSSVRepairServiceMaint | This Code item contains the code template of the graph for the new form. This item is saved in the database. When you publish the project, the platform creates a copy of the code in the RSSVRepairServiceMaint.cs file in the App_RuntimeCode folder of the Acumatica ERP application instance. |
| Files > Pages\RS\RS201000.aspx | These File items contain ASPX page code, which define the Classic UI of the form. |
| Files > Pages\RS\RS201000.aspx.cs | |
| Modern UI Files > screens\RS\RS201000\RS201000.html | These PerTenantFile items contain the HTML and TypeScript code, which define the Modern UI of the new form. |
| Modern UI Files > screens\RS\RS201000\RS201000.ts | |
| Site Map > Repair Services | This SiteMapNode item contains the site map object of the new form. |
| Access Rights > RS201000 | This ScreenWithRights item contains the access rights for this form. By default, the system adds to the customization project the Delete access rights for the Customizer role for each new form. |

6. Publish the customization project. To do that, on the main menu of the Customization Project Editor, select Publish > Publish Current Project. The system opens the Compilation pane, which shows the progress of publication in it. When you see Website updated, the publication has completed.
7. Close the Compilation pane.

**Related Links**

- Graph

#### Analysis of the Generated Code of the Graph

As described in Runtime Architecture of an Application Based on the Acumatica Framework, graphs implement business logic in Acumatica ERP. A graph provides the interface for the presentation logic to work with the business data and relies on data access layer components to store and retrieve that data from the database.

A graph is derived from the PXGraph class, with or without parameters. You can specify DACs as parameters to configure layout or background processing operations.

The following code was generated for the RSSVRepairServiceMaint graph.

```csharp
using System;
using PX.Data;

namespace PhoneRepairShop
{
    public class RSSVRepairServiceMaint : PXGraph<RSSVRepairServiceMaint>
    {
        public PXSave<MasterTable> Save;
        public PXCancel<MasterTable> Cancel;
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
    }
}
```

As you can see, the RSSVRepairServiceMaint graph is derived from the PXGraph class with itself as a parameter. For details, see PXGraph<TGraph> Class. Also, the graph is declared with the public access modifier so that the Acumatica Customization Platform can correctly recognize it. This is a requirement for all graph declarations.

The graph declares the following members:

- **The Save action:** Commits the data changes to the database and then reloads the committed data.
- **The Cancel action:** Discards all the changes made to the data and reloads the data from the database.
- **The MasterView and DetailsView views:** Function as data members for the form control and the grid control. Later in this course, you'll add a custom view to replace these ones.
- **The MasterTable and DetailsTable data access classes (DACs):** Work with data in the database. Later in this course, you'll add custom DACs to replace these ones.

**Related Links**

- PXGraph Class

---

### Lesson 1.3: Make the New Form Visible in the UI

In Acumatica ERP, a workspace is a menu that contains links to the forms and reports of a particular area of the product. You access a workspace by clicking a link on the main menu.

Now that you've created the project items required for a new form, you need to add the form to a workspace so that it appears in the Acumatica ERP UI. You will create a new workspace for the maintenance forms you're creating in this course.

#### Lesson Objectives

As you complete this lesson, you'll learn how to do the following:

- Create a workspace
- Add a link to a custom form to the workspace
- Update the SiteMapNode item in the customization project

##### Step 1.3.1: Create a Workspace

Before adding a form to the Acumatica ERP UI, you need to decide whether it will be organized in an existing workspace or a new one. In this case, you'll create a workspace, which will contain all forms related to the phone repair shop.

To create this workspace, do the following:

1. On the main menu of Acumatica ERP (in the lower left corner), click the configuration menu button and then click Edit Menu to switch to Menu Editing mode.
2. On the top toolbar (top left), click Add Workspace.
3. In the Workspace Parameters dialog box, specify the following settings:
   - **Icon:** phone iphone
   - **Area:** Other
   - **Title:** Phone Repair Shop
4. Click OK to save your changes and close the dialog box.
5. Pin the new workspace to the main menu panel by clicking the Pin button.
6. In the main menu panel, drag the workspace immediately below the Data Views workspace.

**Related Links**

- UI Navigation Options: To Configure a Workspace

##### Step 1.3.2: Add the Link to the Workspace

Now that you've created the Phone Repair Shop workspace, you can add to it a link to the Repair Services (RS201000) form.

To add this link to the workspace, do the following:

1. If you're not still in Menu Editing mode, on the main menu of Acumatica ERP (in the lower left corner), click the configuration menu button, and then click Edit Menu.
2. On the main menu, click Phone Repair Shop.
3. On the top toolbar, click Add Menu Item.
4. In the Select Forms dialog box, select the check box left of Repair Services.
5. Click Add & Close to add the link and close the dialog box.
6. Select the check box to the left of Repair Services to add the item to the quick menu; then click Edit link parameters.
7. In the Item Parameters dialog box, select the Configuration category and click OK.
8. In the bottom left corner of the screen, click Exit Menu Editing to save your changes and exit this mode.
9. To make sure the link was added properly, on the main menu, click Phone Repair Shop.

**Related Links**

- UI Navigation Options: To Configure a Workspace

##### Step 1.3.3: Update the SiteMapNode and ScreenWithRights Items

When you configured the new form's location in Acumatica ERP in Step 1.3.1 and Step 1.3.2, the changes you made were saved to the database but not to the customization project. The system also updated the access rights of the form in your instance so that the access rights reference the Modern UI of the form. To save these changes in the customization project, do the following:

1. In the Customization Project Editor, open the PhoneRepairShop customization project.
2. On the navigation pane, click Site Map. The Site Map page opens.
3. On the page toolbar, click Reload from Database.
4. On the navigation pane, click Access Rights. The Access Rights page opens.
5. On the page toolbar, click Reload from Database.
6. Publish the customization project.

---

### Lesson 1.4: Configure the Data Access Class

In this lesson, you'll configure the data access class (DAC) generated for the Repair Services form. You need this class to access data from the database.

#### Lesson Objectives

As you complete this lesson, you'll learn how to do the following:

- Generate and configure a DAC
- Define a view in a graph

#### Data Access Classes

Data access classes (DACs) represent database tables in the application. A DAC consists of data fields. Each data field definition consists of two members of the class, which have the same name except for the case of the first letter:

- A **public abstract class** (also referred to as a class field or BQL field), such as companyType, that represents the data field in BQL statements
- A **public virtual property** (also referred to as a property field), such as CompanyType, that holds the field value

You can define a DAC by manually typing the code or by using the Create Code File dialog box of the Customization Project Editor. By using the dialog box, you can generate the initial code of a DAC based on the schema of the database table.

##### Requirements for DACs

When you define a data access class, keep these requirements in mind:

- The class must have one of these attributes:
  - **PXCacheName:** Specifies a user-friendly DAC name. This name can be used in generic inquiries, reports, and error messages displayed when no setup data records exist. Without this attribute, the error message would use the DAC name for the link.
  - **PXHidden:** Hides the DAC from generic inquiries, reports, and web services API clients.
- The class must be declared with the public access modifier.
- The class must be declared as extending the PX.Data.PXBqlTable class and implementing the PX.Data.IBqlTable interface.
- Abstract classes of data fields must be defined as implementing interfaces of the PX.Data.BQL namespace.
- A DAC property field must have a nullable type (such as decimal? or DateTime?).

##### Order of the Fields in a DAC

Note the order in which fields are declared in a DAC: During every roundtrip, the Acumatica Framework applies changes to DAC instances in the same order as their fields are declared. Similarly, all field-level event handlers are always raised in the same order as fields are declared in the DAC.

**Related Links**

- Data Access Class
- Designing the Database Structure and DACs

##### Step 1.4.1: Generate a DAC

You generate the DAC code by doing the following:

1. Open the PhoneRepairShop customization project in the Customization Project Editor.
2. In the navigation pane, click Code. The Code page opens. It already contains the record about the RSSVRepairServiceMaint graph created in Step 1.2.1.
3. On the page toolbar, click Add New Record.
4. In the Create Code File dialog box, specify the following settings:
   - **File Template:** New DAC
   - **Class Name:** RSSVRepairService
   - **Generate Members from Database:** Selected
5. Click OK to close the dialog box. The new DAC code is opened in the Code Editor.

**Related Links**

- To Create a New DAC

##### Step 1.4.2: Configure the Attributes of the New DAC

Now that the code of the RSSVRepairService DAC has been generated, you will configure attributes for each field of the DAC.

###### Use of Attributes

In the Acumatica Framework, you use attributes to add common business logic to the application components. An attribute may be placed on a declaration of a class or a class member, with or without parameters. The possible parameters for an attribute depend on the constructor's parameters and the properties defined in the attribute.

The constructor's parameters are placed first without names, and the named property settings follow them, as shown in the following example.

```csharp
[PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
public virtual bool? Released { get; set; }
```

Here the PXDefault attribute is created with a constructor that has a Boolean-type parameter (set to false). That means the default value of the Released property is false. Additionally, the PersistingCheck property is specified.

Another typical example of an attribute is PXUIField, as shown in the following example. It's used to configure the input control for the column in the user interface so that the column can have the same visual representation on all application forms (unless it is redefined for a particular form).

```csharp
[PXUIField(DisplayName = "Available Qty", Enabled = false)]
public virtual string? AvailQty { get; set; }
```

In the PXUIField constructor, the DisplayName and Enabled properties of the AvailQty field are specified. The PXUIField attribute is required for all fields to be displayed on the form. To visually identify a field that requires a value, you can specify the Required = true property of that field in the PXUIField constructor, which places an asterisk by the UI element corresponding to the field.

> The fields of a DAC are bound to the database by data mapping attributes (such as PXDBIdentity and PXDBString). The fields that are bound to the database must have the same name as the fields in the database table. For details, see Bound Field Data Types.

###### Configuration of the Attributes of the RSSVRepairService DAC

Do the following to configure the attributes of the RSSVRepairService DAC:

1. In the Code Editor of the Customization Project Editor, open the RSSVRepairService code item.
2. Remove the `[Serializable]` attribute before the RSSVRepairService DAC declaration and adjust the `[PXCacheName("Repair Service")]` attribute. Because of the attribute, the DAC obtains a user-friendly name.
3. In the DAC code that is generated, replace the generated attributes with the following attributes:

   **For the ServiceID field**, remove IsKey=true for the PXDBIdentity attribute:

   ```csharp
   #region ServiceID
   [PXDBIdentity]
   public virtual int? ServiceID { get; set; }
   public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
   #endregion
   ```

   **For the ServiceCD field**, add the PXDefault attribute and correct the display name:

   ```csharp
   #region ServiceCD
   [PXDBString(15, IsUnicode = true, IsKey = true, InputMask = ">aaaaaaaaaaaaaaa")]
   [PXDefault]
   [PXUIField(DisplayName = "Service ID")]
   public virtual string? ServiceCD { get; set; }
   public abstract class serviceCD : PX.Data.BQL.BqlString.Field<serviceCD> { }
   #endregion
   ```

   **For the Description field**, add the PXDefault attribute:

   ```csharp
   #region Description
   [PXDBString(50, IsUnicode = true)]
   [PXDefault]
   [PXUIField(DisplayName = "Description")]
   public virtual string? Description { get; set; }
   public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
   #endregion
   ```

   **For the Active field**, add the `[PXDefault(true)]` attribute:

   ```csharp
   #region Active
   [PXDBBool()]
   [PXDefault(true)]
   [PXUIField(DisplayName = "Active")]
   public virtual bool? Active { get; set; }
   public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
   #endregion
   ```

   **For WalkInService**, add the `[PXDefault(false)]` attribute:

   ```csharp
   #region WalkInService
   [PXDBBool()]
   [PXDefault(false)]
   [PXUIField(DisplayName = "Walk-In Service")]
   public virtual bool? WalkInService { get; set; }
   public abstract class walkInService : PX.Data.BQL.BqlBool.Field<walkInService> { }
   #endregion
   ```

   **For the PreliminaryCheck field**:

   ```csharp
   #region PreliminaryCheck
   [PXDBBool()]
   [PXDefault(false)]
   [PXUIField(DisplayName = "Requires Preliminary Check")]
   public virtual bool? PreliminaryCheck { get; set; }
   public abstract class preliminaryCheck : PX.Data.BQL.BqlBool.Field<preliminaryCheck> { }
   #endregion
   ```

   **For the Prepayment field**:

   ```csharp
   #region Prepayment
   [PXDBBool()]
   [PXDefault(false)]
   [PXUIField(DisplayName = "Requires Prepayment")]
   public virtual bool? Prepayment { get; set; }
   public abstract class prepayment : PX.Data.BQL.BqlBool.Field<prepayment> { }
   #endregion
   ```

   **For the system fields**, make sure that the following attributes are specified:

   | Field | Attribute |
   |-------|-----------|
   | CreatedDateTime | [PXDBCreatedDateTime()] |
   | CreatedByID | [PXDBCreatedByID()] |
   | CreatedByScreenID | [PXDBCreatedByScreenID()] |
   | LastModifiedDateTime | [PXDBLastModifiedDateTime()] |
   | LastModifiedByID | [PXDBLastModifiedByID()] |
   | LastModifiedByScreenID | [PXDBLastModifiedByScreenID()] |
   | Tstamp | [PXDBTimestamp()] |
   | NoteID | [PXNote()] |

   Remove the PXUIField attribute from the Tstamp field.

4. Save your changes.
5. Publish the customization project.

> If you want to publish a project after it has already been published and the Compilation pane is displayed, close the Compilation pane first.

**Related Links**

- Working with Attributes

##### Step 1.4.3: Define a Data View in the Graph

In the previous step, you've defined the needed DAC. Now you need to configure the view that will retrieve data from the database by using the defined DAC:

1. Open the RSSVRepairServiceMaint graph on the Code Editor page. To do this, in the navigation pane of the Customization Project Editor, click RSSVRepairServiceMaint under the Code node.
2. Add the following using directive:

   ```csharp
   using PX.Data.BQL.Fluent;
   ```

3. In the RSSVRepairServiceMaint graph, add the definition of the RepairService data view:

   ```csharp
   public SelectFrom<RSSVRepairService>.View RepairService = null!;
   ```

   You've declared the data view by using a fluent BQL query. For details, see Search and Select Commands and Data Views in Fluent BQL. You've used the null-forgiving operator in the data view definition. You will turn on the nullable reference types for the customization code in Lesson 1.9.

4. Replace the Save and Cancel actions:

   ```csharp
   public PXSave<RSSVRepairService> Save = null!;
   public PXCancel<RSSVRepairService> Cancel = null!;
   ```

5. Save your changes.
6. Publish the customization project.

**Related Links**

- Data View

---

### Lesson 1.5: Configure the UI of the Form

In this lesson, you'll define the presentation logic and layout of the Repair Services (RS201000) form.

#### Lesson Objectives

As you complete this lesson, you will learn how to define a form's presentation logic and layout in the Modern UI.

#### UI Definition in HTML and TypeScript: General Information

The structure of an Acumatica ERP form in the Modern UI is represented by the following layers:

- The presentation logic in a TypeScript (TS) file, which provides a definition of views and their settings
- The layout of UI elements displayed on the form in HTML

##### Applicable Scenarios

You define an Acumatica ERP form in HTML and TypeScript in the following cases:

- In a customization project, you have developed an Acumatica ERP form for the Classic UI. Now you need to convert this form to the Modern UI to continue supporting it in future versions of Acumatica ERP.
- You're developing a new Acumatica ERP form.

##### Controls of the Modern UI

Controls are building blocks for the layout of an Acumatica ERP form. Each control is composed of an HTML template and a TypeScript class.

A control can have the following attributes:

- **config:** An attribute whose properties define the control's appearance and behavior. Any changes to the values of these properties made in the browser are not passed to the server and can be overwritten by the server on each round trip.
- **value:** The value displayed in the control, which can be changed both in the browser and on the server.
- **id:** An identifier of the control, which is a shortcut for the id property of the config attribute.
- Other bindable attributes, which are shortcuts bound to the properties in the config attribute.

All controls can be divided into the following categories:

- **Simple controls:** Are bindable to server fields
- **Containers:** Hold other controls
- **Compound controls:** Are usually bindable to a view or have their own controller
- **Abstract controls:** Serve as a basis for other control types

##### Acumatica ERP Form in the Modern UI

You'll find the Modern UI source code of original Acumatica ERP forms in the `FrontendSources\screen\src\screens` folder of the Acumatica ERP instance folder.

Below you can see an example of the hierarchy of the files and folders of the Modern UI.

```
Site
- FrontendSources/screen/src/screens
- - GL
- - - GL401000
- - - - extensions (optional)
- - - - - GL401000_extension1.html
- - - - - GL401000_extension1.ts
- - - - - GL401000_extension2.html
- - - - - GL401000_extension2.ts
- - - - GL401000.html
- - - - GL401000.ts
- - - - views.ts (optional)
```

##### Screen Class in TypeScript

To define the views of an Acumatica ERP form in TypeScript, in the TS file of the form, you define a screen class--a class for the form--as shown in the following code.

```typescript
import { graphInfo, PXScreen } from "client-controls";

@graphInfo({
    graphType:'PX.Objects.GL.AccountHistoryEnq',
    primaryView:'Filter'
})
export class GL401000 extends PXScreen {
}
```

The screen class must satisfy these requirements:

- It has the screen ID as the name of the class, such as GL401000.
- It extends the PXScreen class.
- It has the graphInfo decorator, in which you specify the graph and its primary view.

In the screen class, you define a property for each data view:

```typescript
import { graphInfo, PXScreen, createSingle } from "client-controls";

@graphInfo({
    graphType:'PX.Objects.GL.AccountHistoryEnq',
    primaryView:'Filter'
})
export class GL401000 extends PXScreen {
    @viewInfo({containerName: 'Filter'})
    Filter = createSingle(GLHistoryEnqFilter);
}
```

##### View Classes in TypeScript

In the TS file of the form, you define a view class for each view of the graph. The class extends the PXView class.

```typescript
import { PXView } from "client-controls";

export class GLHistoryEnqFilter extends PXView {
}
```

In each view class, you specify the properties for all data fields of the data view:

```typescript
Description: PXFieldState;
ShowCuryDetail: PXFieldState<PXFieldOptions.Hidden | PXFieldOptions.CommitChanges>;
OrderDate: PXFieldState<PXFieldOptions.CommitChanges>;
```

##### Action Definitions in TypeScript

The actions defined in the graph or in the workflow have corresponding commands displayed on the More menu by default. You do not need to define them in the TypeScript code of the Acumatica ERP form.

##### Layout in HTML

In the HTML file, you define the layout of an Acumatica ERP form. You must place all HTML controls inside the template tag.

```html
<template>
    <qp-template id="form-Filter" name="7-10-7" class="equal-height">
        <qp-fieldset id="fsColumnA-Filter" slot="A" view.bind="Filter">
            <field name="OrderDate"></field>
            <field name="ShowCuryDetail"></field>
        </qp-fieldset>
        <qp-fieldset id="fsColumnB-Filter" slot="B" view.bind="Filter">
            <field name="Description"></field>
        </qp-fieldset>
    </qp-template>
    <qp-grid id="grid-Details" view.bind="transactions"></qp-grid>
</template>
```

##### Step 1.5.1: Export Modern UI Files to the Development Folder

Export the RS201000.html and RS201000.ts files to the development folder by doing the following:

1. Open the PhoneRepairShop customization project in the Customization Project Editor.
2. In the navigation pane, click Modern UI Files. The Modern UI Files page opens.
3. On the page toolbar, click Export to Development Folder. The system generates the development folder in the `FrontendSources\screen\src\` folder of your instance.

##### Step 1.5.2: Update the Screen Class in TypeScript

1. Open the RS201000.ts file and update the import directives:

   ```typescript
   import { PXScreen, graphInfo, createCollection } from "client-controls";
   ```

2. Update the screen class:

   ```typescript
   @graphInfo({
       graphType: "PhoneRepairShop.RSSVRepairServiceMaint",
       primaryView: "RepairService",
       hideFilesIndicator: true,
       hideNotesIndicator: true,
   })
   export class RS201000 extends PXScreen {}
   ```

3. Define the property for the data view:

   ```typescript
   export class RS201000 extends PXScreen {
       RepairService = createCollection(RSSVRepairService);
   }
   ```

4. Remove the boilerplate MasterView and DetailsView properties and classes.

##### Step 1.5.3: Define the View Class in TypeScript

```typescript
import { PXScreen, graphInfo, createCollection, PXView, PXFieldState, gridConfig, PXFieldOptions, GridPreset } from "client-controls";

@gridConfig({
    preset: GridPreset.Primary
})
export class RSSVRepairService extends PXView {
    ServiceCD: PXFieldState;
    Description: PXFieldState;
    Active: PXFieldState;
    WalkInService: PXFieldState<PXFieldOptions.CommitChanges>;
    Prepayment: PXFieldState;
    PreliminaryCheck: PXFieldState<PXFieldOptions.CommitChanges>;
}
```

##### Step 1.5.4: Define the Layout in HTML

```html
<template>
    <qp-grid id="grid-RepairService" view.bind="RepairService"></qp-grid>
</template>
```

---

### Lesson 1.6: Update Modern UI Files in the Customization Project

In this lesson, you'll update Modern UI files of the Repair Services (RS201000) form in the customization project.

#### Lesson Objectives

In this lesson, you'll learn how to update Modern UI files in the customization project.

##### Step 1.6.1: Update Modern UI Files in the Customization Project

1. Open the PhoneRepairShop customization project.
2. On the navigation pane, click Modern UI Files.
3. On the page toolbar, click Detect Modified Files.
4. In the Modified Files Detected dialog box, select the check boxes for RS201000.html and RS201000.ts.
5. Click Update Customization Project.
6. Publish the customization project.

##### Step 1.6.2: Test the Form

1. In Acumatica ERP, open the Repair Services form.
2. On the form toolbar, click Add Row.
3. Enter the following values:
   - **Service ID:** TestID
   - **Description:** Test Description
4. On the form toolbar, click Save.
5. Add the following rows:

   | Service ID | Description | Active | Walk-In Service | Requires Prepayment | Requires Preliminary Check |
   |------------|-------------|--------|-----------------|---------------------|---------------------------|
   | BatteryReplace | Battery Replacement | Selected | Cleared | Selected | Cleared |
   | LiquidDamage | Liquid Damage | Selected | Cleared | Selected | Selected |
   | ScreenRepair | Screen Repair | Selected | Selected | Cleared | Cleared |

6. On the form toolbar, click Save.
7. Click the TestID row in the table.
8. On the form toolbar, click Delete Row.
9. On the form toolbar, click Save.

---

### Lesson 1.7: Add an Event Handler to a Check Box

In Smart Fix, depending on the type of work to be done, a repair service can be:

- Performed right away: The Walk-In Service check box on the Repair Services (RS201000) form is selected.
- Performed after a preliminary check: The Requires Preliminary Check check box is selected.

This means that the check boxes on the completed Repair Services form must be mutually exclusive: If one is selected, the other must be cleared.

#### Lesson Objectives

As you complete this lesson, you'll learn how to add an event handler for a check box.

##### Step 1.7.1: Add an Event Handler in the Customization Project Editor

Multiple events can occur on a field of a record. The FieldUpdated event fires for each field of a record that is currently updated or inserted. This event type is typically used to modify other fields in the same data record.

Define the handler in the graph:

```csharp
protected void _(Events.FieldUpdated<RSSVRepairService, RSSVRepairService.walkInService> e)
{
    var row = e.Row;
    row.PreliminaryCheck = !(row.WalkInService == true);
}
```

##### Step 1.7.2: Specify the CommitChanges Property

Make sure the WalkInService field has the CommitChanges property specified:

```typescript
WalkInService: PXFieldState<PXFieldOptions.CommitChanges>;
```

##### Step 1.7.3: Test the Event Handler

1. Open the Repair Services (RS201000) form.
2. In the ScreenRepair row, clear the Walk-In Service check box.
3. Notice that the Requires Preliminary Check check box has been selected automatically.
4. Select the Walk-In Service check box in the ScreenRepair row.
5. Notice that the Requires Preliminary Check check box has been cleared automatically.
6. Save your changes.

---

### Lesson 1.8: Debug the Code Included in a Code Item

After you've added code to your customization project as a Code item, you can debug it by using Visual Studio--the only available debugging method for this code.

#### Lesson Objectives

In this lesson, you will learn how to debug code included in a Code item of a customization project by using Visual Studio.

##### Step 1.8.1: Debug the Customization Code

1. Open the Web.config file in the root folder of the SmartFix_T200 instance.
2. In the `<system.web>` tag, locate the `<compilation>` element and set debug to True:

   ```xml
   <system.web>
       <compilation debug="True" ...>
   ```

3. Launch Visual Studio as an administrator.
4. Click File > Open > Web Site and select the SmartFix_T200 instance folder.
5. In Solution Explorer, open the App_RuntimeCode folder and double-click RSSVRepairServiceMaint.cs.
6. Set a breakpoint on `var row = e.Row;`.
7. Click Debug > Attach to Process.
8. Select the w3wp.exe process and click Attach.
9. In a browser, open the Repair Services (RS201000) form.
10. In the ScreenRepair row, clear the Walk-In Service check box.
11. Press F10 to step through the handler.
12. Press F5 to continue.

---

### Lesson 1.9: Move the Customization Code to an Extension Library

In the previous lessons, you've learned how to work with customization project items in the Customization Project Editor. This lesson shows you how to integrate a customization project with Microsoft Visual Studio.

#### Lesson Objectives

As you complete this lesson, you'll learn how to:

- Create an extension library
- Open the extension library in Visual Studio
- Build the extension library in Visual Studio

#### Extension Libraries

To develop the customization code in Visual Studio, you need to use an extension library: a Visual Studio project that contains customization code and can be individually developed and tested.

Extension library .dll files must be located in the website's Bin folder. At runtime during website initialization, all .dll files in this folder are loaded into server memory for use by Acumatica ERP.

##### Step 1.9.1: Create an Extension Library

1. Open the PhoneRepairShop project in the Customization Project Editor.
2. On the main menu, click Extension Library > Create New.
3. Enter **PhoneRepairShop_Code** as the Project Name.
4. Click OK.

The platform creates solution files and project files for the extension library.

##### Step 1.9.2: Move Code from the Customization Project to the Extension Library

1. Open the PhoneRepairShop project.
2. In the Code node, select RSSVRepairServiceMaint.
3. On the page toolbar, click Move to Ext. Library.
4. Move the RSSVRepairService DAC to the extension library.
5. Ensure the Code page is empty.
6. Open PhoneRepairShop_Code.csproj and add:

   ```xml
   <LangVersion>9.0</LangVersion>
   <Nullable>enable</Nullable>
   ```

7. For each PX assembly reference, add `<Private>False</Private>`.

##### Step 1.9.3: Open the Solution in Visual Studio

1. Open the PhoneRepairShop project.
2. Click Extension Library > Open in Visual Studio.
3. Run the OpenSolution.bat batch file.
4. Ensure the project includes RSSVRepairServiceMaint and RSSVRepairService.
5. Add a DAC folder and move RSSVRepairService.cs there.
6. Remove Examples.cs.

##### Step 1.9.4: Build the Project in Visual Studio

1. Add assembly references for PX.Common.Std.dll, PX.Data.dll, and PX.Data.BQL.Fluent.dll.
2. Ensure `<Private>False</Private>` is specified for all PX references.
3. Build the PhoneRepairShop_Code project.

##### Step 1.9.5: Include the Extension Library in the Customization Project

1. Open the PhoneRepairShop project.
2. Click Files > Add New Record.
3. Select Bin\PhoneRepairShop_Code.dll and click Save.
4. Publish the customization project.

---

### Lesson 1.10: Add an Event Handler In Visual Studio

In this lesson, you'll add an event handler for the Requires Preliminary Check check box by using Visual Studio.

#### Lesson Objectives

You will learn how to add an event handler in Visual Studio.

##### Step 1.10.1: Add an Event Handler in Visual Studio

```csharp
protected void _(Events.FieldUpdated<RSSVRepairService, RSSVRepairService.preliminaryCheck> e)
{
    var row = e.Row;
    row.WalkInService = !(row.PreliminaryCheck == true);
}
```

##### Step 1.10.2: Test the Event Handlers (Self-Guided Exercise)

Test the behavior as you did in Step 1.7.3. Make sure the check boxes are selected and cleared automatically in the needed ways.

---

## Part 2: Creating a Form with Visual Studio

In this part of the course, you'll create the second form of the application: the Serviced Devices maintenance form. This form will provide information about devices that can be repaired in the Smart Fix company.

### Initial Steps

Before developing the form in Visual Studio, add the database table schema for the RSSVDevice table to the customization project.

---

### Lesson 2.1: Create a Graph and a DAC in Visual Studio

In this lesson, you'll create the items that you need to define the Serviced Devices maintenance form in Visual Studio: a DAC and a graph.

#### Lesson Objectives

As you complete this lesson, you will learn how to do the following:

- Define a graph in Visual Studio
- Use Acuminator code snippets to define a DAC and its fields in Visual Studio
- Define a selector control for one field and a drop-down list for another field

##### Step 2.1.1: Define the RSSVDeviceMaint Graph

```csharp
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVDeviceMaint : PXGraph<RSSVDeviceMaint>
    {
    }
}
```

##### Step 2.1.2: Create a DAC in Visual Studio

1. Add a Helper folder and Messages.cs file:

   ```csharp
   namespace PhoneRepairShop
   {
       public static class Messages
       {
           //DAC names
           public const string RSSVDevice = "Serviced Device";
           public const string RSSVRepairService = "Repair Service";
       }
   }
   ```

2. Create RSSVDevice.cs using Acuminator code snippet (type `dac`, tab twice):

   ```csharp
   using PX.Data;

   namespace PhoneRepairShop
   {
       [PXCacheName(Messages.RSSVDevice)]
       public class RSSVDevice : PXBqlTable, IBqlTable
       {
       }
   }
   ```

##### Step 2.1.3: Define a DAC Field with Acuminator Code Snippets

Use the `dacFldInt` snippet for DeviceID:

```csharp
#region DeviceID
public abstract class deviceID : BqlInt.Field<deviceID> { }
[PXDBIdentity]
public virtual int? DeviceID
{
    get;
    set;
}
#endregion
```

##### Step 2.1.4: Define Other DAC Fields in Visual Studio

**DeviceCD (selector field):**

```csharp
#region DeviceCD
[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = ">aaaaaaaaaaaaaaa")]
[PXDefault]
[PXUIField(DisplayName = "Device Code")]
[PXSelector(typeof(Search<RSSVDevice.deviceCD>),
    typeof(RSSVDevice.deviceCD),
    typeof(RSSVDevice.active),
    typeof(RSSVDevice.avgComplexityOfRepair))]
public virtual string? DeviceCD { get; set; }
public abstract class deviceCD : PX.Data.BQL.BqlString.Field<deviceCD> { }
#endregion
```

**Description:**

```csharp
#region Description
[PXDBString(256, IsUnicode = true, InputMask = "")]
[PXUIField(DisplayName = "Description")]
public virtual string? Description { get; set; }
public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
#endregion
```

**Active:**

```csharp
#region Active
[PXDBBool()]
[PXDefault(true)]
[PXUIField(DisplayName = "Active")]
public virtual bool? Active { get; set; }
public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
#endregion
```

**Constants for Complexity drop-down:**

```csharp
// Constants.cs
namespace PhoneRepairShop
{
    public static class RepairComplexity
    {
        public const string Low = "L";
        public const string Medium = "M";
        public const string High = "H";
    }
}
```

```csharp
// Messages.cs
public const string High = "High";
public const string Medium = "Medium";
public const string Low = "Low";
```

**AvgComplexityOfRepair (drop-down list):**

```csharp
#region AvgComplexityOfRepair
[PXDBString(1, IsFixed = true)]
[PXDefault(RepairComplexity.Medium)]
[PXUIField(DisplayName = "Complexity")]
[PXStringList(
    new string[]
    {
        RepairComplexity.Low,
        RepairComplexity.Medium,
        RepairComplexity.High
    },
    new string[]
    {
        Messages.Low, Messages.Medium, Messages.High
    })]
public virtual string? AvgComplexityOfRepair { get; set; }
public abstract class avgComplexityOfRepair : PX.Data.BQL.BqlString.Field<avgComplexityOfRepair> { }
#endregion
```

> For fields that store combo box values (such as AvgComplexityOfRepair), we recommend that you use a non-Unicode string field with a fixed length of one symbol: `[PXDBString(1, IsFixed = true)]`.

##### Step 2.1.5: Configure the RSSVDeviceMaint Graph

```csharp
public class RSSVDeviceMaint : PXGraph<RSSVDeviceMaint, RSSVDevice>
{
    public SelectFrom<RSSVDevice>.View ServDevices = null!;
}
```

---

### Lesson 2.2: Build the Form's UI

In this lesson, you will create HTML and TypeScript files that define the UI for the Serviced Devices (RS202000) form.

#### Lesson Objectives

As you complete this lesson, you will learn how to build HTML and TypeScript files for an Acumatica ERP form.

##### Step 2.2.1: Create the RS202000.ts File (Self-Guided Exercise)

Create the RS202000 folder and files, then define:

```typescript
import { createSingle, PXScreen, graphInfo, viewInfo, PXView, PXFieldState } from "client-controls";

@graphInfo({
    graphType: "PhoneRepairShop.RSSVDeviceMaint",
    primaryView: "ServDevices",
})
export class RS202000 extends PXScreen {
    @viewInfo({containerName: "Service Devices"})
    ServDevices = createSingle(RSSVDevice);
}

// View
export class RSSVDevice extends PXView {
    DeviceCD: PXFieldState;
    Description: PXFieldState;
    Active: PXFieldState;
    AvgComplexityOfRepair: PXFieldState;
}
```

#### Form Layout: Predefined Templates

You can use predefined templates to control the layout of areas of an Acumatica ERP form. Templates automatically adjust based on screen width and resolution.

##### Template Selection Guide

| Form Type | Template | Label Size |
|-----------|----------|------------|
| Data entry forms for transactions | Three-slot templates for Summary area | Default |
| Data entry forms for profiles | 1-1 | M |
| Processing forms | 17-17-14 or 17-14-17 | Default |
| Inquiry forms | 17-17-14 or 17-14-17 | Default |
| Setup forms | 1-1 | XM |
| Maintenance forms | 1-1 | XM |

##### Available Templates

| Template | Description |
|----------|-------------|
| 17-17-14 | Three slots--the third with short elements |
| 17-14-17 | Shows three slots; the second has shorter elements than the first and third do |
| 14-17-17 | Shows three slots; the first has shorter elements than the second and third do |
| 17-31 | Shows two slots; the first has shorter elements than the second does |
| 7-10-7 | Shows three slots; the second includes long elements |
| 10-7-7 | Shows three slots; the first includes long elements |
| 17-7 | Shows two slots; the first includes long elements |
| 7-17 | Shows two slots; the second includes long elements |
| 1-1-1 | Shows three slots with similar lengths of elements |
| 2-1 | Shows two slots; the first includes long elements |
| 1-2 | Shows two slots; the second includes long elements |
| 1-1 | Shows two slots with similar lengths of elements |
| 1 | Shows one slot with long elements |

##### Step 2.2.2: Define the Layout in HTML

```html
<template>
    <qp-template id="form-ServDevices" name="1-1">
        <qp-fieldset id="fsColumnA" slot="A" view.bind="ServDevices">
            <field name="DeviceCD"></field>
            <field name="Description"></field>
            <field name="Active"></field>
            <field name="AvgComplexityOfRepair"></field>
        </qp-fieldset>
    </qp-template>
</template>
```

#### Modern UI Development: Building the Source Code

##### Performing the Prerequisite Actions

Run in the FrontendSources folder:

```bash
npm run getmodules
```

##### Building the Source Code

For development:
```bash
npm run build-dev
```

For production:
```bash
npm run build
```

For specific forms:
```bash
npm run build-dev --- --env screenIds=SO301000
npm run build-dev --- --env modules="AR,AP,GL"
```

For development folder:
```bash
npm run build-dev --- --env customFolder=development
```

##### Automatically Rebuilding

```bash
npm run watch --- --env screenIds="SO301000,FS305100"
npm run watch --- --env customFolder=development screenIds="RS201000, RS202000"
```

##### Step 2.2.3: Build the Modern UI Code of the Form

```bash
npm run build-dev --- --env customFolder=development screenIds=RS202000
```

##### Step 2.2.4: Add Modern UI Files to the Customization Project

1. Open the PhoneRepairShop project.
2. Click Modern UI Files > Add New Record.
3. Select:
   - development\screens\RS\RS202000\RS202000.html
   - development\screens\RS\RS202000\RS202000.ts
4. Click Save.
5. Publish the customization project.

---

### Lesson 2.3: Add the Form to the Site Map and Workspace

#### Lesson Objectives

- Create a site map item for a custom form and configure the form's access rights
- Save the created site map item and the form's access rights to the customization project

##### Step 2.3.1: Create a Site Map Item for the Form

1. Open the PhoneRepairShop customization project.
2. Click Site Map > Manage Site Map.
3. Add a row with:
   - **Screen ID:** RS202000
   - **Title:** Serviced Devices
   - **URL:** ~/Scripts/Screens/RS202000.html
   - **Graph Type:** PhoneRepairShop.RSSVDeviceMaint
   - **Workspaces:** Phone Repair Shop
   - **Category:** Configuration

##### Step 2.3.2: Configure Access Rights for the Form

1. Open Access Rights by Screen (SM201020).
2. Expand Phone Repair Shop > Serviced Devices.
3. Change Customizer access rights from Revoked to Delete.
4. Save and refresh.

##### Step 2.3.3: Include the Form's Access Rights in the Customization Project

1. Open the PhoneRepairShop project.
2. Click Access Rights > Add New Record.
3. Select RS202000 and click Add.

##### Step 2.3.4: Add the Site Map Item to the Customization Project

1. Add Serviced Devices to the quick menu.
2. In the Customization Project Editor, click Site Map > Add New Record.
3. Select the Serviced Devices form and click Save.

##### Step 2.3.5: Test the Form

**Adding Records:**

| Device Code | Description | Active | Repair Complexity |
|-------------|-------------|--------|-------------------|
| NOKIA3310 | Nokia 3310 | Selected | Low |
| MotorRAZR | Motorola RAZR V3 | Selected | Low |
| SamsungGS4 | Samsung Galaxy S4 | Selected | Medium |
| iPhone6 | iPhone 6 | Selected | High |

---

### Lesson 2.4: Create a Substitute Form

At this point, a user can access a specific device record by opening the form and using the selector. With a small number of devices this works fine, but as the number of records grows, users may struggle to find a device quickly.

In Acumatica ERP, you can create a generic inquiry that presents the data of records entered on a maintenance form in a tabular format. You can then define the generic inquiry as a substitute form.

#### Lesson Objectives

In this lesson, you'll learn how to create a substitute form for a custom form and save it to the customization project.

##### Step 2.4.1: Upload a Predefined Generic Inquiry

1. Open Generic Inquiry (SM208000).
2. Click Clipboard > Import from XML.
3. Select `Customization\T200\SourceFiles\ListAsEntryPoint\GI_ServicedDevices.xml`.
4. Click Upload.

##### Step 2.4.2: Configure the Generic Inquiry as a Substitute Form

1. Open the Serviced Devices generic inquiry.
2. Go to the Entry Point tab.
3. In the Entry Screen box, select Serviced Devices (RS202000).
4. Select Replace Entry Screen with this Inquiry in Menu and Enable New Record Creation.
5. Save.

##### Step 2.4.3: Save the Generic Inquiry to the Customization Project

1. Open the PhoneRepairShop project.
2. Click Generic Inquiries > Add New Record.
3. Select the Serviced Devices generic inquiry and click Save.
4. Also add access rights for RS.20.20.PL.
5. Publish the customization project.

##### Step 2.4.4: Test the Substitute Form

1. Select Phone Repair Shop workspace.
2. Click Serviced Devices. The RS2020PL list opens.
3. Click MotorRAZR link. The RS202000 maintenance form opens.
4. Select Active, click Save and Close.
5. Verify the change in the list.

---

## Appendix: Customization Projects

In this topic, you can find links to additional information related to customization projects.

### Types of Items in a Customization Project

When you customize an instance of Acumatica ERP by using the Customization Project Editor, the platform stores all items of a customization project as records in the CustObject table of the database. Each record contains the data of an item, including the XML code of the item in a specific field.

For details, see Types of Items in a Customization Project.

### Deployment of a Customization Result

Once you've finished a customization project, you can export the project as a deployment package that can then be imported and published as a customization project in any Acumatica ERP instance.

For more information, see Deployment of a Customization Result.

### Simultaneous Management of Multiple Customization Projects

With the Acumatica Customization Platform, you can simultaneously manage multiple customization projects by using the Customization Projects (SM204505) form. You can publish multiple customization projects to an Acumatica ERP instance at once.

For details, see Project Publication: General Information.

### Publication of Customization Projects in a Multitenant Site

A customization project is stored in the instance database. The data of each tenant that uses the same instance of Acumatica ERP is isolated from the data of other tenants in the database. However, the website files of the instance are shared by all tenants.

For more information, see Publication of Customization Projects in a Multitenant Site.

### Unpublishing of Customization Projects

When you unpublish all customization projects, the system reverses the changes introduced by the customization project as follows:

- The forms of Acumatica ERP return to their original layout.
- The .cs files of the project with customization code are removed from the website folder in the file system.
- The custom files of these projects are removed from the website folder on the file system.

For details about the changes that cannot be unpublished, see Unpublishing Customization Projects.
