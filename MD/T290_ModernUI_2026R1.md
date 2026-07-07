# Developer Course

## Customization

## T290 Modern UI for Developers
*2026 R1*

*Revision: 4/6/2026*

Contents
- Copyright
- How to Use This Course
- Company Story and Customization Description
- Initial Configuration
To Deploy an Instance for the Training Course
## Part 1: Getting Started with the Modern UI
### Lesson 1.1: Getting Acquainted with the Modern UI
- Modern UI Development: General Information
- Modern UI Development: Switching a Form Between the Modern UI and the Classic UI
### Lesson 1.2: Building the Sources for the First Time
- Modern UI Development: Building the Source Code
#### Activity 1.2.1: To Build the Source Code of All Acumatica ERP Forms for Modern UI Development
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript
### Lesson 2.1: Understanding UI Definition in HTML and TypeScript
- UI Definition in HTML and TypeScript: General Information
#### Activity 2.1.1: To Create the UI of a Form
#### Activity 2.1.2: To Build the Source Code of a Particular Form for the Modern UI Development
- Table (Grid): Configuration of the Table and Its Columns
### Lesson 2.2: Converting an Acumatica ERP Form with the Converter
- UI Definition in HTML and TypeScript: Form Converter
#### Activity 2.2.1: To Convert an Acumatica ERP Form to the Modern UI with the Converter
- Fieldset: Field Configuration
- UI Definition in HTML and TypeScript: Joined Fields
## Part 3: Designing the Layout of an Acumatica ERP Form
### Lesson 3.1: Understanding Approaches to Organize Layout
- Form Layout: General Information
- Form Layout: Predefined Templates
- Form Layout: Grid Presets
- Form Layout: CSS Classes
- Form Layout: Box Connotations
### Lesson 3.2: Converting a Setup Form
- UI of a Setup Form: General Information
- UI of a Setup Form: A Form with Only a Summary Area
- UI of a Setup Form: A Form with Tabs

#### Activity 3.2.1: To Create the UI of a Setup Form
- Selector Control: Configuration of a Link
### Lesson 3.3: Converting a Data Entry Form
- Data Entry Form: General Information
- Data Entry Form: Definition in TypeScript and HTML
#### Activity 3.3.1: To Create the UI of a Data Entry Form
- Collapsible Area: Configuration
- Text Box: Multiline Text Box
- Record Title: Configuration
### Lesson 3.4: Converting a Processing Form
- Processing Form: General Information
- Processing Form: UI Guidelines
- Processing Form: A Form with Only a Grid
- Processing Form: A Form with a Selection Area and a Grid
#### Activity 3.4.1: To Create the UI of a Processing Form
### Lesson 3.5: Organizing Complex Layouts
- Form Layout: An Element Next to Another Element
- Fieldset: Layout Examples
- Button: Configuration
#### Activity 3.5.1: To Put a Button in the Summary Area
- Table (Grid): Layout Examples
- Table (Grid): Configuration of the Table Toolbar
- Table (Grid): Configuration of the Search in the Table
- Date and Time Control: Configuration
- Error, Warning, or Informational Notification: Configuration
- Tab: Configuration
## Part 4: Localizing the Modern UI
### Lesson 4.1: Localizing the Modern UI
- UI Localization: General Information
- UI Localization: TypeScript and HTML Code of the Modern UI
## Part 5: Troubleshooting the Modern UI
### Lesson 5.1: Troubleshooting the Modern UI
- Modern UI Troubleshooting: General Information
- Modern UI Troubleshooting: Developer Tools in the Browser
- Modern UI Troubleshooting: Unreflected Changes in the Modern UI
- Modern UI Troubleshooting: Build Options

## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript
### Lesson 6.1: Customizing the Modern UI
- UI Customization Development: General Information
#### Activity 6.1.1: To Add Elements to an Acumatica ERP Form
#### Activity 6.1.2: To Add a Tab to an Acumatica ERP Form
- UI Adjustments in HTML and TypeScript: HTML Examples
- UI Adjustments in HTML and TypeScript: TypeScript Examples
### Lesson 6.2: Including the Modern UI Changes in a Customization Project
- Customization Project with UI Changes: General Information
- Customization Project with UI Changes: How UI Customization Works
#### Activity 6.2.1: To Include Source Files in a Customization Project
- Customization Project with UI Changes: Custom Plugin
## Part 7: Using Advanced Techniques
### Lesson 7.1: Reusing UI Definitions
- Reusing of UI Definitions: General Information
- Reusing of UI Definitions: Creation of a Reusable UI Definition
- Reusing of UI Definitions: Reusable UI Definitions with Parameters
### Lesson 7.2: Handling UI Events
- UI Events: General Information
- UI Events: Changing of the CSS for a Row of a Table
- UI Events: Debugging the UI Code in the Browser
#### Activity 7.2.1: To Implement and Debug an Event Handler
- UI Events: Changing of Availability of a UI Element
- UI Events: Handling of Data That Is Unrelated to Any View
### Lesson 7.3: Using View Parameters
- UI Definition in HTML and TypeScript: View Parameters in the viewInfo Decorator
## Part 8: Testing the Modern UI
### Lesson 8.1: Testing the Modern UI
- Testing of the Modern UI: General Information
- Testing of the Modern UI: Names of WG Containers
- Testing of the Modern UI: Update of Tests Written for the Classic UI
- Testing of the Modern UI: Frontend Actions in Wrappers
- Appendix: Initial Configuration

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
**Last Updated: 04/06/2026**

How to Use This Course
The T290 Modern UI for Developers training course shows you how to define the Modern UI of Acumatica ERP.
The course is intended for application developers who are starting to learn how to customize the Modern UI of
Acumatica ERP.
The course is based on a set of examples that demonstrate the general approach to customizing the Modern
UI. The course is designed to give you ideas about how to develop the Modern UI for your own embedded
applications. It demonstrates the implementation of the Modern UI for new Acumatica ERP forms and for the
customization of existing Acumatica ERP form. As you go through the course, you will continue the development of
the customization for the cell phone repair shop, which was performed in the training courses of the T series (which
we recommend that you take before completing the current course).
Aer you complete all the lessons of the course, you will be familiar with the programming techniques used to
define and customize the Modern UI of Acumatica ERP.

We recommend that you complete the examples in the presented order because some examples use
the results of previous ones.

What the Course Prerequisites Are
To complete this course, you should be familiar with the basic concepts of Acumatica Framework. Also, we
recommend that you complete the T200 Maintenance Forms, T210 Customized Forms and Master-Details
Relationships, T220 Data Entry and Setup Forms, T230 Actions, T240 Processing Forms, T250 Inquiry Forms, and T270
Workflow API training courses before you begin this course.
To complete the course successfully, you should have the following required knowledge:
• Familiarity with the basics of Node.js
• Understanding of the Model-View-ViewModel (MVVM) pattern
• Basic knowledge of JavaScript
• Understanding of TypeScript concepts, such as interface, type, classes, enum, generic, and union
• Knowledge of HTML fundamentals
• Knowledge of the debugging in browser

What Is in a Lesson
Each lesson focuses on a particular development scenario that you can implement by using Acumatica ERP
customization tools and the Acumatica Framework. Each lesson consists of a brief description of the scenario and
an example of its implementation.

Where the Source Code Is
You can find the source code of the customization described in this course and code snippets for the course in the
ModernUI\T290 folder of the Help-and-Training-Examples repository in Acumatica GitHub.

What the Documentation Resources Are
**The complete documentation for Acumatica ERP and the Acumatica Framework is available at https://**
help.acumatica.com/ and is included in the Acumatica ERP instance. While viewing any form used in the course, you
can click Open Help in the top pane of the Acumatica ERP screen to bring up a form-specific Help menu; you can

use the links on this menu to quickly access form-related information and activities and to open a reference topic
with detailed descriptions of the form elements.

Which License You Should Use
For the educational purposes of this course, you use Acumatica ERP under the trial license, which does not require
activation and provides all available features. For the production use of the Acumatica ERP functionality, an
administrator has to activate the license the organization has purchased. Each feature may be subject to additional
licensing; please consult the Acumatica ERP licensing policy for details.

Company Story and Customization Description
In this course, you will continue the development to support the cell phone repair shop of the Smart Fix company;
you began this development while completing the previous training courses of the T series.

You will load and publish the customization project with the results of these courses in To Deploy an
Instance for the Training Course.

In the previous training courses of the T series, you have created the following forms:
• The Repair Services (RS201000) custom maintenance form, which the Smart Fix company uses to manage
the lists of repair services that the company provides
• The Serviced Devices (RS202000) custom maintenance form, which the Smart Fix company uses to manage
the lists of devices that can be serviced
• The Services and Prices (RS203000) custom maintenance form, which provides users with the ability to
define and maintain the price for each provided repair service
• The Repair Work Orders (RS301000) custom data entry form, which is used to create and manage work
orders for repairs
• The Repair Work Order Preferences (RS101000) custom setup form, which an administrative user uses to
specify the company's preferences for the repair work orders
• The Assign Work Orders (RS501000) custom processing form, which users will use to assign multiple repair
work orders at the same time
In the previous training courses of the T series, you have also customized the Stock Items (IN202500) form to mark
particular stock items as repair items—that is, items that are used for the repair services.
In this training course, you will develop the Modern UI for these forms.

Initial Configuration
You need to perform prerequisite actions before you start to complete the course.

To Deploy an Instance for the Training Course

The following activity will walk you through the process of preparing and deploying an Acumatica ERP instance that
you can use to perform the steps in the lessons of this training course.

Story
To perform customization tasks and complete the activities described in the lessons of this training course, you
need to deploy an instance of Acumatica ERP with the PhoneRepairShop customization project published and the
Modern UI turned on.

Process Overview
In this activity, you will prepare the environment and install tools that will help you to perform customization tasks.
You will then deploy an instance of Acumatica ERP with the PhoneRepairShop customization project published and
the dataset from the T240 Processing Forms course.

Step 1: Preparing the Environment

If you have completed any of the training courses of the T series and are using the same environment
for the current course, you can skip this step.

Before you begin deploying the needed Acumatica ERP instance, do the following:
1. Make sure that the environment you’re going to use conforms to the System Requirements for the Acumatica
ERP Installation.
2. Make sure that the Web Server (IIS) features listed in Configuration of IIS Web Server Features are turned on.
3. Install Microso Visual Studio Code.
4. Clone or download the customization project and the source code of the extension library from the Help-
and-Training-Examples repository in Acumatica GitHub to a folder on your computer.
5. Install Acumatica ERP. On the Main Soware Configuration page of the Acumatica ERP Setup wizard, select
the Install Acumatica ERP check box.

Step 2: Deploying the Instance
To perform customization tasks, you need to deploy an instance of Acumatica ERP for the T290 Modern UI for
Developers training course on the instance.
**You deploy an Acumatica ERP instance and configure it as follows:**
1. Open the Acumatica ERP Configuration wizard, and do the following:
a. Click Deploy a New Acumatica ERP Instance for T-Series Developer Courses.
b. On the Instance Configuration page, do the following:
a. In the Training Course box, select T290 Modern UI for Developers.

b. In the Local Path to the Instance box, select a folder that’s outside of the C:\Program Files
(x86), C:\Program Files, and C:\Users folders. (We recommend that you store the website
folder outside of these folders to avoid an issue with permission to work in these folders when you
customize the website.)
c. On the Database Configuration page, make sure the name of the database is SmartFix_T290.
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
• Username: admin
• Password: setup
Change the password when the system prompts you to do so.
4. In the top right corner of the Acumatica ERP screen, click the username and then My Profile. The User
Profile (SM203010) form opens. On the General Info tab, under Personal Settings, select YOGIFON in the
Default Branch box; then click Save on the form toolbar.
In subsequent sign-ins to this account, you’ll be signed in to this branch.
5. Optional: Add the Customization Projects (SM204505), Site Map (SM200520), and Generic Inquiry (SM208000)
forms to your favorites. For details about how to add a form to your favorites, see The Acumatica ERP UI:
Favorites.

If for some reason you cannot complete instructions in this step, you can create an Acumatica ERP
instance and manually publish the needed customization project, as described in Appendix: Initial
Configuration.
## Part 1: Getting Started with the Modern UI | 11

## Part 1: Getting Started with the Modern UI
In this part, you will find an overview of the main principles of the Modern UI of Acumatica ERP and learn about its
architecture and features. You’ll also learn how to:
• Use the development folder to create the Modern UI source files for custom and customized forms
• Build the source code for the development of the Modern UI
• Switch a form between the Modern UI and the Classic UI

### Lesson 1.1: Getting Acquainted with the Modern UI

In this lesson, you will learn how to do the following:
• Enable the Modern UI while deploying an instance
• Switch a form between the Modern UI and the Classic UI

**Modern UI Development: General Information**

The Modern UI is a .NET-compatible product that delivers updated UI capabilities without relying on ASPX pages.
On the server side, the Modern UI is represented by web services. On the client side, it’s represented by a template-
based single-page application (SPA) framework based on Aurelia.
The application code is written in TypeScript. The framework transcribes this code into JavaScript code for
execution in the web browser. This approach simplifies code maintenance. Developers use HTML and CSS to design
form layouts.

Applicable Scenarios
**You will work with the Modern UI in the following cases:**
• You need to make it possible to use the Modern UI for your instance.
• You need to convert any number of forms of your Acumatica ERP instance to the Modern UI.
• You need to create a new form that is based on the Modern UI.
• You need to be able to switch between the Modern UI and the Classic UI of a form.

Enabling of the Modern UI in the Acumatica ERP Configuration Wizard
By default, the system uses the Modern UI for a newly deployed instance. That is, the Use Modern UI as Default
check box is selected on the Website Configuration page of the Acumatica ERP Configuration wizard, as shown
below.
## Part 1: Getting Started with the Modern UI | 12

Make sure that you have the Install NodeJS check box selected so that the Acumatica ERP
Configuration wizard installs the needed version of Node.js for compilation of the customization code
of the Modern UI. If you want to use the version of Node.js that has already been installed in your
system, you can clear the Install NodeJS check box and add the following key to the appSettings
section of the Web.config file of your instance: <add key="NodeJs:NodeJsPath"
value="C:\Program Files\NodeJs"/>. In this key, value specifies the path to the location
where Node.js has been installed.

When the Use Modern UI as Default check box is selected, the system sets the value of the Default UI box to
Modern on the Site Preferences (SM200505) form.

Architecture of the Modern UI
The architecture of the Modern UI is based on the Model-View-ViewModel (MVVM) pattern, with the parts of the
architecture represented as follows:
• A view is represented by an HTML template.
• A view model is represented by the code written in TypeScript.
• A model is represented by a graph on the server that exchanges data from the client side.
The following diagram shows the architecture of the Modern UI and the interaction between its components.
## Part 1: Getting Started with the Modern UI | 13

Currently, the Modern UI is embedded in the infrastructure of the Classic UI. The Modern UI is represented by a
new frame that enables TypeScript controls in the Classic UI infrastructure. Also, the Modern UI includes web API
controllers that are added to the controllers of the Classic UI. Some parts of the Modern UI have been connected
to the Classic UI architecture via an Aurelia adapter, which adapts TypeScript's controllers to work in the Classic UI
architecture. The forms that are fully converted to the Modern UI (highlighted in yellow in the preceding diagram)
work directly with web API controllers.
JSON serves as the protocol between the client side and the server side. As a result, all requests can be seen in a
unified format and used for debugging.

Capabilities of the Modern UI
The Modern UI provides a variety of new capabilities for both developers and users. For developers, the Modern UI
provides the following capabilities in comparison to the Classic UI:
• The template (HTML and CSS) and presentation logic layers (TypeScript) are fully customizable.
• The client-side model can be programmed by using the event-driven model, which is similar to the server-
side model.
• The graph code generally does not require any modifications. The Modern UI and the Classic UI share the
same graph.
• The developers can always switch between the Modern UI and the Classic UI.
For users, the Modern UI provides exciting new features. For details, see the following topics:
## Part 1: Getting Started with the Modern UI | 14

• Modern UI: Changes in UI Elements
• Modern UI: Filters
• Modern UI: User Personalization
• Modern UI: System-Wide Form Configuration
• Modern UI: Managing User-Defined Fields

**Modern UI Development: Switching a Form Between the Modern UI and the Classic**
UI

Acumatica ERP provides a number of ways that you can use to switch an Acumatica ERP form that’s using the
Modern UI to the Classic UI or vice versa.
When you deploy or update an instance of Acumatica ERP in the Acumatica ERP Configuration wizard, make sure
the Use Modern UI as Default check box is selected (the default state), as shown below.

**Figure: Selection of the Modern UI as the default interface**

Before you use any of the methods described in the following sections, you may need to make a change to
the Web.config file of your Acumatica ERP instance. In the appSettings section, check if the <add
key="EnableSiteMapSwitchUI" value="False" /> key has been added. If it has, remove this key
from the file and save your changes. This is because this key disables the option to switch the UI of a form in the
instance. By removing the key, you enable the option to switch the UI of a form.

If you switch a form's UI by using the methods described below, the form's UI will be switched for all
users. Thus, this capability is not provided to all users. Only users with the Edit level of access rights to
the Site Map (SM200520) form can switch the UI of a form.
## Part 1: Getting Started with the Modern UI | 15

Switching the UI of the Particular Form
While viewing a form in the Modern UI, you can click the Settings button on the form title bar and then Switch to
Classic UI to switch the form to the Classic UI.

**Figure: The Switch to Classic UI command**

While viewing a form in the Classic UI, you can click Tools > Switch to Modern UI on the form title bar to switch the
form to the Modern UI.

**Figure: The Switch to Modern UI command**

The Switch to Modern UI command is available for only the forms that have been migrated to the
Modern UI.

Switching the UI of the Entire Site
To specify the user interface for all forms, use the Default UI setting on the Site Preferences (SM200505) form. It
defines the UI to be used by default (Modern UI or Classic UI) for all users of the current tenant.
## Part 1: Getting Started with the Modern UI | 16

**Figure: The Default UI setting**

The form will use the interface specified as the default UI, if the form supports the selected UI.

Switching the UI of Multiple Forms
You can use the Site Map (SM200520) form to specify the default UI to be displayed for any number of forms.
To cause a form to be displayed in the Modern UI or the Classic UI, you select Modern, Classic, or Default in the UI
column of the row that corresponds to the form, as shown in the following screenshot.
## Part 1: Getting Started with the Modern UI | 17

**Figure: The options in the UI column**

The Site Map form also has the Copy UI Settings to Tenants button on the form toolbar. By using this button, you
can copy the UI settings of all the listed forms to other tenants. When you click this button, the system displays a
dialog box where you can select the tenants to which you want to copy the UI settings.

### Lesson 1.2: Building the Sources for the First Time

In this lesson, you will learn how to do the following:
• Verify that certain prerequisite installation and configuration tasks have been performed before building the
UI from the source code
• Build the Modern UI from the source code
• Automatically rebuild the source code for a form when its source files are modified and saved

**Modern UI Development: Building the Source Code**

When you want to start migrating Acumatica ERP forms to the Modern UI or you want create a new form based on
it, you need to build the source code of each form to see its Modern UI version.

Performing the Prerequisite Actions
Before you begin building the source code for the first time, you do the following:
1. Make sure you have Node.js installed on your computer. By default, the Acumatica ERP Configuration wizard
installs it when you deploy a new application instance.
2. Run the following command in the FrontendSources folder. You can use the terminal in Visual Studio
Code, Windows PowerShell, or a similar program.

npm run getmodules
## Part 1: Getting Started with the Modern UI | 18

This command installs the required dependencies needed to build the sources correctly.

The system performs this instruction automatically during the first publication of a
customization project that contains any Modern UI files.

Building the Source Code for All Acumatica ERP Forms
To build the sources in the FrontendSources folder of your instance, you need to run a command in the same
folder. This command generates the form schema and JavaScript code from the TypeScript code and creates the
mapping between the JavaScript and TypeScript code. You use this mapping to debug the client code in a web
browser.
To build the sources during the UI development, you run the following command in the FrontendSources
folder.

npm run build-dev

To build the sources in production mode, you run the following command in the FrontendSources folder.

npm run build

The system performs this instruction automatically during the publication of a customization project
that contains any Modern UI files.

We recommend that you use the build-dev command instead of the build command during the development.
This provides easier debugging in a web browser.

Building the Source Code for Particular Acumatica ERP Forms
To speed up the build process, you can initiate the build for only specific forms instead of all of them. To do this,
you can run one of the following modified versions of the command (instead of the one in the preceding section) in
the FrontendSources\screen folder:
•    npm run build-dev --- --env screenIds=SO301000

This command above builds the sources for only the Sales Orders (SO301000) form.
•    npm run build-dev --- --env modules="AR,AP,GL"

This command builds the sources for only the forms in the specified functional areas (AR, AP, and GL in this
example).

You need to use quotation marks if you specify more than one form or area to be built.

Building the Source Code in the Development Folder for Custom and Customized Forms
To build the source code for all the forms you’ve created or customized in the development folder, you run the
following command in the FrontendSources\screen folder.

npm run build-dev --- --env customFolder=development

You can initiate the build for only specific forms instead of all of them. To do this, you run the following command.
## Part 1: Getting Started with the Modern UI | 19

npm run build-dev --- --env customFolder=development screenIds=SO301000

As with the example in the preceding section, you can use the modules parameter instead of the
screenIds parameter to build the sources for only the forms in specific functional areas.

The command above builds the sources for only the Sales Orders (SO301000) form, which you may have
customized in the development folder.
To create new forms or customize existing forms for the Modern UI, you use the development folder located in
the FrontendSources\screen\src\ folder of your instance. For details on creating the source files for these
forms in the development folder, see Modern UI Development: Creating Modern UI Source Files for Custom and
Customized Forms. For details on including the source files from the development folder in your customization
project, see Including the Modern UI Changes in a Customization Project.

For troubleshooting errors during the building of the Modern UI, you can use additional build options.
For details, see Modern UI Troubleshooting: Build Options.

Automatically Rebuilding the Source Code for Particular Acumatica ERP Forms
During the migration of the needed forms to the Modern UI, you make changes to the generated source files in
the FrontendSources\screen folder of your instance. To see the changes you make to a form, you’ll need
to rebuild the corresponding source files every time a change is made. To save time, you can run a command that
causes the source files to be rebuilt automatically each time a file is modified and saved in this folder.
To automatically rebuild the sources for only specific forms, you can run one of the following commands in the
FrontendSources\screen folder once:
•    npm run watch --- --env screenIds="SO301000,FS305100"

This command automatically rebuilds the sources for only the Sales Orders (SO301000) and Service Contract
Schedules (FS305100) forms when their corresponding source files are modified and saved.
•    npm run watch --- --env modules="AR,AP,GL"

This command automatically rebuilds the sources for only the forms in the specified functional areas (AR,
AP, and GL in this example).
•    npm run watch --- --env customFolder=development screenIds="RS201000, RS202000"

This command automatically rebuilds the sources you’ve created in the development folder for only the
forms whose screen IDs have been specified in the screenIds parameter (RS201000 and RS202000 in this
example). You can also use the modules parameter instead of screenIds to automatically rebuild the
sources for only the forms in specific functional areas.

To run the watch command from the root folder of your instance, you can navigate to the root folder
in Windows PowerShell and run the following command along with the screenIds or modules
parameter.

npm run watch --prefix .\FrontendSources\screen\

We recommend that you not use the watch command without the screenIds or modules
parameter because the command may behave in an unstable manner.
## Part 1: Getting Started with the Modern UI | 20

#### Activity 1.2.1: To Build the Source Code of All Acumatica ERP Forms for Modern UI
Development

The following activity will walk you through the process of initially building the source code for all the forms that
are available in the Modern UI.

Story
Suppose that you are going to develop the Modern UI for new Acumatica ERP forms or customize the Modern UI of
existing forms for the Smart Fix company. Before you start your development, you need to rebuild all the Modern UI
sources in the FrontendSources folder of your instance.

Process Overview
You will execute a command to build the source code for the first time.

System Preparation
Before you build the source code for the first time, use Windows PowerShell or a similar program to run the
following command in the FrontendSources folder of your instance: npm run getmodules. This command
installs the required dependencies needed to build the sources correctly.

**Step: Building the Source Code for All Acumatica ERP Forms**
When you build the sources in the FrontendSources folder of your instance for all Acumatica ERP forms that are
available in the Modern UI, the system generates the form schema and JavaScript code from the TypeScript code
and creates the mapping between the JavaScript and TypeScript code. You can then use this mapping to debug the
client code in a web browser.
To build the sources, run the npm run build-dev command in the FrontendSources folder. Note that this
command may take some time to finish its execution.
Once the command has finished executing, you should see a message that a webpack has been successfully
compiled.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 21

## Part 2: Defining Acumatica ERP Forms in HTML and
TypeScript
In this part, you will learn about the main components of the Modern UI, which include the definition of views of
the form in TypeScript and the layout definition in HTML. You can also find information about how to convert an
ASPX file of an Acumatica ERP form to HTML and TypeScript.

### Lesson 2.1: Understanding UI Definition in HTML and TypeScript

In this lesson, you will learn how to define the presentation logic and layout of a form in the Modern UI.

**UI Definition in HTML and TypeScript: General Information**

**The structure of an Acumatica ERP form in the Modern UI is represented by the following layers:**
• The presentation logic in a TypeScript (TS) file, which provides a definition of views and their settings
• The layout of UI elements displayed on the form in HTML

Applicable Scenarios
**You define an Acumatica ERP form in HTML and TypeScript in the following cases:**
• In a customization project, you have developed an Acumatica ERP form for the Classic UI. Now you need to
convert this form to the Modern UI to continue supporting it in future versions of Acumatica ERP.
• You’re developing a new Acumatica ERP form.

Controls of the Modern UI
Controls are building blocks for the layout of an Acumatica ERP form. Each control is composed of an HTML
template and a TypeScript class.
**A control can have the following attributes:**
• config: An attribute whose properties define the control’s appearance and behavior. Any changes to the
values of these properties made in the browser are not passed to the server and can be overwritten by the
server on each round trip.
• value: The value displayed in the control, which can be changed both in the browser and on the server.
• id: An identifier of the control, which is a shortcut for the id property of the config attribute.
• Other bindable attributes, which are shortcuts bound to the properties in the config attribute.
You can change config directly in HTML. You can also specify particular properties defined in config:
• As a set by using config.bind, as the following code shows: config.bind="{imageSet: 'main',
imageKey: 'Refresh'}"
• Individually, as the following code shows: config-allow-edit.bind="true"

If you specify the value for a single property, you need to transform the name of the property that
is available in config. For example, suppose that the propertyName property is available in
config. To specify a single property, you transform its name to config-property-name.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 22

**All controls can be divided into the following categories:**
• Simple controls: Are bindable to server fields
• Containers: Hold other controls
• Compound controls: Are usually bindable to a view or have their own controller
• Abstract controls: Serve as a basis for other control types
For simple controls, you typically don’t specify their type, such as qp-checkbox, in HTML. Instead, you
use the field tag in HTML. The server automatically defines the type of the field in the Modern UI. The
PX*FieldAttribute attribute assigned to the field in the backend code creates a specific type, which is an
inheritor of PXFieldState. This type aﬀects the default control used by the client.

You can’t use shortened versions of custom HTML tags, such as <qp-grid .../> or <qp-
button .../>. An HTML limitation prohibits shortened version of tags except for a limited number
of standard HTML tags.

Acumatica ERP Form in the Modern UI
You’ll find the Modern UI source code of original Acumatica ERP forms in the FrontendSources\screen\src
\screens folder of the Acumatica ERP instance folder.
Below you can see an example of the hierarchy of the files and folders of the Modern UI.

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

The FrontendSources\screen\src\screens folder contains subfolders with two-letter names. Each
subfolder includes the source code of the forms whose screen IDs start with these letters. Inside each subfolder is a
folder named aer the screen ID, such as GL401000. This folder contains HTML and TS files named aer the same
screen ID—for example, GL401000.ts and GL401000.html. For large forms with many data views, such as
Sales Orders (SO301000), view definitions may be located in separate files named views.ts.

You must use the import directive to refer to the view definitions from separate files, as shown
below.

import{
SOOrder,
BlanketTaxZoneOverrideFilter
} from './views';

The extensions folder contains TS and HTML files for extensions of the form. Each file name, such as
GL401000_MultiCurrency.ts, starts with the screen ID and ends with a postfix that indicates the extension’s
purpose.
**You can define the views and layout in extensions of the form for:**
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 23

• The areas of the form that are specific to particular features.
• The definitions of dialog boxes (which are also called smart panels in the Classic UI).
• The definitions of tabs. However, for tabs in extensions, you need to specify that the tab has an external
definition and provide its ID by using the ref attribute of the qp-tab tag. For details, see Tab:
Configuration.
• Any UI customization of the form. For details about UI customization, see UI Customization Development:
General Information.

Screen Class in TypeScript
To define the views of an Acumatica ERP form in TypeScript, in the TS file of the form, you define a screen class—a
class for the form—as shown in the following code.

import {
graphInfo,
PXScreen
} from "client-controls";

@graphInfo({
graphType:'PX.Objects.GL.AccountHistoryEnq',
primaryView:'Filter'
})
export class GL401000 extends PXScreen {
}

When you start typing the name of an API element in a TypeScript file in the FrontendSources
\screen folder in Visual Studio Code, the list of available elements is shown. You can hover over an
element to see its description.

**The screen class must satisfy these requirements:**
• It has the screen ID as the name of the class, such as GL401000.
• It extends the PXScreen class.
• It has the graphInfo decorator, in which you specify the graph and its primary view.

You can also specify optional parameters of the graphInfo decorator and use other
decorators.

In the Modern UI, each Acumatica ERP form must use its own graph type.

In the screen class, you define a property for each data view, as shown below.

import {
graphInfo, PXScreen, createSingle
} from "client-controls";

@graphInfo({
graphType:'PX.Objects.GL.AccountHistoryEnq',
primaryView:'Filter'
})
export class GL401000 extends PXScreen {
@viewInfo({containerName: 'Filter'})
Filter = createSingle(GLHistoryEnqFilter);
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 24

}

**This property must satisfy the following requirements:**
• It has the same name as the name of the data view. You’ll use this name to bind a UI control to the data view
in HTML.
• It has a viewInfo decorator with the specified container name. (This name is used as an object name
during the configuration of particular functionality, such as workflows and import and export scenarios. If
this value isn’t specified, the system displays the name of the data view as the object name.)
• If you need to display a form control, the property is initialized with the createSingle method, which
takes as the input parameter an instance of the view class (described below).
• If you need to display a table (grid) or a tree, the property is initialized with the createCollection
method, which takes as the input parameter an instance of the view class. You can specify configuration
parameters for the table by using the gridConfig decorator and for the tree by using the treeConfig
decorator.

The createCollection method can also be used when multiple records need to be
displayed and these records are rendered without a predefined Acumatica ERP control, such
as in the Outlook plug-in.

Multiple containers can be bound to the same graph's view. If you need to bind multiple container
controls to the same graph's view and one of these controls corresponds to a table or a tree, you
initialize the property in TypeScript by using the createCollection function.

View Classes in TypeScript
In the TS file of the form, you define a view class for each view of the graph, as shown below. The class extends the
PXView class.

You can use any name for the view class. However, we recommend that you use the name of the data
view’s primary DAC.
For the view classes added in a customization project, we recommend that you keep the prefix in the
name. The prefix consists of a two-letter identifier indicating the part of the functional area and a two-
letter prefix of the application area.

import {
PXView
} from "client-controls";

export class GLHistoryEnqFilter extends PXView {
}

In each view class, you specify the properties for all data fields of the data view that you want to be able to show or
use in the UI, as shown below. You use the name of the data field as the property name.

**Description: PXFieldState;**
**ShowCuryDetail: PXFieldState<PXFieldOptions.Hidden |**
PXFieldOptions.CommitChanges>;
**OrderDate: PXFieldState<PXFieldOptions.CommitChanges>;**
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 25

The fields that you’ve included in view classes are also used in various integration-related
functionality, such as in the Acumatica Mobile Framework, the screen-based SOAP API, and the copy-
paste functionality.

You specify the type of each property, which can be:
• PXFieldState.
• PXFieldState<list_of_options>, where you specify the options by using the PXFieldOptions
enum. The options can be combined.
You can also use decorators for fields. For information about decorators for fields, see Fieldset: Field Configuration.
For details about how to add a field from a joined data access class of the data view, see UI Definition in HTML and
**TypeScript: Joined Fields.**

Action Definitions in TypeScript
The actions defined in the graph or in the workflow have corresponding commands displayed on the More menu by
default. You do not need to define them in the TypeScript code of the Acumatica ERP form.
However, if you need to place a button for an action somewhere on the form other than the toolbar or the More
menu, you need to include the action’s definition in TypeScript. For details, see Button: Configuration.

When you include the property of the PXActionState type for the action in the TypeScript code
of a form, this action is automatically bound by name to an action in the graph. The action is not
displayed on the form toolbar by default. To specify explicitly whether the button for the action is
displayed on the form toolbar, you can use the PXButton.DisplayOnMainToolbar property in
the graph’s action declaration.

Layout in HTML
In the HTML file, you define the layout of an Acumatica ERP form, as shown in the following example. You must
place all HTML controls inside the template tag.

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

If you open the FrontendSources\screen folder in Visual Studio Code, when you start typing
the name of the tag or its attribute in an HTML file in a subfolder of this folder, the list of available tags
or attributes is shown. You can also hover over an HTML element to see its description.

In the HTML file, you use the rules described in the following resources:
• Part 3: Designing the Layout of an Acumatica ERP Form: The general approach you should follow
• UI Component Guide: Guidelines for particular UI elements
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 26

We recommend that you configure the appearance of UI controls by using TypeScript decorators
instead of properties of the config attribute in HTML. These decorators include fieldConfig,
controlConfig, gridConfig, and columnConfig. For details about the configuration of
particular controls, see UI Component Guide.

UI Components of an Acumatica ERP Form
The following diagram shows the UI components of an Acumatica ERP form and the interactions between them.

**Figure: UI components and their interactions**

#### Activity 2.1.1: To Create the UI of a Form

The following activity will walk you through the process of creating the UI of an Acumatica ERP form from scratch.

Story
The Repair Services (RS201000) form, which you will develop, will be used to view the list of services provided by
the Smart Fix company. By clicking buttons on the form toolbar, users will be able to add a new service, edit an
existing service, and delete a service. Below you can see what this form should look like.

**Figure: Service list on the Repair Services form**
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 27

You have already implemented the backend for the form, which includes the RSSVRepairServiceMaint graph
and the RSSVRepairService data access class (DAC). You have also already added the RSSVRepairService
table to the application database.

Process Overview
You will create TypeScript and HTML files for the Repair Services (RS201000) form. In the TypeScript file, you will
define the screen class and view class for the form. In the HTML file, you will define the layout of the form.

Step 1: Creating Files for the Form
To create the Modern UI for the Repair Services (RS201000) form, you need to create the form’s TypeScript and
**HTML files as follows:**
1. In Visual Studio Code or in the file system, open the FrontendSources\screen folder of your
Acumatica ERP instance. (In Visual Studio Code, you can open the folder by clicking File > Open Folder on
the toolbar.)
2. In the FrontendSources\screen\src folder, create the development folder (if it hasn't being
created yet), and within it, create the screens folder.
3. In the FrontendSources\screen\src\development\screens folder, create a folder with the RS
name if it hasn't been created yet. You will store the UI sources for all forms with the RS prefix in this folder.
4. In the FrontendSources\screen\src\development\screens\RS folder, create a folder with the
RS201000 name if it has not been created yet. You will store the UI sources for the Repair Services form in
this folder.
5. In the FrontendSources\screen\src\development\screens\RS\RS201000 folder, create the
following files:
• RS201000.ts
• RS201000.html

Step 2: Defining the Screen Class in TypeScript
To define the view of the Repair Services (RS201000) form in TypeScript, define a screen class and a property for the
data view of the form as follows:
1. In the RS201000.ts file, add the following import directives.

import {
PXScreen, graphInfo, createCollection,
} from "client-controls";

When you start typing the name of an API element in a TypeScript file in the
FrontendSources\screen folder in Visual Studio Code, the list of available elements is
shown. You can hover over an element to see its description.

2. Define the screen class for the form as follows. The class name is the ID of the form.

export class RS201000 extends PXScreen {}

3. For the screen class, add the graphInfo decorator, and specify the graph and the primary view of the form
in the decorator properties, as shown below. Hide the Note and Files buttons on the form title bar by using
the hideFilesIndicator and hideNotesIndicator properties. You don’t need notes and files for
the whole form because each record in the table on the form has its own notes and files.

@graphInfo({
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 28

graphType: "PhoneRepairShop.RSSVRepairServiceMaint",
primaryView: "RepairService",
hideFilesIndicator: true,
hideNotesIndicator: true,
})
export class RS201000 extends PXScreen {}

4. Define the property for the data view of the form by using the following code. Because the data view is used
to display a table, you need to initialize the property with the createCollection method. The input
parameter of this method is an instance of the view class, which you’ll define in the next step.

export class RS201000 extends PXScreen {
RepairService = createCollection(RSSVRepairService);
}

Step 3: Defining the View Class in TypeScript
You need to define a view class for the single data view of the Repair Services (RS201000) form. Proceed as follows:
1. In the RS201000.ts file, update the list of import directives, as shown below.

import {
PXScreen, graphInfo, createCollection,
PXView, PXFieldState,
gridConfig, PXFieldOptions, GridPreset
} from "client-controls";

2. Define the view class as follows.

export class RSSVRepairService extends PXView               {}

3. In the view class, specify the properties for all data fields of the data view, as shown below. You use the
name of the data field as the property name.

export class RSSVRepairService extends PXView {
**ServiceCD: PXFieldState;**
**Description: PXFieldState;**
**Active: PXFieldState;**
**WalkInService: PXFieldState<PXFieldOptions.CommitChanges>;**
**Prepayment: PXFieldState;**
**PreliminaryCheck: PXFieldState<PXFieldOptions.CommitChanges>;**
}

The WalkInService and PreliminaryCheck fields should commit changes to the server; that's why
you’ve used the PXFieldOptions.CommitChanges option for the property type.
4. Add the gridConfig decorator to the view class, as shown below. In the gridConfig decorator, you
must specify the preset property. Because the table is the primary element of the Repair Services form,
you use the Primary preset.

@gridConfig({
preset: GridPreset.Primary
})
export class RSSVRepairService extends PXView {
**ServiceCD: PXFieldState;**
**Description: PXFieldState;**
**Active: PXFieldState;**
**WalkInService: PXFieldState<PXFieldOptions.CommitChanges>;**
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 29

**Prepayment: PXFieldState;**
**PreliminaryCheck: PXFieldState<PXFieldOptions.CommitChanges>;**
}

Step 4: Defining the Layout in HTML
The Repair Services (RS201000) form contains only a table. So to define the layout of the form, you need to include
only the qp-grid element in the RS201000.html file, as the following code shows.

<template>
<qp-grid id="grid-RepairService" view.bind="RepairService"></qp-grid>
</template>

You’ve specified the ID of the qp-grid control and bound the control to the RepairServices property, which
you’ve defined in the RS201000.ts file.

If you open the FrontendSources\screen folder in Visual Studio Code, when you start typing
the name of the tag or its attribute in an HTML file in a subfolder of this folder, the list of available tags
or attributes is shown. You can also hover over an HTML element to see its description.

#### Activity 2.1.2: To Build the Source Code of a Particular Form for the Modern UI
Development

The following activity will walk you through the process of building the source code of a particular form that is
defined in the Modern UI.

Story
In the previous activity, you have defined the Repair Services (RS201000) form in the Modern UI for the Smart Fix
company. You need to build the source code of this form in the FrontendSources\screen folder of your
instance.

Process Overview
You will execute a command to start building the source code for the Repair Services (RS201000) form. You will then
review the UI of the form.

Step 1: Building the Source Code for a Particular Form
To build the sources for the Repair Services (RS201000) form, do the following in a command line tool (You can use
terminal in Visual Studio Code, or Windows PowerShell or a similar program.):
1. Switch to the FrontendSources\screen folder.
2. Run the following command in the FrontendSources\screen folder.

npm run build-dev --- --env customFolder=development screenIds=RS201000

You have specified the ID of the Repair Services form in the screenIds parameter to indicate that the
sources should be built for only the Repair Services form.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 30

Once the command finishes executing, you should see a message about successful compilation of a
webpack.

Step 2: Viewing the Form in the Modern UI
To test your changes, do the following:
1. Open the Repair Services (RS201000) form.

If the form is not opened from the workspace, try entering the RS201000 id in the page URL.

The form opens in the Modern UI.

In case the form opens in the Classic UI, click Tools > Switch to Modern UI on the form title
bar.

2. In the ScreenRepair row, clear the Walk-In Service check box, as shown on the following screenshot.

**Figure: Clearing the Walk-In Service check box**

3. Notice that the Requires Preliminary Check check box has been selected automatically.
4. Select the Walk-In Service check box in the ScreenRepair row.
5. Notice that the Requires Preliminary Check check box has been cleared automatically.
6. Save your changes.

Table (Grid): Configuration of the Table and Its Columns

In this topic, you can learn how to configure a table and its columns.

Table Configuration
To specify the configuration parameters of a grid, you use the gridConfig decorator in TypeScript. You put the
decorator on the definition of the view class for the table, as shown in the following example.

@gridConfig({
preset: GridPreset.Inquiry,
initNewRow: true,
quickFilterFields: ['AccountClassID', 'Type', 'PostOption', 'CuryID']
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 31

})
export class AccountRecords extends PXView {
}

For each table, you must specify a preset in the preset property of the gridConfig decorator. A preset is
a predefined set of properties of the gridConfig decorator that define how the table is displayed. For more
information about presets, see Form Layout: Grid Presets.

Table Columns
To specify the configuration parameters of a table column, you use the columnConfig decorator, as shown in the
following example.

export class SOLine extends PXView {
@columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
**ExcludedFromExport: PXFieldState;**
**IsConfigurable: PXFieldState;**
@columnConfig({ hideViewLink: true })
**BranchID: PXFieldState<PXFieldOptions.CommitChanges>;**
}

### Lesson 2.2: Converting an Acumatica ERP Form with the Converter

In this lesson, you will learn how to use the built-in converter to convert an Acumatica ERP form from the Classic UI
to the Modern UI.

**UI Definition in HTML and TypeScript: Form Converter**

You can use Acumatica's form converter, which transforms an Acumatica ERP form from the Classic UI to the
Modern UI. The converter parses an ASPX page of the form and generates the source files of the form in the Modern
UI. These source files include a TypeScript definition and initialization of views and an HTML layout of the form.

The use of the form converter doesn't guarantee a perfect conversion of a form to the Modern UI. The
converter provides a baseline conversion that you should further fine-tune based on your specific
requirements. We strongly recommend that you:
• Make necessary backups first and use the converter only in your development
environment.
• Always review the files generated by the converter and make any required corrections.
• Publish the converted form to your production environment only aer fully testing its
functionality in your development environment.

Running the Converter
To use the converter, you need to first make sure that the Modern UI is enabled for your instance. For details
on this, see Modern UI Development: General Information. You also need to check whether the following
key exists in the appSettings section of the Web.config file of your Acumatica ERP instance: <add
key="EnableSiteMapSwitchUI" value="False" />. If this key exists, remove it and save your changes
to the file.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 32

To run the converter, you open the needed Acumatica ERP form in the Classic UI and click Customization >
Convert to Modern UI on the form title bar.

The Convert to Modern UI command appears on the form only if the Customizer role is assigned to
your user account.

Aer you execute this command, the following files are generated:
• views.ts, which contains declarations of all views that are used in the form
• [SCREENID].ts, which contains the initialization of views for the form
• [SCREENID].html, which contains the HTML layout of the form
By default, the files are saved in a ZIP archive, which you can download.

• Before conversion, you need to turn on all Acumatica ERP features related to the form. This
will give you the converted version that’s the most similar to the original.
• The converter ignores any JavaScript code in ASPX files or the code in the ASPX.CS files.

Configuring the Converter
You can modify the behavior of the converter by adjusting the px.core\ui\screenConverter tag of the
web.config file of the instance, as shown in the following example.

<ui>
<screenConverter declareViewsInViewModelFile="false" />
</ui>

**You can use any of the following properties of the screenConverter tag:**
• declareViewsInViewModelFile: If you set this property to True, the converter will declare the views
in the <ScreenID>.ts file instead of creating a separate views.ts file.
• shouldFilesBeDownloaded: If you set this property to False, the files are saved in the
folder that’s defined by the screenConverterOutputFolder property. By default, the
shouldFilesBeDownloaded property is True and the files are saved in a ZIP archive.
• screenConverterOutputFolder: You can use this property to specify the output folder for
the generated files. Use this property only if the shouldFilesBeDownloaded property is False.
By default, the output folder is FrontendSources\screen\src\development\screens
\[FirstTwoLettersOfSCREENID]\[SCREENID].
• usingOfPXJoinSyntaxEnabled: If you set this property to True, the system uses the approach for
joined fields, which involves the use of periods. For details on the approaches to define joined fields, see UI
**Definition in HTML and TypeScript: Joined Fields. By default, this property is True.**
• isAutoFormatEnabled: If you set this property to True, the system uses the Prettier tool to
automatically format the HTML and TypeScript code generated by the converter.

#### Activity 2.2.1: To Convert an Acumatica ERP Form to the Modern UI with the
Converter

The following activity will walk you through the conversion of an Acumatica ERP form from the Classic UI to the
Modern UI. You will convert the form by using the form converter.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 33

Story
Suppose that you’ve developed the Serviced Devices (RS202000) form for the Smart Fix company. The form has
been developed for a previous version of Acumatica ERP and is displayed in the Classic UI. You need to convert the
form to the Modern UI and want to use the form converter.

Process Overview
You will modify the converter settings to fit your needs and convert the form to the Modern UI. You will then review
the contents of the TypeScript and HTML files and adjust them. You will build the Modern UI sources for the form
and review the resulting form.

Step 1: Adjusting the Converter Settings
The form converter comes with default settings. You need to adjust them to have the following results aer
conversion:
• The views are declared in the RS202000.ts file instead of in a separate views.ts file.
• The files of the Serviced Devices (RS202000) form are saved in the FrontendSources\screen\src
\development\screens\RS\RS202000 folder of the instance instead of a ZIP file.
To configure the converter, add the shouldFilesBeDownloaded and declareViewsInViewModelFile
attributes in the px.core\ui\screenConverter tag of the web.config file of the instance, as shown in the
following code.

<ui>
<screenConverter usingOfPXJoinSyntaxEnabled="true"
shouldFilesBeDownloaded="false"
declareViewsInViewModelFile="true"/>
</ui>

The usingOfPXJoinSyntaxEnabled attribute is specified in the web.config file by default.

Step 2: Generating the Source Files with the Converter
Now you can generate the source files of the form by using the converter. To generate the files, do the following:
1. Open the Serviced Devices (RS202000) form.
2. On the Customization menu, click Convert to Modern UI.
The system generates the files, saves them in the FrontendSources\screen\src\development
\screens\RS\RS202000 folder of the instance, and displays a notification that the conversion has
completed.
3. Close the notification by clicking OK.

Step 3: Adjusting the Generated TypeScript File
The generated TypeScript file may contain unnecessary import directives. To clean up the code, do the following:
1. Review the RS202000.ts file.
The TypeScript code contains the RS202000 screen class, which extends the PXScreen class and includes
a property for the data view of the form. The code also contains the RSSVDevice view class, which extends
the PXView class.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 34

2. Adjust the file as follows:
a. Remove unnecessary import directives.
b. Fix any formatting issues.
c. Adjust the name in the viewInfo decorator, which specifies the container name. (This name is used as
an object name during the configuration of the particular functionality, such as workflows and import
and export scenarios.)
The resulting file looks as follows.

import {
createSingle, PXScreen, graphInfo, viewInfo, PXView, PXFieldState
} from "client-controls";

@graphInfo({
graphType: "PhoneRepairShop.RSSVDeviceMaint",
primaryView: "ServDevices",
})
export class RS202000 extends PXScreen {
@viewInfo({containerName: "Service Devices"})
ServDevices = createSingle(RSSVDevice);
}

// View
export class RSSVDevice extends PXView            {
**DeviceCD : PXFieldState;**
**Description : PXFieldState;**
**Active : PXFieldState;**
**AvgComplexityOfRepair : PXFieldState;**
}

Step 4: Adjusting the Generated HTML File
The generated HTML file may contain unnecessary code elements and inaccurate IDs. To adjust the file, do the
following:
1. Review the RS202000.html file.
The HTML code includes one qp-template element with the 1-1 name, which organizes the elements on
the form into two columns of equal width. Each column is defined with the qp-fieldset element, which
is marked with the slot attribute to identify the column of the template to which the fieldset belongs. Each
fieldset includes the field elements for the form’s UI elements.
2. Adjust the file as follows:
a. Move fields from the fieldset with slot="B" to the end of the fieldset with slot="A", and remove the
fieldset with slot="B". Since the form has only four fields, it’s better to organize them in one column.
b. Change the IDs so that they match the guidelines for IDs. You can use the following IDs:
• For the qp-template tag: form-ServDevices
• For the qp-fieldset tag: fsColumnA

You can find the guidelines for IDs of particular UI components in UI Component Guide.

c. Remove the wg-container attribute because you don’t have tests for the Classic UI of the Serviced
Devices (RS202000) form, which can be reused for the Modern UI. (For details about the tests, see Part 8:
Testing the Modern UI.)
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 35

d. Fix any formatting issues.
The resulting HTML code looks as follows.

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

Step 5: Building the Source Code and Viewing the Form
Now you can build the source files and view how the converted form looks in the Modern UI. Do the following:
1. Build the source code of the Serviced Devices (RS202000) form. For details on how to do it, see Activity 2.1.2:
To Build the Source Code of a Particular Form for the Modern UI Development.
If your files are located in the development folder, you can use the following command.

npm run build-dev --- --env customFolder=development screenIds=RS202000

2. While you are on the Classic UI of the Serviced Devices (RS202000) form, click Tools > Switch to Modern UI
on the form title bar. The Modern UI for the form is displayed.
3. In the Device Code box, click the selector icon.
The lookup table opens, as shown in the following screenshot.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 36

**Figure: The lookup table**

4. In the lookup table, select the MotorRAZR device.
The rest of the elements on the form are filled in with the MotorRAZR device properties, as shown in the
following screenshot.

**Figure: The device properties**

5. Clear the Active check box.
6. On the form toolbar, click Save.

**Fieldset: Field Configuration**

You can define properties of a field in a fieldset by using the fieldConfig decorator or by using the
controlConfig decorator, which is a shortcut for the fieldConfig decorator.
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 37

The controlConfig decorator accepts configuration of the associated control as a parameter. If you do not need
to specify the type of the control (in the controlType property), you can use the controlConfig decorator
instead of the fieldConfig decorator.

Example
Suppose that you have the following code in HTML.

<field name="FormatLocale"
control-type="qp-selector" config.bind="{ displayMode: 'text',
suggester: { descriptionName:'CultureReadableName' } }">
</field>

We recommend that you rewrite the code above by using the fieldConfig decorator as follows.

@fieldConfig({
controlType: "qp-selector",
controlConfig: {
displayMode: 'text',
suggester: { descriptionName: 'CultureReadableName' },
}
})
**FormatLocale: PXFieldState;**

If you do not need to change the default type of the control, you can make the TypeScript code shorter by using the
controlConfig decorator, as shown in the following example.

@controlConfig({
displayMode: 'text',
suggester: { descriptionName: 'CultureReadableName' },
})
**FormatLocale: PXFieldState;**

If you have defined the control properties in TypeScript by using one of the decorators as shown above, the code of
the field in HTML looks as shown in the following code.
<field name="FormatLocale"></field>

**UI Definition in HTML and TypeScript: Joined Fields**

To add a field from a joined data access class (DAC) of the data view to the UI of an Acumatica ERP form, you can
use one of the approaches described in this topic.

Using Two Underscores
In this approach, you use two underscores to separate the name of the joined DAC and the field name in this DAC.
The following example shows the declaration of a joined field in TypeScript.

Customer__AcctName: PXFieldState;

The following HTML code uses this joined field.

<field name="Customer__AcctName"></field>
## Part 2: Defining Acumatica ERP Forms in HTML and TypeScript | 38

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
**AcctName: PXFieldState;**
**ClassID: PXFieldState;**
}

3. In the view class that corresponds to the data view with the joined DAC, declare a property for the joined
DAC. The name of the property is the name of the joined DAC, and the type of the property is the class that
you’ve just declared. The following example shows an example of the property for the joined DAC.

export class DiscountCustomer extends PXView {
...
**Customer : Customer;**
}

export class RS209500 extends PXScreen {
Discount = createSingle(DiscountCustomer);
}

As a result of adding the joined fields in this way, you can use them in HTML. You can specify the full name of the
field, which includes the following parts separated by periods: the view class name, the name of the joined DAC,
and the field name. Alternatively, if the fieldset that contains the field has the data view specified, you can use a
shorter name that omits the view class name. The following code shows both of these approaches.

<qp-fieldset id="columnOne" view.bind="Discount">
<field name="DiscountCustomer.Customer.AcctName"></field>
<field name=".Customer.AcctName"></field>
</qp-fieldset>
## Part 3: Designing the Layout of an Acumatica ERP Form | 39

## Part 3: Designing the Layout of an Acumatica ERP Form
This part provides instructions on organizing the layout of an Acumatica ERP form.

### Lesson 3.1: Understanding Approaches to Organize Layout

In this lesson, you will learn about the following:
• Approaches to organize the layout of an Acumatica ERP form and a table (also referred to as a grid) by using
diﬀerent tools
• The specification of predefined templates for diﬀerent parts of the form
• The use of presets to configure the appearance of grids
• The ways you can use the predefined CSS classes

**Form Layout: General Information**

When developing an Acumatica ERP form or any of its parts, you should design the layout of the form according
to the standard practices. This chapter provides recommendations and descriptions of tools that you can use to
design the layout quickly and in a unified way.

Applicable Scenarios
**You can use the information from this chapter in the following scenarios:**
• Migrating an existing form from the Classic UI to the Modern UI
• Developing a new form by using the Modern UI
• Developing a new part of an existing form, such as a grid or a tab with elements

Approaches to Organize Layout
**The Modern UI provides the following approaches to quickly organize layout in a unified way:**
• A summary area or a tab area of an Acumatica ERP form can use one of the predefined layouts. Predefined
layouts are defined by templates that you specify for an area of a form. See details in Form Layout:
Predefined Templates.
• For grids, you can specify a predefined set of properties by using presets, which are described in detail in
**Form Layout: Grid Presets.**
• You can use CSS classes to adjust color settings and organize layout inside templates. They can also be used
for grids and for the organization of multiple UI controls inside a field area. For a description of CSS classes,
see Form Layout: CSS Classes.

**Form Layout: Predefined Templates**

You can use predefined templates to control the layout of areas of an Acumatica ERP form. Templates
automatically adjust based on screen width and resolution.
## Part 3: Designing the Layout of an Acumatica ERP Form | 40

Overview of Templates
You specify a template by using the qp-template tag with the following required attributes:
• id: Identifies a template instance and can be used to reference the template in customizations and
extensions
• name: Specifies which template to use
Each template organizes UI elements into named slots. The name of a template has the following structure:
<Slot1>-...-<SlotN>, where:
• N is the number of slots in the template.
• Each slot number represents its relative width.
For example, the 7-10-7 template has three slots:
• The first slot has a width of 7/24 of the form’s width.
• The second slot is 10/24 of the form’s width.
• The third slot has a width of 7/24 of the form’s width; 24 is the sum of the relative width of all slots: 7 + 10
+ 7 = 24.
To use qp-template, you need to distribute the UI controls among the available slots by using the slot
attribute. In each slot, controls are rendered vertically. The slots are referred to by names, such as A, B, and C. You
can apply the slot attribute to any element, such as qp-fieldset, qp-grid, or div. If a template contains
multiple slots, you can distribute controls between only some of them. For example, if a template contains three
slots, you may use only two of them; any unused slot remains empty.

Template Usage Example
The following example uses the 7-10-7 template.

<qp-template id="formDocument" name="7-10-7" wg-container>
<qp-fieldset id="fsColumnA" slot="A" view.bind="Document">
<field name="OrderType"></field>
<field name="OrderNbr"></field>
<field name="Status"></field>
<field name="OrderDate"></field>
<field name="RequestDate"></field>
<field name="CustomerOrderNbr"></field>
<field name="CustomerRefNbr"></field>
</qp-fieldset>
<qp-fieldset id="fsColumnB" slot="B" view.bind="Document">
<field name="CustomerID" config-allow-edit.bind="true"></field>
<field name="CustomerLocationID" config-allow-edit.bind="true"></field>
<field name="ContactID" config-allow-edit.bind="true"></field>
<field name="CuryID" control-type="qp-currency" view.bind="CurrencyInfo"></field>
<field name="DestinationSiteID"></field>
<field name="ProjectID" config-allow-edit.bind="true"></field>
<field name="OrderDesc" config-type.bind="1" config-rows.bind="3"></field>
</qp-fieldset>
<qp-fieldset id="fsColumnC-summary" slot="C" view.bind="Document">
<field name="OrderQty"></field>
<field name="CuryDiscTot"></field>
<field name="CuryVatExemptTotal"></field>
<field name="CuryVatTaxableTotal"></field>
<field name="CuryTaxTotal"></field>
<field name="CuryOrderTotal"></field>
## Part 3: Designing the Layout of an Acumatica ERP Form | 41

<field name="CuryControlTotal"></field>
</qp-fieldset>
</qp-template>

Template Selection Guide
We recommend choosing the template and label size based on the form type, as specified in the following table.

Form Type                               Template                                Label Size

Data entry forms for transactions       Three-slot templates for the Sum-       Default
(records representing the exchange      mary area; you can select a tem-
or movement of money, goods, or         plate by using the recommenda-
services)                               tions below.

Data entry forms for profiles           1-1                                     M
(records representing a person,
company, or entity)

Processing forms                        17-17-14 or 17-14-17                    Default

Inquiry forms                           17-17-14 or 17-14-17                    Default

Setup forms                             1-1                                     XM

Maintenance forms                       1-1                                     XM

Use the following general recommendations when selecting a template.
**If the Summary area or a tab of a form should include three slots of elements:**
• Use 7-10-7 if you need narrower le and right slots and a wider center slot.
• Use 17-17-14 for equal le and center slots but a narrower right slot.
• Use 1-1-1 for three slots with similar widths.
**If the Summary area of a form should include two slots of elements:**
• Use 1-1 for slots with equal width.
• Use 17-7 or 2-1 if you want to put a grid in the first slot and a fieldset in the second or if 1-1 is not wide
enough due to longer elements in the first slot.
• Use 7-17 or 1-2 if you want to put a fieldset in the first slot and a grid in the second or if 1-1 isn’t wide
enough due to very long elements in the second slot.

Available Templates
See the following table to learn more about the available templates.
## Part 3: Designing the Layout of an Acumatica ERP Form | 42

Template   Description

17-17-14   Three slots—the third with short elements.

17-14-17   Shows three slots; the second has shorter elements than the first and third do.

14-17-17   Shows three slots; the first has shorter elements than the second and third do.

17-31      Shows two slots; the first has shorter elements than the second does.
## Part 3: Designing the Layout of an Acumatica ERP Form | 43

Template   Description

7-10-7     Shows three slots; the second includes long elements.

10-7-7     Shows three slots; the first includes long elements.

17-7       Shows two slots; the first includes long elements.

7-17       Shows two slots; the second includes long elements.
## Part 3: Designing the Layout of an Acumatica ERP Form | 44

Template   Description

1-1-1      Shows three slots with similar lengths of elements.

2-1        Shows two slots; the first includes long elements.

1-2        Shows two slots; the second includes long elements.

1-1        Shows two slots with similar lengths of elements.
## Part 3: Designing the Layout of an Acumatica ERP Form | 45

Template        Description

1               Shows one slot with long elements.

Comparison of Templates
The following diagram compares the widths of the slots in templates.
## Part 3: Designing the Layout of an Acumatica ERP Form | 46

**Form Layout: Grid Presets**

To configure the appearance of a table (which is also called grid) on an Acumatica ERP form, you can specify
a preset for the grid control. A preset is a predefined set of properties, such as mergeToolbarWidth or
syncPosition, that define the appearance of the grid.
Presets are an analog of the SkinID property in ASPX in the Classic UI. However, not all values of the SkinID
property have analogs in the Modern UI. You need to find the appropriate preset in the list of available values.
You should use presets because they unify the appearance of grids in the UI and simplify the process of updating
the design.
To specify the preset, you use the preset property of the gridConfig decorator, as shown in the following
example.

Without Preset                                            With Preset

@gridConfig({
@gridConfig({                                                 preset: GridPreset.ReadOnly
adjustPageSize: true,                                 })
mergeToolbarWith: 'ScreenToolbar',                    export class GLHistoryEnquiryResult
syncPosition: true,                                       extends PXView
preserveSortsAndFilters: true,
allowDelete: false,
allowInsert: false,
allowImport: false,
allowSkipTabs: false,
actionsConfig: {
insert: {hidden: true},
delete: {hidden: true}}
})
export class GLHistoryEnquiryResult
extends PXView

Available Presets
The following table lists the values of the preset property and the designs they provide.

If some of the properties of the preset do not fit your needs, you can override them in the
gridConfig decorator.
## Part 3: Designing the Layout of an Acumatica ERP Form | 47

Value          Description

Primary        An editable table, which includes the following com-
ponents:
• Table toolbar: Merged with the form toolbar
• Filtering and search: Available and saved in the ses-
sion
• Table footer: Displayed
**Guidelines:**
• Use this preset for primary lists with an editable ta-
ble if there is no entry form for the records, such as
Chart of Accounts (GL202500).
• Do not specify a grid caption.
**Respective SkinID in ASPX: Primary**

Inquiry        A read-only table, which includes the following com-
ponents:
• Table toolbar: Merged with the form toolbar
• Filtering and search: Available and saved in the ses-
sion
• Table footer: Displayed
• The insert and delete operations inside the table:
Forbidden
**Guidelines:**
• Use this preset for primary lists with read-only
grids, such as Sales Orders (SO3010PL), Invoic-
es (SO3030PL), and Import Bank Transactions
(CA3065PL).
• Use the preset for inquiry forms.
• Do not specify a table caption.
**Respective SkinID in ASPX: PrimaryInquiry**

Processing     A read-only table, which includes the following com-
ponents:
• Table toolbar: Merged with the form toolbar
• Filtering and search: The grid filters are shown on
demand (showFilterBar: GridFilter-
BarVisibility.OnDemand)
• Table footer: Displayed
• The insert and delete operations inside the table:
Forbidden
**Guidelines:**
• Use for processing forms.
This preset does not have an analogue in ASPX.
## Part 3: Designing the Layout of an Acumatica ERP Form | 48

Value        Description

ReadOnly     A read-only table, which includes the following com-
ponents:
• Table toolbar: Displayed separately from the form
toolbar
• Filtering and search: Only the Search box is avail-
able
• Table footer: Displayed
• The insert and delete operations inside the table:
Forbidden
**Guidelines:**
• Use this preset for read-only tables on data entry
forms, such as the table on the Shipments tab on
Sales Orders (SO301000) form.
• Do not specify a grid caption.
**Respective SkinID in ASPX: Inquiry**

Details      An editable table, which includes the following com-
ponents:
• Table toolbar: Displayed separately from the form
toolbar
• Filtering and search: Only the Search box is avail-
able
• Table footer: Displayed
• The insert and delete operations inside the table:
Allowed
**Guidelines:**
• Use this preset for editable tables on data entry
forms.
• Do not specify a table caption.
**Respective SkinID in ASPX: Details**
## Part 3: Designing the Layout of an Acumatica ERP Form | 49

Value                                                       Description

Attributes                                                  A partially editable table with a predefined set of rows,
but the cells can be editable. The table includes the
following components:
• Table toolbar: Not displayed
• Filtering and search: Unavailable
• Table footer: Not displayed
• The insert and delete operations inside the grid:
Forbidden
**Guidelines:**
• Use this preset for smaller tables surrounded by
other fieldsets, such as the Attributes table on the
Attributes tab of the Stock Items (IN202500) form.
• Specify a table caption.
**Respective SkinID in ASPX: Attributes**

ShortList                                                   An editable table, which includes the following com-
ponents:
• Table toolbar: Displayed separately from the form
toolbar
• Filtering and search: Unavailable
• Table footer: Not displayed
• The insert and delete operations inside the grid: Al-
lowed
**Guidelines:**
• Use this preset for smaller tables, such as the Sales
Categories table on the Attributes tab of the Stock
Items form.
• Do not specify a table caption.
**Respective SkinID in ASPX: ShortList**

Empty                                                       An empty preset that includes no settings.
**Guideline: Use this preset if the preset property be-**
comes mandatory and you need to use a custom set of
properties.

**You can find the default values of settings in each preset in the file attached to the Form Layout: Grid Presets topic**
on help.acumatica.com.

**Form Layout: CSS Classes**

You can use the predefined CSS classes, which are listed below, to adjust the color settings and layout of templates.
These classes are defined in the FrontendSources/screen/static/basic-layout.css file of the
instance folder.
## Part 3: Designing the Layout of an Acumatica ERP Form | 50

adaptive-height
You can use the adaptive-height CSS class for qp-text-box in multiline mode or for its parent control. The
class causes the parent control to adapt its height to the size of the qp-text-box control.
The following example uses this class.

<qp-panel
id="LogFileFilterRecord" caption="Log" auto-repaint="true" width="50vw">
<qp-fieldset id="frmLockout" view.bind="LogFileFilterRecord">
<field
name="Text"
class="label-size-xxs adaptive-height"
config-type.bind="1"></field>
</qp-fieldset>
<footer>
<qp-button id="btnCancel" dialog-result="Cancel" caption="Close"></qp-button>
</footer>
</qp-panel>

align-end
The align-end CSS class causes an element to be placed on the right side of the container, such as a form.
For example, if you need a set of buttons to be placed along the right border of a form that is displayed in a pop-up
panel, you need to put the buttons in a div tag and specify the align-end class for this div tag. Such a form is
shown in the following screenshot.

**Figure: The Bank Transaction Rules form**

The following example implements this screenshot and uses the align-end class.

<template>
<qp-template> ... </qp-template>
## Part 3: Designing the Layout of an Acumatica ERP Form | 51

<div class="h-stack align-end">
<qp-button id="buttonApply" state.bind="SaveClose"></qp-button>
<qp-button id="buttonApplyAll" state.bind="SaveAndApply"></qp-button>
</div>
</template>

In a dialog box (the qp-panel control), you do not need to specify the align-end class for the
buttons located in the footer tag. The alignment along the right side is implemented for the footer
by default.

col-auto
The col-auto CSS class causes an element to have its caption fully visible and does not include excessive space
in any screen size, as shown for the View On Map button in the following screenshots for a narrow screen and a
wide screen. That is, the width of the button is fixed for any screen size.

The following example uses this class.

<field name="AddressButtons">
<qp-button id="btnViewOnMap" state.bind="ViewOnMap" class="col-auto">
</qp-button>
</field>

The class can be applied to any element (however, it is designed to be used with buttons).

col-N
The col-N CSS class, where N is a number from 1 to 12, specifies the width of a control in columns. N defines the
number of columns that the control occupies relative to the width of its parent control. (That is, the width of the
parent control is considered to be 12 columns.)
You can use these classes to organize multiple UI controls inside a field area (which are implemented with Merge
in PXLayoutRule in ASPX). For examples of the organization of multiple UI controls in a field area, see Form
**Layout: An Element Next to Another Element.**

Avoid using controls that span multiple columns of controls (which are implemented with
ColumnSpan in PXLayoutRule in ASPX). You should use multiline text boxes instead. For details,
see Text Box: Multiline Text Box.

The following example uses this class.
<field name="ShipVia">
## Part 3: Designing the Layout of an Acumatica ERP Form | 52

<qp-button id="btnShopRates" state.bind="ShopRates" class="col-7">
</qp-button>
</field>

The class can be applied to any element.

col-lg-X, col-md-X, and col-sm-X
The col-lg-X, col-md-X, and col-sm-X CSS classes, where X is a number from 1 to 12, specify the width of a
control in columns for diﬀerent types of screens:
• For large screens: col-lg-X
• For medium screens: col-md-X
• For small screens: col-sm-X
X indicates the width in columns, where 1 is the smallest width and 12 is the full width of the parent control.
The following example uses these classes.
<div class="v-stack col-sm-12 col-md-6 col-lg-4"></div>

The class can be applied to any element.

control-size-<SIZE>
The control-size-XXX classes limit the maximum width of a control to the specified size.
<SIZE> can have the following values:
• xxs: 40 px
• xs: 70 px
• s: 100 px
• sm: 140 px
• m: 200 px
• xm: 250 px
• l: 300 px
• xl: 350 px
• xxl: 400 px
The following examples use these classes.
<field class="control-size-m"...>
<qp-field class="control-size-l"...>
<qp-fieldset class="control-size-xxl"...>

The class can be applied to any element.

**We do not recommend that you use these classes extensively: Because the Modern UI forms are**
adaptive to the width of the screen, the use of these classes may lead to diﬀerent widths of controls in
a single column.

default-control
The default-control class indicates that the control should have focus when the form opens.

<field name="CustomerID" config-allow-edit.bind="true"
## Part 3: Designing the Layout of an Acumatica ERP Form | 53

class="default-control"></field>

In ASPX, you would use the DefaultControlID attribute of the PXFormView tag to indicate the
default control that would have focus.

equal-height
The equal-height class indicates that the columns in the template should be aligned in height.
The class can be applied to qp-template.

framed-section
The framed-section CSS class displays a container in a separate gray frame, as shown in the following
screenshot.

The following example uses this class.
<qp-grid id="gridSalesPerTran" view.bind="SalesPerTran"
class="framed-section"></qp-grid>

The class can be applied to any container.

full-width
The full-width CSS class stretches the right side of the template to the right side of the screen, ignoring the
maximum size of the form, which is 1600 px.
We recommend that you use this CSS class if you have a wide grid inside the template.
The following example uses this class.
<qp-template name="7-17" id="comissions-form" class="full-width">
...
</qp-template>

The class can be applied to qp-template.

h-stack
The h-stack CSS class defines the list of elements rendered horizontally.
The following example uses this class.

<div class="h-stack">
<div class="h-stack" >
<qp-fieldset id="first" hide-caption="true">
## Part 3: Designing the Layout of an Acumatica ERP Form | 54

...
</qp-fieldset>
<qp-fieldset id="second" hide-caption="false">
...
</qp-fieldset>
</div>
<qp-fieldset id="summary" hide-caption="false" caption="Summary" >
...
</qp-fieldset>
</div>

The class can be applied to any container.

highlights-section
The highlights-section CSS class defines the style for the pane with a blue background, as shown in the
following screenshot.

The following example uses this class.

<qp-fieldset id="totals"
hide-caption="false"
class="highlights-section">
...
</qp-fieldset>

The class can be applied to qp-fieldset.

indent-1, indent-2, and indent-3
The indent-1, indent-2, and indent-3 CSS classes define the number of indentations for the control. The
number in the class name corresponds to the number of indentations.
## Part 3: Designing the Layout of an Acumatica ERP Form | 55

The following example uses this class.
<field name="CopyLineNotesToInvoice"></field>
<field name="CopyLineNotesToInvoiceOnlyNS" class="indent-1"></field>

The class can be applied to the field tag.

label-size-<SIZE>
The label-size-<SIZE> CSS class specifies the width of the labels in the container.

We recommend that you not overuse this set of CSS classes. Ideally, all labels on a form should have
the same size.

<SIZE> can have the following values:
• xxs: 40 px
• xs: 70 px
• s: 100 px
• sm: 140 px
• m: 200 px
• xm: 250 px
• l: 300 px
• xl: 350 px
• xxl: 400 px
The class can be applied to any element.

label-size-zero
The label-size-zero class specifies that the element and its nested elements do not have labels but should
have the asterisk (*) displayed to indicate that the value is required.
In the following code, two boxes (First Name and Last Name) are displayed in a single line. The Last Name box is
required, as shown in the following screenshot.

<qp-field control-state.bind="PrimaryContactCurrent.FirstName"
config-placeholder.bind="'First Name'">
<qp-label slot="label" caption="Name"></qp-label>
<qp-field control-state.bind="PrimaryContactCurrent.LastName"
class="label-size-zero" config-placeholder.bind="'Last Name'"
## Part 3: Designing the Layout of an Acumatica ERP Form | 56

config-required="PrimaryContactCurrent.LastName.required">
</qp-field>
</qp-field>

no-label
The no-label CSS class specifies that the element and all its nested elements do not have labels. However, you
can override this behavior in any nested element by specifying class="label-size-<SIZE>" for the nested
element.

You do not need to use class="no-label" with qp-field. qp-field does not have a label by
default.

If you need to hide the label part of the control but display the asterisk (*) symbol, use the label-size-zero
class.
In the following code, the One field has a label of size S, the Two field has no label, the Three field has no label,
and the Four field has a label of size M.

<qp-template name="1" id="mf" class="no-label">
<qp-fieldset id="fs1" view.bind="MyView" class="label-size-s">
<field name="One"></field>
<field name="Two" class="no-label"></field>
</qp-fieldset>
<qp-fieldset id="fs2" view.bind="MyView">
<field name="Three"></field>
<field name="Four" class="label-size-m"></field>
</qp-fieldset>
</qp-template>

The class can be applied to any element.

no-stretch
The no-stretch CSS class prevents the element from being stretched over the height of the whole Acumatica
ERP form or the area of a form, such as a tab.

By default, the grid is stretched over the height of the whole area.

The following example uses this class.
<div class="v-stack">
<div id="Filter_form" wg-container>
...
</div>
<qp-grid id="grid" view.bind="EnqResult"
class="no-stretch">
</qp-grid>
</div>

The class can be applied to any container.
## Part 3: Designing the Layout of an Acumatica ERP Form | 57

stretch
The stretch CSS class stretches the element over the height of the container to which the element belongs. For
example, the element can be stretched to the height of the whole Acumatica ERP form or the area of a form, such as
a tab.
The class is applied by default to qp-grid and qp-tabbar.
The following example uses this class.
<qp-rich-text-editor id="edDescription" class="stretch"
state.bind="Case.Description">
</qp-rich-text-editor>

The class can be applied to any element.

transparent-section
The transparent-section CSS class defines the style of the pane with the transparent background.
We recommend that you use transparency only for fieldsets with a maximum of two rows of controls.
The following screenshot shows an element in a transparent fieldset.

The following screenshot shows a selected check box in a transparent fieldset.

The following example uses this class.
<qp-template name="17-17-14" id="byBookFilterForm">
<qp-fieldset slot="A" id="deprBookFilterForm" view.bind="deprbookfilter"
class="transparent-section">
<field name="BookID"></field>
</qp-fieldset>
</qp-template>
## Part 3: Designing the Layout of an Acumatica ERP Form | 58

The class can be applied to qp-fieldset .

v-stack
The v-stack CSS class defines the list of elements rendered vertically.
The elements are rendered vertically by default; therefore, in most cases, there is no need to use this class.
The following example uses this class.

<div class="v-stack">
<div id="Filter_form" wg-container>
...
</div>
<qp-grid id="grid" view.bind="EnqResult">
</qp-grid>
</div>

The class can be applied to any container.

**Form Layout: Box Connotations**

You can highlight a value in the textbox control with a color connotation. The color can change depending on the
value in the box.
In Acumatica ERP data entry forms with workflows, the Status box has default connotations. You can add new
connotations or change existing ones. The following screenshot shows an example of the color connotation for the
Open status.

**Figure: The highlighted status**

Changing the Default Color or Defining a New One
You can specify a connotation for any box that has the PXStringListAttribute attribute on the
corresponding DAC field. Some forms, such as data entry forms with workflows, have default connotations defined
in the style.scss file of the screen folder or in the common folder of the module.
**You can change the default connotation or create a new one by doing the following:**
1. In the screen folder, open the style.scss file or create a new one.
If you’re creating a new file, in the style.scss file, import the mixin from the WebSites/Pure/Site/
FrontendSources/screen/static/status-colors.scss file.
## Part 3: Designing the Layout of an Acumatica ERP Form | 59

2. Define the correspondence between the PXStringList attribute values and the colors, as shown in the
following example.

@import 'static/status-colors.scss';
@include colored-status('N', var(--status-blue-color));
@include colored-status('H', var(--status-orange-color));
@include colored-status('P', var(--status-red-color));
@include colored-status('V', var(--status-red-color));
@include colored-status('E', var(--status-orange-color));
@include colored-status('A', var(--status-orange-color));
@include colored-status('R', var(--status-red-color));
@include colored-status('C', var(--status-gray-color));
@include colored-status('L', var(--status-gray-color));
@include colored-status('B', var(--status-orange-color));
@include colored-status('S', var(--status-blue-color));
@include colored-status('I', var(--status-blue-color));
@include colored-status('D', var(--status-orange-color));

Specifying the Connotation for a Box
To specify connotations that have been defined in the style.scss file, do the following:
1. In the HTML template of the screen, import the styles.scss file that defines the connotations.

<template>
<require from="./style.scss"></require>
...
</template>

2. For the field that you want to highlight, specify class="colored" in the corresponding field tag.

<qp-fieldset id="fsColumnA-Order" slot="A" view.bind="Document">
...
<field name="Status" class="colored" pinned="true"></field>
</qp-fieldset>

**Related Links**
• Combo Box: Configuration
• Workflow Creation: To Add States
• Defining Workflow States

### Lesson 3.2: Converting a Setup Form

In this lesson, you will learn how to do the following to define the setup form:
• Organize the layout of a setup form
• Configure a setup form with only a Summary area
• Configure a setup form with tabs
## Part 3: Designing the Layout of an Acumatica ERP Form | 60

**UI of a Setup Form: General Information**

On a setup form, an administrator provides configuration or maintenance settings for some functionality in the
instance. In the Acumatica ERP workspaces, setup forms are typically listed under Preferences. This topic provides
recommendations and guidelines on organizing the layout of a setup form.
The following screenshot shows an example of a setup form with tabs.

**Figure: A setup form with tabs**

Applicable Scenarios
**You configure the setup form in the following cases:**
• You are migrating an existing setup form to the Modern UI.
• You are creating a new setup form by using the Modern UI.

Template and Label Sizes
The recommended template for a setup form is 1-1. For details about the available templates, see Form Layout:
Predefined Templates.

The recommended size for labels on setup forms is xm.

Recommendations for Organizing the Layout
The following table shows recommendations for organizing the layout of a setup form.
## Part 3: Designing the Layout of an Acumatica ERP Form | 61

Correct                                                   Incorrect

When most labels are long, make all labels long (with class="label-size-<SIZE>" in qp-template).
They should be as long as is needed to make most labels visible without a user needing to hover over the label to
see the full name.

UX and Functional Design Guidelines
The form design should be tailored for screens with a resolution of 1280 x 720.
The number of the setup form should start with 10. For details, see Form and Report Numbering.
**Related Links**
• Creating Setup Forms
• Form Types
• Form and Report Numbering

**UI of a Setup Form: A Form with Only a Summary Area**

The following topic describes how to configure a form that contains only a Summary area. (In the Classic UI, these
forms were based on the FormView template.) The following screenshot shows an example of a form with this
layout.
## Part 3: Designing the Layout of an Acumatica ERP Form | 62

**Figure: A form with only a Summary area**

View Definition in TypeScript
To configure a form with only a Summary area in the TypeScript file, you do the following:
• For the view of the Summary area, you use the createSingle method.
• You use a class that extends PXView with the full list of the fields of the Summary area.
The following code shows an example of the TypeScript configuration for the form in the screenshot above.

import {
PXScreen, createSingle, graphInfo, PXView,
PXFieldState, PXFieldOptions, controlConfig
} from 'client-controls';

export class FASetup extends PXView {
**FAAccrualAcctID: PXFieldState<PXFieldOptions.CommitChanges>;**
**FAAccrualSubID: PXFieldState<PXFieldOptions.CommitChanges>;**
**ProceedsAcctID: PXFieldState<PXFieldOptions.CommitChanges>;**
**ProceedsSubID: PXFieldState<PXFieldOptions.CommitChanges>;**
**DeprHistoryView: PXFieldState;**
**DepreciateInDisposalPeriod: PXFieldState;**
**AccurateDepreciation: PXFieldState;**
**ReconcileBeforeDisposal: PXFieldState;**
**AllowEditPredefinedDeprMethod: PXFieldState;**

@controlConfig({allowEdit: true, })
**RegisterNumberingID: PXFieldState;**

@controlConfig({allowEdit: true, })
**AssetNumberingID: PXFieldState;**

@controlConfig({allowEdit: true, })
**BatchNumberingID: PXFieldState;**
## Part 3: Designing the Layout of an Acumatica ERP Form | 63

@controlConfig({allowEdit: true, })
**TagNumberingID: PXFieldState;**
**CopyTagFromAssetID: PXFieldState;**
**AutoReleaseAsset: PXFieldState;**
**AutoReleaseDepr: PXFieldState;**
**AutoReleaseDisp: PXFieldState;**
**AutoReleaseTransfer: PXFieldState;**
**AutoReleaseReversal: PXFieldState;**
**AutoReleaseSplit: PXFieldState;**
**UpdateGL: PXFieldState;**
**AutoPost: PXFieldState;**
**SummPost: PXFieldState;**
**SummPostDepreciation: PXFieldState;**
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
## Part 3: Designing the Layout of an Acumatica ERP Form | 64

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

**UI of a Setup Form: A Form with Tabs**

The following topic describes how to configure a form that consists of multiple tabs. (In the Classic UI, these forms
were based on the TabView template.) The following screenshot shows an example of a form with this layout.

**Figure: A form with multiple tabs**
## Part 3: Designing the Layout of an Acumatica ERP Form | 65

View Definition in TypeScript
To configure a setup form with multiple tabs in the TypeScript file, you do the following:
• For the views that display one record, you use the createSingle method.
• For the views that display a grid, you use the createCollection method.
• For each view for a tab, you use a class that extends PXView with the full list of fields to be displayed.
• For each grid and its columns, you use the gridConfig and columnConfig decorators. For details, see
Table (Grid): Configuration of the Table and Its Columns.
The following code shows an example of the TypeScript configuration for the form in the screenshot above.

import {
PXScreen,
PXView,
PXFieldState,
PXFieldOptions,
createSingle,
graphInfo,
viewInfo,
columnConfig,
} from "client-controls";

@graphInfo({
graphType: "PX.Objects.SO.SOSetupMaint",
primaryView: "sosetup",
})
export class SO101000 extends PXScreen {
@viewInfo({ containerName: "SO Preferences" })
sosetup = createSingle(sosetup);
}

export class sosetup extends PXView {
**DefaultOrderType: PXFieldState;**
**TransferOrderType: PXFieldState;**
**ShipmentNumberingID: PXFieldState;**

@columnConfig({ allowNull: false })
**PickingWorksheetNumberingID: PXFieldState;**

**AdvancedAvailCheck: PXFieldState;**

**MinGrossProfitValidation: PXFieldState;**
**UsePriceAdjustmentMultiplier: PXFieldState;**
**IgnoreMinGrossProfitCustomerPrice: PXFieldState;**
**IgnoreMinGrossProfitCustomerPriceClass: PXFieldState;**
**IgnoreMinGrossProfitPromotionalPrice: PXFieldState;**

**FreightAllocation: PXFieldState;**

**FreeItemShipping: PXFieldState;**
**HoldShipments: PXFieldState;**
**RequireShipmentTotal: PXFieldState;**
**AddAllToShipment: PXFieldState<PXFieldOptions.CommitChanges>;**
**CreateZeroShipments: PXFieldState<PXFieldOptions.CommitChanges>;**
## Part 3: Designing the Layout of an Acumatica ERP Form | 66

**CreditCheckError: PXFieldState;**
**UseShipDateForInvoiceDate: PXFieldState;**

**AutoReleaseIN: PXFieldState;**

**SalesProfitabilityForNSKits: PXFieldState;**

**DfltIntercompanyOrderType: PXFieldState;**
**OrderType: PXFieldState;**
**DfltIntercompanyRMAType: PXFieldState;**
**DisableAddingItemsForIntercompany: PXFieldState;**
**DisableEditingPricesDiscountsForIntercompany:**
PXFieldState<PXFieldOptions.CommitChanges>;

**ShowOnlyAvailableRelatedItems: PXFieldState;**
**DefaultReturnOrderType: PXFieldState;**

**OrderRequestApproval: PXFieldState<PXFieldOptions.Disabled |**
PXFieldOptions.Hidden>;
}

Layout in HTML
**You define the layout of a setup form with tabs by adding the following tags to the HTML code of the form:**
• A qp-tabbar tag with a nested qp-tab element for each tab.
• For each tab that does not contain a grid, a nested qp-template tag. For details on using templates, see
**Form Layout: Predefined Templates.**
The following code shows the HTML template for the form in the screenshot above.

<template>
<qp-tabbar id="tabbar">
<qp-tab id="tab-General" caption="General" class="label-size-xm">
<qp-template id="form-General" name="1-1" wg-container="sosetup_tab">
<div id="divColumnA-General" slot="A">
<qp-fieldset id="fsDataEntry-General"
view.bind="sosetup" caption="Data Entry Settings">
<field name="DefaultOrderType"></field>
<field name="TransferOrderType"></field>
<field name="ShipmentNumberingID"></field>
<field name="PickingWorksheetNumberingID"></field>
<field name="AdvancedAvailCheck"></field>
</qp-fieldset>
<qp-fieldset id="fsPrice-General" view.bind="sosetup"
caption="Price Settings">
<field name="MinGrossProfitValidation"></field>
<field name="UsePriceAdjustmentMultiplier"></field>
<field name="IgnoreMinGrossProfitCustomerPrice">
<qp-label slot="label"
caption="Ignore Min. Markup Validation for Prices">
</qp-label>
</field>
<field name="IgnoreMinGrossProfitCustomerPriceClass"></field>
<field name="IgnoreMinGrossProfitPromotionalPrice"></field>
</qp-fieldset>
<qp-fieldset id="fsShipment-General" view.bind="sosetup"
## Part 3: Designing the Layout of an Acumatica ERP Form | 67

caption="Shipment Settings">
<field name="FreeItemShipping"></field>
<field name="HoldShipments"></field>
<field name="RequireShipmentTotal"></field>
<field name="AddAllToShipment"></field>
<field name="CreateZeroShipments"></field>
</qp-fieldset>
<qp-fieldset id="fsInvoice-General" view.bind="sosetup"
caption="Invoice Settings">
<field name="CreditCheckError"></field>
<field name="UseShipDateForInvoiceDate"></field>
</qp-fieldset>
</div>
<div id="divColumnB-General" slot="B">
<qp-fieldset id="fsPosting-General" view.bind="sosetup"
caption="Posting Settings">
<field name="AutoReleaseIN"></field>
</qp-fieldset>
<qp-fieldset id="fsFreightCalculation-General"
view.bind="sosetup"
caption="Freight Calculation Settings">
<field name="FreightAllocation"></field>
</qp-fieldset>
<qp-fieldset id="fsSalesProfitability-General"
view.bind="sosetup"
caption="Sales Profitability Settings">
<field name="SalesProfitabilityForNSKits"></field>
</qp-fieldset>
<qp-fieldset id="fsIntercompanyOrder-General"
view.bind="sosetup"
caption="Intercompany Order Settings">
<field name="DfltIntercompanyOrderType"></field>
<field name="DfltIntercompanyRMAType"></field>
<field name="DisableAddingItemsForIntercompany"></field>
<field name="DisableEditingPricesDiscountsForIntercompany"></field>
</qp-fieldset>
<qp-fieldset id="fsRelatedItem-General"
view.bind="sosetup"
caption="Related Item Settings">
<field name="ShowOnlyAvailableRelatedItems"></field>
</qp-fieldset>
<qp-fieldset id="fsRelatedCase-General"
view.bind="sosetup" caption="Related Case Settings">
<field name="DefaultReturnOrderType"></field>
</qp-fieldset>
</div>
</qp-template>
</qp-tab>
</qp-tabbar>
</template>

#### Activity 3.2.1: To Create the UI of a Setup Form

The following activity will walk you through the process of creating the UI of an Acumatica ERP setup form.
## Part 3: Designing the Layout of an Acumatica ERP Form | 68

Story
The Smart Fix company uses the Repair Work Orders (RS301000) data entry form to create and manage repair work
orders. The Repair Work Order Preferences (RS101000) setup form, for which you will develop the Modern UI, will
be used by an administrative user to specify the company's preferences for the repair work orders. The following
screenshot shows what this form should look like.

**Figure: The Repair Work Order Preferences form**

You have already implemented the backend for the form, which includes the RSSVSetup data access class (DAC)
and the RSSVSetupMaint graph. Also, you have added the RSSVSetup table to the application database.

Process Overview
You will create TypeScript and HTML files for the Repair Work Order Preferences (RS101000) form. In the TypeScript
file, you will define the screen class and view class for the form. In the HTML file, you will define the layout of the
form.

Step 1: Creating Files for the Setup Form
To create the Modern UI for the Repair Work Order Preferences (RS101000) setup form, you need to create
TypeScript and HTML files for the form. Create the files as follows:
1. In the FrontendSources\screen\src\development\screens\RS folder, create a folder with the
RS101000 name. You will store the UI sources for the Repair Work Order Preferences form in this folder.
2. In the FrontendSources\screen\src\development\screens\RS\RS101000 folder, create the
following files:
• RS101000.ts
• RS101000.html

Step 2: Defining the Screen Class in TypeScript
To define the view of the Repair Work Order Preferences (RS101000) setup form in TypeScript, you define a screen
class and a property for the data view of the form. Do the following:
1. In the RS101000.ts file, add the import directives as follows.

import {
PXScreen,
createSingle,
graphInfo,
} from "client-controls";

2. Define the screen class for the form as follows. The class name is the ID of the form.
## Part 3: Designing the Layout of an Acumatica ERP Form | 69

export class RS101000 extends PXScreen {}

3. For the screen class, add the graphInfo decorator, and specify the graph and the primary view of the form
in the decorator properties, as shown in the following code.

@graphInfo({
graphType: "PhoneRepairShop.RSSVSetupMaint",
primaryView: "Setup",
})
export class RS101000 extends PXScreen {}

4. Define the property for the data view of the form by using the following code. Because the data view is used
to display only a Summary area, you need to initialize the property with the createSingle method. The
input parameter of this method is an instance of the view class that you will define in the next step.

export class RS101000 extends PXScreen {
Setup = createSingle(RSSVSetup);
}

Step 3: Defining the View Class in TypeScript
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
**NumberingID: PXFieldState;**

@controlConfig({allowEdit: true, })
**WalkInCustomerID: PXFieldState;**

**DefaultEmployee: PXFieldState;**
**PrepaymentPercent: PXFieldState;**
}

You have used the controlConfig decorator to display the values in the Numbering Sequence and
Walk-In Customer boxes as links to the records whose identifiers are displayed in the selector control.
## Part 3: Designing the Layout of an Acumatica ERP Form | 70

4. Save your changes.

Step 4: Defining the Layout in HTML
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
**You have used the following recommended settings:**
• The 1-1 template for the form
• The label-size-xm class for the qp-template element

Step 5: Building and Viewing the Setup Form
To build the source files for the Repair Work Order Preferences (RS101000) setup form and view its Modern UI
version, do the following:
1. Run the following command in the FrontendSources\screen folder of your instance.

npm run build-dev --- --env customFolder=development screenIds=RS101000

2. Aer the source files have been built successfully, launch your Acumatica ERP instance, and open the Repair
Work Order Preferences setup form.
3. On the form title bar, click Tools > Switch to Modern UI. The Modern UI version of the Repair Work Order
Preferences setup form is displayed. The form should look similar to the form shown in the screenshot in the
Story section of this activity.
4. Click the link in the Numbering Sequence box and make sure the Numbering Sequences (CS201010) form
opens with the RSSVWORDER record displayed.

**Selector Control: Configuration of a Link**

A selector control can display a value as a link to the record whose identifier is displayed in the selector control. The
link is configured diﬀerently depending on the location of the selector control.
## Part 3: Designing the Layout of an Acumatica ERP Form | 71

Selector Control in a Fieldset
In a fieldset, you add the link by specifying allowEdit: true in the controlConfig decorator for the field in
TypeScript, as shown in the following example.
@controlConfig({allowEdit: true, })
**CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;**

@controlConfig({allowEdit: true, })
**CustomerLocationID: PXFieldState;**

@controlConfig({allowEdit: true, })
**ContactID: PXFieldState;**

The code above displays links, as shown below.

**Figure: Links in the selector controls**

**The allowEdit: true setting also adds the + (Add Row) button to the lookup table of the selector**
control.

Selector Control in a Table
In a table, a link is displayed by default for a selector control. To remove the link, specify hideViewLink: true
in the columnConfig decorator in TypeScript, as shown in the following example.
@columnConfig({ hideViewLink: true })
**BranchID: PXFieldState<PXFieldOptions.CommitChanges>;**

The code above removes the link from the Branch column, as shown below.

**Figure: Link in the grid**

Link Behavior
You can configure which action is executed when a user clicks the link in the selector control. By default, the system
opens the record that is selected by the user in the selector control. To specify a custom action, you use one of the
following:
• For a selector control located in a fieldset, the editCommand property in the controlConfig decorator,
as shown in the following code
export class EPAssignmentMap extends PXView {
@controlConfig({editCommand: "OpenForm"})
## Part 3: Designing the Layout of an Acumatica ERP Form | 72

**GraphType: PXFieldState<PXFieldOptions.CommitChanges>;**
}

• For a selector control located in a grid, the linkCommand decorator
@gridConfig({
preset: GridPreset.Inquiry
})
export class RSSVWorkOrderToPay extends PXView {
@linkCommand<RS401000>("ViewOrder")
**OrderNbr: PXFieldState;**
}

To use a custom action for the link in the selector control, you also need to do the following:
1. In the graph, define an action that opens the entered form with the new record.
2. In the TypeScript file of the form, declare a property of the PXActionState type for this action in the
screen class.

### Lesson 3.3: Converting a Data Entry Form

In this lesson, you will learn how to do the following when you define a data entry form:
• Organize the layout of the data entry form
• Configure the data entry form in HTML and TypeScript

**Data Entry Form: General Information**

A data entry form is used for the input of records of a particular type. This topic provides recommendations and
guidelines on organizing the layout of a data entry form.
The following screenshot shows an example of a data entry form. As is true of most data entry forms, the displayed
form has a Summary area and a tab area with multiple tabs. The displayed tab shows a table (generally referred to
as a grid).
## Part 3: Designing the Layout of an Acumatica ERP Form | 73

**Figure: A data entry form**

Applicable Scenarios
**You configure a data entry form in the following cases:**
• You are migrating an existing data entry form to the Modern UI.
• You are creating a new data entry form by using the Modern UI.

Templates and Label Sizes
Data entry forms that display transactional data, such as Invoices and Memos (AR301000), should use three-column
templates for the Summary area. You can select a particular template by the general recommendations below. You
use the default label size.
Data entry forms that display profile data, such as Customers (AR303000), should use the 1-1 template. Label size
should be M.
When selecting a particular template, you can also use the general recommendations that are described in Form
**Layout: Predefined Templates.**

Recommendations for Organizing the Layout
The following table shows recommendations for organizing the layout of a data entry form.

Correct                                                    Incorrect

If you need to show total numbers, put them in the blue fieldset (class="highlights-section") on the
right side of the screen.
Do not put the blue fieldset between other fieldsets.
## Part 3: Designing the Layout of an Acumatica ERP Form | 74

Correct                                                     Incorrect

If you have multiple fieldsets with short labels and short values in fields, put them in multiple columns and
stacks by using one of the following templates: 1-1-1, 7-10-7, 17-17-14, or 1-1.
Do not put fields with short labels and short values in fields in the 1 template.

Make labels with long text longer. Try to make the labels in all fieldsets similar in length (specify class="la-
bel-size-<SIZE>" in qp-template).
## Part 3: Designing the Layout of an Acumatica ERP Form | 75

Correct                                                     Incorrect

Use a caption instead of showing a single tab.
For grids, add the caption as described in Table with a Title.
For fieldsets in a qp-template, use the qp-caption control.

Try to occupy slots of the template equally in order to balance the screen.

Use the multiline Description field because the width of a single slot on the data entry form may not fit the
field properly. For details, see Text Box: Multiline Text Box.

Show full-width grids without a gray background.
## Part 3: Designing the Layout of an Acumatica ERP Form | 76

Correct                                                  Incorrect

UX and Functional Guidelines
The form design should be tailored for screens with a resolution of 1280 x 720.
The number of the data entry form should start with 30. For details, see Form and Report Numbering.

**Data Entry Form: Definition in TypeScript and HTML**

The following topic describes how to configure a data entry form depending on its layout.

View Definition in TypeScript
For a form with a Summary area and multiple tabs, you use the following in the TypeScript file of the form:
• For the summary view and the views that display one record, the createSingle method.
• For each property for the view that displays a grid, the createCollection method.
• For the summary view and each view for a tab, a class that extends PXView with the full list of fields to be
displayed.
• The headerDescription decorator for fields in the Summary area whose value should be included in
the record title below the form name.
• The gridConfig and columnConfig decorators for each grid and its columns. For details, see Table
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
## Part 3: Designing the Layout of an Acumatica ERP Form | 77

})
export class SO301000 extends PXScreen {
//Actions that are used in qp-button tags in HTML
**AddInvoiceOK: PXActionState;**
**OverrideBlanketTaxZone: PXActionState;**
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
**OrderType: PXFieldState;**
**OrderNbr: PXFieldState;**
**Status: PXFieldState<PXFieldOptions.Disabled>;**
**DontApprove: PXFieldState<PXFieldOptions.Disabled>;**
**Approved: PXFieldState<PXFieldOptions.Disabled>;**
**OrderDate: PXFieldState<PXFieldOptions.CommitChanges>;**
**RequestDate: PXFieldState<PXFieldOptions.CommitChanges>;**
**CustomerOrderNbr: PXFieldState<PXFieldOptions.CommitChanges>;**
**CustomerRefNbr: PXFieldState;**
**CuryInfoID: PXFieldState;**

@headerDescription
**CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;**
**CustomerLocationID: PXFieldState<PXFieldOptions.CommitChanges>;**
**ContactID: PXFieldState<PXFieldOptions.CommitChanges>;**
**CuryID: PXFieldState<PXFieldOptions.CommitChanges>;**
**DestinationSiteID: PXFieldState<PXFieldOptions.CommitChanges>;**
**ProjectID: PXFieldState<PXFieldOptions.CommitChanges>;**
**OrderDesc: PXFieldState;**

**OrderQty: PXFieldState<PXFieldOptions.Disabled>;**
**CuryDiscTot: PXFieldState<PXFieldOptions.CommitChanges>;**
**CuryVatExemptTotal: PXFieldState<PXFieldOptions.Disabled>;**
**CuryVatTaxableTotal: PXFieldState<PXFieldOptions.Disabled>;**
**CuryTaxTotal: PXFieldState<PXFieldOptions.Disabled>;**
**CuryOrderTotal: PXFieldState<PXFieldOptions.Disabled>;**
**CuryControlTotal: PXFieldState<PXFieldOptions.CommitChanges>;**
**ArePaymentsApplicable: PXFieldState<PXFieldOptions.CommitChanges>;**
**IsRUTROTDeductible: PXFieldState<PXFieldOptions.CommitChanges>;**
**IsFSIntegrated: PXFieldState<PXFieldOptions.Disabled>;**

**ShowDiscountsTab: PXFieldState;**
**ShowShipmentsTab: PXFieldState;**
**ShowOrdersTab: PXFieldState;**
}

export class SOOrder extends PXView {
**BranchID: PXFieldState<PXFieldOptions.CommitChanges>;**
**BranchBaseCuryID: PXFieldState;**
**DisableAutomaticTaxCalculation: PXFieldState<PXFieldOptions.CommitChanges>;**
## Part 3: Designing the Layout of an Acumatica ERP Form | 78

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
**AddInvBySite: PXActionState;**
**ShowMatrixPanel: PXActionState;**
...

//Table columns
**Availability: PXFieldState<PXFieldOptions.Hidden>;**

@columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
**ExcludedFromExport: PXFieldState;**
**IsConfigurable: PXFieldState;**

@columnConfig({ hideViewLink: true })
**BranchID: PXFieldState<PXFieldOptions.CommitChanges>;**
...
}

Layout in HTML
**You define the layout of a data entry form by adding the following tags to the HTML code of the form:**
• A qp-template tag. For details on using templates, see Form Layout: Predefined Templates.
• A qp-tabbar tag with a nested qp-tab element for each tab.
• For each tab that does not contain a grid, a nested qp-template tag.
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
## Part 3: Designing the Layout of an Acumatica ERP Form | 79

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

#### Activity 3.3.1: To Create the UI of a Data Entry Form

The following activity will walk you through the process of developing the UI of a data entry form.

Story
Suppose that you need to develop the Repair Work Orders (RS301000) form in the Modern UI. The form will have a
Summary area and two tabs below the Summary area.
The following screenshot shows what the form should look like.
## Part 3: Designing the Layout of an Acumatica ERP Form | 80

**Figure: The Repair Work Orders form**

You have already implemented the backend for the form, which includes the RSSVWorkOrderEntry graph and
the RSSVWorkOrder, RSSVWorkOrderItem, and RSSVWorkOrderLabor data access classes (DACs). You
have also already added the corresponding tables to the application database.

Process Overview
You will create TypeScript and HTML files for the Repair Work Orders (RS301000) form. In the TypeScript file, you
will define the screen class and view classes for the form. You will also define the following views:
• WorkOrders, which is the primary view of the form and is bound to the Summary area of the form
• RepairItems, which is bound to the table on the Repair Items tab of the form
• Labor, which is bound to the table on the Labor tab of the form
In the HTML file, you will define the layout of the form.

Step 1: Creating Files for the Form
To implement the Modern UI version of the Repair Work Orders (RS301000) form, you need to create the TypeScript
and HTML files for the form. Create the files as follows:
1. In the FrontendSources\screen\src\development\screens\RS folder, create a folder with the
RS301000 name if it has not been created yet.
2. In the FrontendSources\screen\src\development\screens\RS\RS301000 folder, create the
following files:
• RS301000.ts
• RS301000.html

Step 2: Defining the Screen Class in TypeScript
To define the view of the Repair Work Orders (RS301000) form in TypeScript, you define a screen class and a
property for the data views of the form. Do the following:
1. In the RS301000.ts file, add the import directives as follows.

import {
PXScreen, createCollection, graphInfo,
viewInfo, createSingle,
## Part 3: Designing the Layout of an Acumatica ERP Form | 81

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

In the viewInfo decorator, you have specified the names of the containers. These names are used as
object names during the configuration of particular functionality, such as workflows and import and export
scenarios. If this value is not specified, the system displays the name of the data view as the object name.

Step 3: Defining the Primary View Class in TypeScript
You need to define a view class for the primary data view of the Repair Work Orders (RS30100) form, which is
WorkOrders.
**Proceed as follows:**
1. In the RS301000.ts file, update the list of import directives, as the following code shows.

import {
PXScreen, createCollection, graphInfo,
viewInfo, createSingle,
PXView, PXFieldOptions, PXFieldState, controlConfig,
} from "client-controls";

2. Define the RSSVRepairWorkOrder view class as follows.
## Part 3: Designing the Layout of an Acumatica ERP Form | 82

export class RSSVWorkOrder extends PXView {}

3. In the view class, specify the properties for all data fields of the data view that should be displayed in the UI,
as shown below. You use the name of the data field as the property name.

export class RSSVWorkOrder extends PXView {
**OrderNbr: PXFieldState;**

@controlConfig({allowEdit: true, })
**CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;**
**DateCreated: PXFieldState;**
**DateCompleted: PXFieldState;**
**Status: PXFieldState;**

@controlConfig({rows: 2})
**Description : PXFieldState<PXFieldOptions.Multiline>;**

@controlConfig({allowEdit: true, })
**ServiceID : PXFieldState<PXFieldOptions.CommitChanges>;**

@controlConfig({allowEdit: true, })
**DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;**
**OrderTotal: PXFieldState;**
**Assignee: PXFieldState;**
**Priority: PXFieldState<PXFieldOptions.CommitChanges>;**
**InvoiceNbr: PXFieldState;**
}

For the CustomerID, ServiceID, DeviceID, and Priority fields, changes should be committed to
the server; therefore, you have used the PXFieldOptions.CommitChanges option for the property
type.
You have defined the links in the selector controls for the CustomerID, ServiceID, and DeviceID
fields by specifying allowEdit: true in the controlConfig decorator.
You have used the controlConfig decorator with the specified rows property and the Description
field with the PXFieldOptions.Multiline option to define a multiline text box with two text lines. For
details, see Text Box: Multiline Text Box.

Step 4: Defining the View Classes for Tables in TypeScript
In the TypeScript file of the form, you need to define view classes for the views that are bound to tables on
the Repair Items and Labor tabs of the Repair Work Orders (RS301000) form: RSSVWorkOrderItem and
RSSVWorkOrderLabor. Proceed as follows:
1. In the RS301000.ts file, add gridConfig and GridPreset to the list of import directives.
2. Define the RSSVWorkOrderItem view class as follows.

export class RSSVWorkOrderItem extends PXView {
**RepairItemType: PXFieldState;**
**InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;**
InventoryID_description: PXFieldState;
**BasePrice: PXFieldState;**
}

For the InventoryID field, changes should be committed to the server; therefore, you have used the
PXFieldOptions.CommitChanges option for the property type.
## Part 3: Designing the Layout of an Acumatica ERP Form | 83

3. Add the gridConfig decorator to the RSSVWorkOrderItem view class, as the following code shows.
In the gridConfig decorator, you must specify the preset property. Because the table is used on one of
the tabs of the data entry form, you use the Details preset. For information about presets, see Form Layout:
Grid Presets.

@gridConfig({
preset: GridPreset.Details
})
export class RSSVWorkOrderItem extends PXView {
**RepairItemType: PXFieldState;**
**InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;**
InventoryID_description: PXFieldState;
**BasePrice: PXFieldState;**
}

4. Define the RSSVWorkOrderLabor view class similarly to the way you defined the
RSSVWorkOrderItem view class. The resulting class should be defined as follows.

@gridConfig({
preset: GridPreset.Details
})
export class RSSVWorkOrderLabor extends PXView {
**InventoryID: PXFieldState;**
InventoryID_description: PXFieldState;
**DefaultPrice: PXFieldState;**
**Quantity: PXFieldState<PXFieldOptions.CommitChanges>;**
**ExtPrice: PXFieldState;**
}

5. Save your changes.

Step 5: Defining the Layout in HTML
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
## Part 3: Designing the Layout of an Acumatica ERP Form | 84

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
## Part 3: Designing the Layout of an Acumatica ERP Form | 85

5. Save your changes.

Step 6: Building and Viewing the Form
To build the source files for the Repair Work Orders (RS301000) form and view its Modern UI version, do the
following:
1. Run the following command in the FrontendSources\screen folder of your instance.

npm run build-dev --- --env customFolder=development screenIds=RS301000

2. Aer the source files have been built successfully, launch your Acumatica ERP instance, and open the Repair
Work Orders form for the 000001 work order.
3. On the form title bar, click Tools > Switch to Modern UI. The Modern UI version of the Repair Work Orders
form is displayed. The form should look similar to the form shown in the screenshot in the Story section of
this activity.

If you see multiple actions of the form toolbar and no More menu aer you switch to the
Modern UI, open the Apply Updates (SM203510) form, and in the More menu, click Reset
Caches.

4. Click the link in the Customer ID box and make sure the Customers (AR303000) form opens with the
C000000001 record displayed. Close the Customers form.
5. On the Repair Work Orders form, make sure the Description box is multiline.
6. On the form toolbar, click Remove Hold. Notice that the status of the repair work order has changed to
Ready for Assignment.

**Collapsible Area: Configuration**

You can configure an area on an Acumatica ERP form to be collapsible. You can define either of the following as a
collapsible area:
• A section that corresponds to a container
• A part of the form that corresponds to a template
When a part of the form is collapsed, only the pinned elements are displayed.

Configuring a Collapsible Area
To make an area of an Acumatica ERP form collapsible, you add the qp-collapsible attribute to the tag that
defines the area, as shown in the following code example.

<qp-template name="7-10-7" id="document_form"
qp-collapsible class="equal-height">
</qp-template>

If the form has any collapsible areas, the arrow button is displayed at the top right corner of the form, as shown in
the following screenshot.
## Part 3: Designing the Layout of an Acumatica ERP Form | 86

**Figure: A Summary area with collapsible sections**

When a user clicks the button, all containers with the qp-collapsible attribute are collapsed; only the pinned
elements are displayed.
Sections with no pinned elements in the Summary or Selection area are displayed but empty (that is, they contain
no elements), and their height is adjusted to the height of the other sections, as shown in the following screenshot.
If the section has a title, the title is displayed.

**Figure: The Summary area with the collapsed sections**

If the section is displayed on a tab and it has no pinned elements, only the title of the section is displayed without
the space normally used by its elements, as shown with the sections on the right side of the form in the following
screenshot.

**Figure: Collapsible sections on a tab**
## Part 3: Designing the Layout of an Acumatica ERP Form | 87

Defining Pinned Elements
By default, all required elements—that is, the elements that correspond to the fields with the
[PXUIFields(Required = true)] attribute in the DAC—are pinned. A user can specify which elements are
pinned in the Section Configuration dialog box, as shown in the following screenshot.

**Figure: The Section Configuration**

You can configure other elements to be pinned by default. To pin an element that corresponds to a field from code,
you use the pinned attribute of the field tag, as shown in the following example.

<field name="OrderQty" pinned></field>
<field name="CuryDetailExtPriceTotal" pinned="true"></field>

To unpin an element that corresponds to a field, you specify pinned="false", as shown in the following code.

<field name="OrderDate" pinned="false"></field>

**Text Box: Multiline Text Box**

**You can configure a multiline text box by using one of the following approaches:**
• Using code in TypeScript (recommended)
• Using code in HTML

You should use multiline text boxes in the Modern UI instead of text boxes that span multiple columns
in the Classic UI.

An example of a multiline text box is shown in the following screenshot.
## Part 3: Designing the Layout of an Acumatica ERP Form | 88

**Figure: The Description multiline text box**

Configuring a Multiline Text Box with TypeScript (Recommended)
To configure a multiline text box with TypeScript, you use the controlConfig decorator with the
specified rows property, which specifies the number of lines in the control, and define the field with the
PXFieldOptions.Multiline option, as the following example shows.

@controlConfig({rows: 2})
**DocDesc: PXFieldState<PXFieldOptions.Multiline>;**

In the HTML code, the field is defined as shown in the following code.
<field name="DocDesc"></field>

Configuring a Multiline Text Box with HTML
To configure a multiline text box only in an HTML template, you specify the following attributes in the field
declaration:
• config-type.bind="1", which indicates that the control has multiple lines.
• config-rows.bind="N", which specifies the number of lines in the control (where N is the number of
lines).
An example of a multiline box is shown in the following code.

<field name="OrderDesc" config-type.bind="1" config-rows.bind="2"></field>

Adding a New Line in the Multiline Text Box
By default, when a user clicks Enter in the multiline text box, the focus is shied to the next control. To add a new
line, the user should click Ctrl+Enter.
You can give users the ability to create a new line by pressing Enter. To do it, assign true to the
ITextEditorControlConfig.enterKeyAddsNewLine property, as shown in the following code.

@fieldConfig({
controlType: "qp-text-editor",
controlConfig: {
type: 1,
rows: 13,
enterKeyAddsNewLine: true
}
## Part 3: Designing the Layout of an Acumatica ERP Form | 89

})
**Content: PXFieldState;**

**Related Links**
• Interface ITextEditorControlConfig

**Record Title: Configuration**

For data entry and maintenance forms, the record title includes the values of key fields by default. If you need to
adjust the automatically defined record title, you can use one of the following approaches:
• Use the headerDescription decorator (recommended)
• Implement the ICaptionable interface in the graph of the form

Using the headerDescription Decorator
To define a record title, in the view class that corresponds to the primary view of the graph, you need to add the
headerDescription decorator to every field whose value should be specified in the record title. For details on
defining view classes, see View Classes in TypeScript.
The decorator is applicable to any field in the primary view.
For example, the record title for the Shipments (SO302000) form is composed of the following values: Shipment
Nbr. (the ShipmentNbr field, which is the key field of the form) and Customer (the CustomerID field). To define
the record title, the headerDescription decorator is added to the CustomerID field in the definition of the
view class, as shown in the following code.

export class SOShipmentHeader extends PXView {
**ShipmentNbr: PXFieldState;**

@headerDescription
**CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;**

...
}

You do not need to add the headerDescription decorator to the key field (ShipmentNbr in the
example above) because the key fields are added to the record title by default.

Depending on which fields are marked with the headerDescription decorator, the record title is composed
according to the following rules:
• If no field is marked with the headerDescription decorator, the record title is received from the server.
If the server does not send the record title or sends information that it should be empty, the record title is
composed of the set of key fields of the view and the description of the last key field if this description exists.
• If only the non-key fields are marked with the headerDescription decorator, the record
title is composed of the key fields of the view and the description of the fields marked with the
headerDescription decorator.
• If only the key fields are marked with the headerDescription decorator, the record title is composed
of these key fields and the description of the last key field, regardless of whether it is marked with the
headerDescription decorator.
• If both the key fields and the non-key fields are marked with the headerDescription decorator, the
record title is composed based on these fields.
## Part 3: Designing the Layout of an Acumatica ERP Form | 90

You can specify how a field is shown in the record title by specifying the parameter of the headerDescription
decorator on this field. For details, see Enumeration HeaderDescription.

### Lesson 3.4: Converting a Processing Form

In this lesson, you will learn how to do the following while defining a processing form:
• Organize the layout of the processing form
• Configure the processing form in HTML and TypeScript

**Processing Form: General Information**

On a processing form, a user can perform an operation on multiple selected records at once.

Applicable Scenarios
You implement a processing form if you need to provide the ability for the user to invoke an operation on multiple
records at once.

Processing Forms
Processing forms look similar to inquiry forms. A processing form usually has the following components:
• A table (which is also referred to as a grid) that displays the list of records retrieved by the processing data
view. The table includes:
• A column with an unlabeled check box, which gives the user the ability to select one record or multiple
records in the grid for processing.
• Additional columns that contain key settings of each listed record, including its ID or number.
• Optional: A redirection button or link that can be clicked to open the data entry form for any selected
record.
• Optional: Table filters (also known as quick filters) that are displayed above the table.
• A form toolbar that includes the Process, Process All, and Cancel buttons.
• Optional: An area that provides selection criteria (for narrowing the records that are listed and may be
processed) or configuration settings—or both—for the processing method.
The screenshot below shows an example of a processing form with a Selection area and a grid.
## Part 3: Designing the Layout of an Acumatica ERP Form | 91

**Figure: A processing form**

Naming Conventions for Processing Forms
Processing forms have IDs that start with a two-letter abbreviation (indicating the functional area of the form)
followed by 50 (indicating a processing form), such as RS501000.
The names of the graphs that work with processing forms have the Process suﬀix. For instance,
RSSVAssignProcess will be the name of the graph for the Assign Work Orders (RS501000) form.
For more details about these naming conventions, see Form and Report Numbering and Graph Naming.

Definition of the Processing Graph and Data View
To configure the graph that works with the processing form, you do the following:
• You define the data view for the processing form.
To do this, you use the SelectFrom<Table>.ProcessingView class. This class is derived from the
PXProcessingBase<Table> class, which is a base class for the data views of processing forms. You
can also use one of the types that use the traditional BQL style of data queries, such as PXProcessing or
PXProcessingJoin

To ensure the history of a processing operation is saved correctly, the main DAC of the
processing view must contain the NoteID field. This field must have the PXNote attribute
declared on it.

• You add the Cancel action to the processing graph.
You do this by using the PXCancel class. If the processing form does not have a filter, you use the main DAC
of the processing data view as the type parameter, as shown in the following code.
// Definition of the Cancel button for processing without filtering
public class SalesOrderProcess : PXGraph<SalesOrderProcess>
{
public SelectFrom<SalesOrder>.ProcessingView SalesOrders;
// Main DAC of the processing data view
public PXCancel<SalesOrder> Cancel;
}

• Optional: You replace the names of the default buttons in the graph constructor.
## Part 3: Designing the Layout of an Acumatica ERP Form | 92

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
**Related Links**
• Form Types
• Form and Report Numbering

**Processing Form: UI Guidelines**

In this topic, you can learn the UI guidelines for the processing forms.

Template, Label Sizes, and Other Layout Settings
By default, you should use the following guidelines while designing a processing form:
• For the Selection area, you use the 17-17-14 or 17-14-17 template. For more details about templates,
see Form Layout: Predefined Templates.
• For the Selection area, you use the default label sizes.
• For the table area, you use the Processing preset. For details about presets, see Form Layout: Grid
Presets.
• No Activities, Files, and Notes buttons in the form title bar should be displayed.
• No Files and Notes buttons in the table should be displayed.

For particular forms, the Notes and Files buttons can be required. For example, on the Process
Export Scenarios (SM207035) form, this is the only way a user can download an exported file.
## Part 3: Designing the Layout of an Acumatica ERP Form | 93

• Sections in the Selection area can have captions if the captions make sense.

Recommendations for Organizing the Layout
The following table shows recommendations for organizing the layout of a processing form.

Correct                                                    Incorrect

Put all commands on a single toolbar.                      Do not separate commands into two toolbars.

Use a single field tag for an element with the Date        Do not use two separate fields in a fieldset for the
Range label and two date and time controls for the se-     Start Date and End Date boxes on processing forms.
lection of the start date and the end date.
This approach applies only to processing forms.

UX and Functional Guidelines
The form design should be tailored for screens with a resolution of 1280 x 720.
The number of a processing form should start with 50. For details, see Form and Report Numbering.

**Processing Form: A Form with Only a Grid**

The following topic describes how to configure a processing form that contains only a table and does not contain
the Selection area or table filters (also called quick filters). (In the Classic UI, these forms were based on the
GridView template.) The following screenshot shows an example of a form with this layout.
## Part 3: Designing the Layout of an Acumatica ERP Form | 94

**Figure: A processing form without the Selection area or table filters**

View Definition in TypeScript
For a processing form with only a table (and no Selection area or table filters), you need to use the following in the
**TypeScript file of the form:**
• The createCollection method to define a property for the data view to display a table.
• A class that extends PXView with the full list of fields for the table.
• The Processing preset for the table. For details about presets, see Form Layout: Grid Presets.
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
**ClassID: PXFieldState**
**Type: PXFieldState;**
**Description: PXFieldState;**
**Active: PXFieldState;**
**IsDefault: PXFieldState;**
## Part 3: Designing the Layout of an Acumatica ERP Form | 95

**Application: PXFieldState<PXFieldOptions.CommitChanges>;**
**ImageUrl: PXFieldState;**
**PrivateByDefault: PXFieldState<PXFieldOptions.CommitChanges>;**
**RequireTimeByDefault: PXFieldState<PXFieldOptions.CommitChanges>;**
**Incoming: PXFieldState;**
**Outgoing: PXFieldState;**
}

Layout in HTML
For a processing form with only a table (and no Selection area or table filters), you define the layout by adding one
qp-grid control to the HTML code of the form, as shown in the following example.

<template>
<qp-grid id="grid" view.bind="ActivityTypes">
</qp-grid>
</template>

**Processing Form: A Form with a Selection Area and a Grid**

The following topic describes how to configure a processing form that contains the Selection area and a table. (In
the Classic UI, these forms were based on the FormDetail template.)

Examples of Form Layout
The following screenshot shows an example of a form that contains the Selection area and a table but does not
contain table filters (also called quick filters).

**Figure: A processing form with the Selection area**
## Part 3: Designing the Layout of an Acumatica ERP Form | 96

An example of a processing form that contains the Selection area, table filters (also called quick filters), and a table
is shown in the following screenshot. The table filters are stored in the system data and are displayed for the table
whose data view has the PXFilterable attribute. For an example of a form with stored table filters, see Assign
Opportunities (CR503110) form.

**Figure: A processing form with the Selection area and table filters**

An example of a form that contains a single element in the Selection area and a table is shown in the following
screenshot.

**Figure: A processing form with one element in the Selection area and a table**
## Part 3: Designing the Layout of an Acumatica ERP Form | 97

View Definition in TypeScript
For a processing form with the Selection area and a table (but no table filters), you need to do the following in the
**TypeScript file of the form:**
• For the view that displays elements of the Selection area, you use the createSingle method.
• For the view that displays the table, you use the createCollection method.
• For each view, you use a class that extends PXView with the full list of fields to be displayed.
• You use the gridConfig decorator for the table and the columnConfig decorator for the columns. For
details, see Table (Grid): Configuration of the Table and Its Columns.
• You use the PXFieldOptions.CommitChanges option for a field that corresponds to a filtering
parameter in TypeScript to enable callback.
• A preset for the table: Processing. For details about presets, see Form Layout: Grid Presets.
The following code shows an example of this implementation of a processing form.

import {
PXScreen, createSingle, createCollection, graphInfo, PXView, PXFieldState,
gridConfig, columnConfig, linkCommand, PXActionState, PXFieldOptions
} from 'client-controls';

@gridConfig({ preset: GridPreset.Processing })
export class FABookBalance extends PXView {

@columnConfig({ allowCheckAll: true })
**Selected: PXFieldState;**

@columnConfig({ hideViewLink: true })
FixedAsset__BranchID: PXFieldState;

@linkCommand('ViewAsset')
@columnConfig({ allowUpdate: false })
**AssetID: PXFieldState;**

FixedAsset__Description: PXFieldState;

@linkCommand('ViewClass')
@columnConfig({ allowUpdate: false })
**ClassID: PXFieldState;**

@columnConfig({ hideViewLink: true })
FixedAsset__ParentAssetID: PXFieldState;

@linkCommand('ViewBook')
@columnConfig({ allowUpdate: false })
**BookID: PXFieldState;**

@columnConfig({ allowUpdate: false })
**CurrDeprPeriod: PXFieldState;**

@columnConfig({ allowNull: false })
**YtdDeprBase: PXFieldState;**

@columnConfig({ allowUpdate: false })
FixedAsset__BaseCuryID: PXFieldState;
## Part 3: Designing the Layout of an Acumatica ERP Form | 98

FADetails__ReceiptDate: PXFieldState;

FixedAsset__UsefulLife: PXFieldState;

@columnConfig({ hideViewLink: true })
FixedAsset__FAAccountID: PXFieldState;

@columnConfig({ hideViewLink: true })
FixedAsset__FASubID: PXFieldState;

FADetails__TagNbr: PXFieldState;

@columnConfig({ hideViewLink: true })
Account__AccountClassID: PXFieldState;
}

export class Filter extends PXView {
**OrgBAccountID: PXFieldState<PXFieldOptions.CommitChanges>;**
**BookID: PXFieldState<PXFieldOptions.CommitChanges>;**
**PeriodID: PXFieldState<PXFieldOptions.CommitChanges>;**
**Action: PXFieldState<PXFieldOptions.CommitChanges>;**
**ClassID: PXFieldState<PXFieldOptions.CommitChanges>;**
**ParentAssetID: PXFieldState<PXFieldOptions.CommitChanges>;**
}

@graphInfo({ graphType: 'PX.Objects.FA.CalcDeprProcess', primaryView: 'Filter' })
export class FA502000 extends PXScreen {

**ViewBook: PXActionState;**
**ViewAsset: PXActionState;**
**ViewClass: PXActionState;**

Filter = createSingle(Filter);

Balances = createCollection(FABookBalance);
}

Layout in HTML
You define the layout for a processing form with the Selection area and a table (but no table filters) by adding the
following elements to the HTML code of the form:
• A qp-template element to display the elements of the Selection area. For details on using templates, see
**Form Layout: Predefined Templates.**
• A qp-grid element with the list of records to process.
The following code shows an example of a processing form with this layout.

<template>
<qp-template name="17-17-14" id="formFilter" wg-container="Filter_form">
<qp-fieldset slot="A" id="columnFirst" view.bind="Filter">
<field name="OrgBAccountID" control-type="qp-branch-selector"></field>
<field name="BookID"></field>
<field name="PeriodID"></field>
<field name="Action"></field>
</qp-fieldset>
## Part 3: Designing the Layout of an Acumatica ERP Form | 99

<qp-fieldset slot="B" id="columnSecond" view.bind="Filter">
<field name="ClassID"></field>
<field name="ParentAssetID"></field>
</qp-fieldset>
</qp-template>
<qp-grid id="grid" view.bind="Balances">
</qp-grid>
</template>

#### Activity 3.4.1: To Create the UI of a Processing Form

The following activity will walk you through the process of developing the UI of a processing form.

Story
Suppose that you need to develop the Assign Work Orders (RS501000) form in the Modern UI. The form will have
the Selection area and a table, as shown in the following screenshot.

**Figure: The Assign Work Orders form**

You have already implemented the backend for the form, which includes the RSSVAssignProcess graph and
the RSSVWorkOrder and RSSVWorkOrderToAssignFilter data access classes (DACs). You have also
already added the corresponding tables to the application database.

Process Overview
You will create TypeScript and HTML files for the Assign Work Orders (RS501000) form. In the TypeScript file, you
will define the screen class and view classes for the form. You will also define the following views:
• Filter, which is bound to the Selection area of the form
• WorkOrders, which is bound to the table of the form
In the HTML file, you will define the layout of the form.

Step 1: Creating Files for the Form
To implement the Modern UI version of the Assign Work Orders (RS501000) form, you need to create the TypeScript
and HTML files for the form. Create the files as follows:
1. In the FrontendSources\screen\src\development\screens\RS folder, create a folder with the
RS501000 name if it has not been created yet.
## Part 3: Designing the Layout of an Acumatica ERP Form | 100

2. In the FrontendSources\screen\src\development\screens\RS\RS501000 folder, create the
following files:
• RS501000.ts
• RS501000.html

Step 2: Defining the Screen Class in TypeScript
To define the view of the Assign Work Orders (RS501000) form in the TypeScript file of the form, you define a screen
class and a property for the data view of the form. Do the following:
1. In the RS501000.ts file, add the import directives as follows.

import {
PXScreen, createCollection, graphInfo,
viewInfo, createSingle,
} from "client-controls";

2. Define the screen class for the form, as the following code shows. The class name is the ID of the form.

export class RS501000 extends PXScreen {}

3. For the screen class, add the graphInfo decorator, and specify the graph and the primary view of the form
in the decorator properties, as the following code shows.

@graphInfo({
graphType: "PhoneRepairShop.RSSVAssignProcess",
primaryView: "Filter"
})
export class RS501000 extends PXScreen {}

4. Define the property for the data views of the form, as the following code shows. To initialize the data view of
the Selection area of the form, you should use the createSingle method. For the data view that is used
to display a table, you need to initialize the property with the createCollection method. The method
takes as the input parameter an instance of the view class, which you will define in the next step.

The names of the data view properties should be the same as those in the graph. For example,
if the WorkOrders view is declared in the RSSVAssignProcess graph, the property with
the same name should be declared in the RS501000 screen class.

export class RS501000 extends PXScreen {
@viewInfo({containerName: "Filter Parameters"})
Filter = createSingle(RSSVWorkOrderToAssignFilter);

@viewInfo({containerName: "Work Orders to Assign"})
WorkOrders = createCollection(RSSVWorkOrder);
}

In the viewInfo decorators, you have specified the name of the container for the Selection area and the
table.

Step 3: Defining the View Class for the Selection Area in TypeScript
In the TypeScript file of the form, you need to define a view class for the primary data view of the Assign Work
Orders (RS501000) form, which is Filter.
## Part 3: Designing the Layout of an Acumatica ERP Form | 101

**Proceed as follows:**
1. In the RS501000.ts file, update the list of import directives, as the following code shows.

import {
PXScreen, createCollection, graphInfo,
viewInfo, createSingle,
PXView, PXFieldOptions, PXFieldState,
} from "client-controls";

2. Define the RSSVWorkOrderToAssignFilter class as follows.

export class RSSVWorkOrderToAssignFilter extends PXView {}

3. In the view class, specify the properties for all data fields of the data view that should be displayed in the UI,
as shown below. You use the name of the data field as the property name.

export class RSSVWorkOrderToAssignFilter extends PXView {
**Priority: PXFieldState<PXFieldOptions.CommitChanges>;**
**TimeWithoutAction: PXFieldState<PXFieldOptions.CommitChanges>;**
**ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;**
}

All fields of the view should be defined so that changes are committed to the server; therefore, you have
used the PXFieldOptions.CommitChanges option for the property type.

Step 4: Defining the View Class for the Table in TypeScript
In the TypeScript file of the form, you need to define a view class for the table on the Assign Work Orders
(RS501000) form, which is RSSVWorkOrder. Proceed as follows:
1. In the RS501000.ts file, add gridConfig, columnConfig, and GridPreset to the list of import
directives.
2. Define the RSSVWorkOrder view class as follows.

export class RSSVWorkOrder extends PXView {
@columnConfig({ allowCheckAll: true })
**Selected: PXFieldState;**

@columnConfig({ hideViewLink: true })
**OrderNbr: PXFieldState;**
**Description: PXFieldState;**

@columnConfig({ hideViewLink: true })
**ServiceID: PXFieldState;**

@columnConfig({ hideViewLink: true })
**DeviceID: PXFieldState;**

**Priority: PXFieldState;**

@columnConfig({ hideViewLink: true})
**AssignTo: PXFieldState<PXFieldOptions.CommitChanges>;**
**NbrOfAssignedOrders: PXFieldState;**
**TimeWithoutAction: PXFieldState;**
}
## Part 3: Designing the Layout of an Acumatica ERP Form | 102

For the Selected field, in the columnConfig decorator, you have specified that a user should be able to
select all records on the page by clicking the check box in the column header.
For the OrderNbr, ServiceID, DeviceID fields, and AssignTo fields, in the columnConfig
decorator, you have specified that the selector link should not be displayed.
For the AssignTo field, you have specified that changes in this field should be committed to the server.
3. Add the gridConfig decorator to the RSSVWorkOrder view class, as the following code shows. In
the gridConfig decorator, you must specify the preset property. Because the table is used on the
processing form, you use the Processing preset. For details about presets, see Form Layout: Grid Presets.

@gridConfig({
preset: GridPreset.Processing,
autoAdjustColumns: true
})
export class RSSVWorkOrder extends PXView {
...
}

In the gridConfig decorator, you have also specified that the table width should be adjusted to the
screen width. Otherwise, some of the columns may not be wide enough to display values.
4. Save your changes.

Step 5: Defining the Layout in HTML
The Assign Work Orders (RS501000) form contains the Selection area and a table below it. The Selection area has
two columns, which you can arrange by using the 17-17-14 template, which is a recommended one for processing
forms. To define the layout of the form, do the following:
1. Define the Selection area of the form by adding the qp-template tag with the 17-17-14 template. For the
first two slots, define a fieldset, as shown in the following code. To leave the third slot empty, do not specify
any tags for it.

<template>
<qp-template
id="form-Filter"
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
</template>

Each fieldset has been bound to the same Filter view.
For details about the qp-template tag and slots, see Form Layout: Predefined Templates.
2. In each fieldset, add the field tags for the fields that should be displayed in the corresponding fieldset, as
the following code shows.

<qp-fieldset
## Part 3: Designing the Layout of an Acumatica ERP Form | 103

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

In the first fieldset, you have also specified the width of the labels by using the label-size-xm class. For more
details about CSS classes, see Form Layout: CSS Classes.
3. Define the table that displays repair work orders: Aer the qp-template tag, add the qp-grid tag and
bind it to the WorkOrders view, as the following code shows.

<qp-grid id="grid-WorkOrders" view.bind="WorkOrders"> </qp-grid>

4. Save your changes.

Step 6: Building and Viewing the Form
To build the source files for the Assign Work Orders (RS501000) processing form and view its Modern UI version, do
the following:
1. Run the following command in the FrontendSources\screen folder of your instance.

npm run build-dev --- --env customFolder=development screenIds=RS501000

2. Aer the source files have been built successfully, launch your Acumatica ERP instance, and open the Assign
Work Orders form.
3. On the form title bar, click Tools > Switch to Modern UI. The Modern UI version of the Assign Work Orders
form is displayed. The form should look similar to the form shown in the screenshot in the Story section of
this activity.
4. In the Priority box, select High. Make sure that only repair work orders with the High priority (if any) are
displayed in the table.
5. Click Cancel on the form toolbar.
6. On the form toolbar, click Assign All. Aer all repair work orders in the table have been processed, a report
opens with the result of processing.

### Lesson 3.5: Organizing Complex Layouts

In this lesson, you will review various complex layout examples. You can find more examples in UI Component
Guide.
## Part 3: Designing the Layout of an Acumatica ERP Form | 104

**Form Layout: An Element Next to Another Element**

If you need to define an additional element—for example, a button, another box, or a label—next to another
element, you can add the corresponding tag inside the field tag. Each field tag can include one tag or multiple
tags, such as qp-field or qp-button.

Adding an Element Next to Another Element
**You can add more than one control in place of an element in the following ways:**
• By adding the additional controls inside the field tag.
In this case, the controls inside the field tag are dependent on the behavior of the control bound to the
field tag. For example, when the control bound to the field tag is hidden (for example, due to a feature
switch or by a user in the Section Configuration dialog box), the dependent control is also hidden.
By default, the second control will occupy half of the space dedicated for the original field. If you need
the second control to have a diﬀerent width, you can specify class="col-N", where N is a number of
columns that is less than 12. The minimum width is 2 columns. To move a field to the next line, specify
col-12.
• By replacing the contents of the field tag with controls specified inside the field tag (by using the
replace-content attribute).
In this case, all controls specified in the field tag are independent of each other and the replaced control
(except for nested controls).
If you need to specify the width of the controls, you need to specify col-XX classes for all controls nested
in the field tag unless the controls are nested within each other.

We recommend that you use the approach with adding controls inside the field tag for two related
controls, such as a box and the related check box, or a box and its dynamic description.
Use the approach with replacing of the contents of the field tag for multiple unrelated controls.
Do not use the approach with adding controls inside the field tag if you need to put multiple
controls that are unrelated to each other in a row. If an error occurs in one of these controls, it will be
duplicated in another control, as shown in the following screenshot.

**Figure: Duplicated error**

Adding a Check Box Next to an Element
You can add a check box next to an element (defined by the field tag) so that they both occupy a space of a single
control. To do that, you need to add the qp-field bound to this check box inside the field tag. For example,
suppose that you need to add the Canceled check box next to the Cancel Date box, as shown in the following
screenshot.
## Part 3: Designing the Layout of an Acumatica ERP Form | 105

**Figure: The Canceled check box next to the Cancel Date box**

The following code can be used to add the check box next to the Cancel Date box (which corresponds to the
CancelDate field).

<field name="CancelDate">
<qp-field control-state.bind="CurrentDocument.Cancelled"
config-enabled.bind="false"></qp-field>
</field>

You do not need to specify class="no-label" for the second control to omit the empty label of
the check box because this class is used by default for a qp-field tag inside the field tag.

Adding a Button to the Next Line
You can add a button aer another control so that it is displayed on the next line below the control. To do that, you
need to add the button control inside the field tag and specify class="col-12" for the button control.
For example, suppose that you need to place the Address Lookup button on the next line aer a check box, as
shown in the following screenshot.

**Figure: The Address Lookup button below a check box**

The Address Lookup button is implemented by using the qp-address-lookup tag. The following code shows
an example of moving the Address Lookup button to the next line.

<field name="OverrideAddress">
<qp-address-lookup class="col-12" view.bind="Shipping_Address">
</qp-address-lookup>
</field>

Replacing a Field
You can put any content, such as a text box and a button, instead of an element (that is, occupy the field space with
arbitrary elements) by doing the following:
1. In the field tag, specify the replace-content attribute.
## Part 3: Designing the Layout of an Acumatica ERP Form | 106

2. Optional: If the field whose content you are replacing has no definition in TypeScript, add the unbound
attribute.
3. Inside the field tag, add all tags for the elements you want to add.

Suppose that you want to replace the content of an element with one button that occupies the whole space of the
element, as shown in the following screenshot.

**Figure: A button in place of a field**

You need to add the qp-button tag inside the field tag, as shown in the following code.

<field name="fakename" replace-content unbound>
<qp-button id="buttonShopRates" class="col-12" state.bind="ShopRates"></qp-button>
</field>

Adding Multiple Elements in a Line
You can put multiple elements, such as check boxes, in one line by placing them inside the single field tag and
specifying the proper CSS class. For details about CSS classes, see Form Layout: CSS Classes.
Suppose that you need to show three check boxes in a single line, as shown in the following screenshots.

**Figure: Three check boxes in a single line**

You need to put three qp-field tags inside a single field tag and specify class="col-4" for each of the qp-
field tags, as shown in the following code.

<field name="fake01" unbound replace-content>
<qp-field control-state.bind="StaffScheduleSelected.WeeklyOnSun"
config-enabled.bind="false" class="col-4"></qp-field>
<qp-field control-state.bind="StaffScheduleSelected.WeeklyOnWed"
config-enabled.bind="false" class="col-4"></qp-field>
<qp-field control-state.bind="StaffScheduleSelected.WeeklyOnSat"
config-enabled.bind="false" class="col-4"></qp-field>
</field>

You do not need to set class="no-label" to remove the label before each check box because this
class is used by default in this case.

The col-4 class helps to distribute the check boxes equally, which means that each check box occupies 4 (12 / 3 =
4) columns out of the 12 columns that the replaced element occupies. (If you need to have four columns and need
to distribute them equally, you should use class="col-3".)
## Part 3: Designing the Layout of an Acumatica ERP Form | 107

**Fieldset: Layout Examples**

In this topic, you will find cases of complicated layouts inside a fieldset and instructions on how to implement
them.

Check Boxes Next to Particular Controls
If you need to put check boxes next to particular controls and define them to occupy half of the control space, as
shown in the following screenshot, add the qp-field tag inside the field tag, and specify class="col-6"
for the nested control.

**Figure: Check boxes next to particular controls**

The following code implements this approach.

<qp-fieldset id="groupFields" view.bind="View1">
<field name="Field1">
<qp-field control-state.bind="View1.Override1" class="col-6" >
</qp-field>
</field>
<field name="Field2">
<qp-field control-state.bind="View1.Override2" class="col-6" >
</qp-field>
</field>
<field name="Field3">
<span class="col-6"></span>
</field>
<field name="Field4">
<span class="col-6"></span>
</field>
</qp-fieldset>

Check Boxes Next to All Controls
If you need to put check boxes next to all controls (as the following screenshot shows), add the qp-field tag
inside the field tag. There is no need to specify the class.
## Part 3: Designing the Layout of an Acumatica ERP Form | 108

**Figure: Check boxes next to all controls**

The following code illustrates this approach.

<qp-fieldset id="groupFields" view.bind="View1">
<field name="Field1">
<qp-field control-state.bind="View1.Override1"></qp-field>
</field>
<field name="Date">
<qp-field control-state.bind="View1.Override2"></qp-field>
</field>
<field name="Field3">
<qp-field control-state.bind="View1.Override3"></qp-field>
</field>
</qp-fieldset>

Complicated Layout with Multiple Controls in a Row
Suppose that you need to organize a complicated layout with multiple controls in a row, as shown in the following
screenshot.

**Figure: Complicated layout**

The following code implements the layout above.

<qp-fieldset id="groupAgingSettings-ARStatementCycleRecord"
view.bind="ARStatementCycleRecord" Caption="Aging Settings">
<field name="UseFinPeriodForAging">
</field>
<field name="AgingPeriodsCaption" unbound>
## Part 3: Designing the Layout of an Acumatica ERP Form | 109

<qp-label slot="label" caption="Aging Period (Days)">
</qp-label>
<qp-label caption="Description">
</qp-label>
</field>
<field name="AgeMsgCurrent">
<qp-label slot="label" caption="Current Period">
</qp-label>
</field>
<field name="AgeMsg00">
<div slot="label" class="no-label h-stack">
<qp-field
control-state.bind=
"ARStatementCycleRecord.Bucket01LowerInclusiveBound"
class="col-4">
</qp-field>
-
<qp-field
control-state.bind="ARStatementCycleRecord.AgeDays00"
class="col-4">
</qp-field>
</div>
</field>
<field name="AgeMsg01">
<div slot="label" class="no-label h-stack">
<qp-field
control-state.bind=
"ARStatementCycleRecord.Bucket02LowerInclusiveBound"
class="col-4">
</qp-field>
-
<qp-field
control-state.bind="ARStatementCycleRecord.AgeDays01"
class="col-4">
</qp-field>
</div>
</field>
<field name="AgeMsg02">
<div slot="label" class="no-label h-stack">
<qp-field
control-state.bind=
"ARStatementCycleRecord.Bucket03LowerInclusiveBound"
class="col-4">
</qp-field>
-
<qp-field
control-state.bind="ARStatementCycleRecord.AgeDays02"
class="col-4">
</qp-field>
</div>
</field>
<field name="AgeMsg03">
<div slot="label" class="no-label h-stack">
Over
<qp-field
control-state.bind=
"ARStatementCycleRecord.Bucket04LowerInclusiveBound"
## Part 3: Designing the Layout of an Acumatica ERP Form | 110

class="col-4">
</qp-field>
</div>
</field>
<field name="AgeBasedOn">
</field>
</qp-fieldset>

Multiple Columns in a Fieldset
Suppose that you need to implement the layout that is shown in the following screenshot.

**Figure: Multiple columns in a fieldset**

The following code implements the layout above.

<qp-fieldset id="RouteSelected_10"
view.bind="RouteSelected">
<field name="fake01" unbound replace-content>
<qp-label caption="Start day" class="col-4">
</qp-label>
<qp-label caption="Start time" class="col-4">
</qp-label>
<qp-label caption="Trips per day" class="col-4">
</qp-label>
</field>
<field name="fake02" unbound replace-content>
<qp-field control-state.bind="RouteSelected.ActiveOnSunday"
no-label="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.BeginTimeOnSunday_Time"
no-label="true"
config-time-mode.bind="true" class="col-4">
</qp-field>
<qp-field control-state.bind="RouteSelected.NbrTripOnSunday"
no-label="true" class="col-4">
</qp-field>
## Part 3: Designing the Layout of an Acumatica ERP Form | 111

</field>
<field name="fake03" unbound replace-content>
<qp-field control-state.bind="RouteSelected.ActiveOnMonday"
no-label="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.BeginTimeOnMonday_Time"
no-label="true"
config-time-mode.bind="true" class="col-4">
</qp-field>
<qp-field control-state.bind="RouteSelected.NbrTripOnMonday"
no-label="true" class="col-4">
</qp-field>
</field>
<field name="fake04" unbound replace-content>
<qp-field control-state.bind="RouteSelected.ActiveOnTuesday"
no-label="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.BeginTimeOnTuesday_Time"
no-label="true"
config-time-mode.bind="true" class="col-4">
</qp-field>
<qp-field control-state.bind="RouteSelected.NbrTripOnTuesday"
no-label="true" class="col-4">
</qp-field>
</field>
<field name="fake05" unbound replace-content>
<qp-field control-state.bind="RouteSelected.ActiveOnWednesday"
no-label="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.BeginTimeOnWednesday_Time"
no-label="true"
config-time-mode.bind="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.NbrTripOnWednesday"
no-label="true" class="col-4">
</qp-field>
</field>
<field name="fake06" unbound replace-content>
<qp-field control-state.bind="RouteSelected.ActiveOnThursday"
no-label="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.BeginTimeOnThursday_Time"
no-label="true"
config-time-mode.bind="true" class="col-4">
</qp-field>
<qp-field control-state.bind="RouteSelected.NbrTripOnThursday"
no-label="true" class="col-4">
</qp-field>
</field>
<field name="fake07" unbound replace-content>
<qp-field control-state.bind="RouteSelected.ActiveOnFriday"
no-label="true" class="col-4">
## Part 3: Designing the Layout of an Acumatica ERP Form | 112

</qp-field>
<qp-field
control-state.bind="RouteSelected.BeginTimeOnFriday_Time"
no-label="true"
config-time-mode.bind="true" class="col-4">
</qp-field>
<qp-field control-state.bind="RouteSelected.NbrTripOnFriday"
no-label="true" class="col-4">
</qp-field>
</field>
<field name="fake08" unbound replace-content>
<qp-field control-state.bind="RouteSelected.ActiveOnSaturday"
no-label="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.BeginTimeOnSaturday_Time"
no-label="true"
config-time-mode.bind="true" class="col-4">
</qp-field>
<qp-field
control-state.bind="RouteSelected.NbrTripOnSaturday"
no-label="true" class="col-4">
</qp-field>
</field>
</qp-fieldset>

**Button: Configuration**

In this topic, you can learn how to adjust a button for specific cases.

Action Definition in TypeScript
The actions that are defined in the graph or in the workflow have corresponding commands displayed on the More
menu of the form toolbar by default. You do not need to define them in the TypeScript code of the Acumatica ERP
form.
However, if you need to place a button for an action on an area of the form other than the toolbar or the More
menu, you need to do the following:
• To place a button on a table toolbar, you specify the property with the name of the corresponding action in
the view class of the table, as shown below.
import {
gridConfig,
PXView,
PXActionState
} from "client-controls";

@gridConfig({
preset: GridPreset.Details
})
export class SOLine extends PXView {
**AddInvBySite: PXActionState;**
}
## Part 3: Designing the Layout of an Acumatica ERP Form | 113

• To place a button in a dialog box or somewhere on the form (outside of the form toolbar or table toolbar),
you specify the property with the name of the corresponding action in the screen class, as the following
code shows. You then add the qp-button tag for the action in HTML. For details, see Dialog Box:
Configuration.

import {
graphInfo,
PXScreen,
PXActionState
} from "client-controls";

@graphInfo({
graphType: "PX.Objects.GL.AccountHistoryEnq",
primaryView: "Filter"
})
export class GL401000 extends PXScreen {
**AddInvoiceOK: PXActionState;**
}

• To execute an action when a user clicks the link in a column of a grid, you specify the property with the
name of the corresponding action in the screen class and use the linkCommand decorator for the column
that displays the link. An example of such action is shown in the following code.
@graphInfo({
graphType: "PX.Objects.FA.CalcDeprProcess",
primaryView: "Filter"
})
export class FA502000 extends PXScreen {
**ViewAsset: PXActionState;**
Balances = createCollection(FABookBalance);
}

@gridConfig({
preset: GridPreset.Processing
})
export class FABookBalance extends PXView {
@linkCommand("ViewAsset")
@columnConfig({ allowUpdate: false })
**AssetID: PXFieldState;**
...
}

• To execute an action when a user clicks the link in a field in a fieldset, you specify the property with
the name of the corresponding action in the screen class and use the editCommand property in the
controlConfig decorator for the field that displays the link. An example of such action is shown in the
following code.
@graphInfo({
graphType: "PX.Objects.EP.EPAssignmentMaint",
primaryView: "AssigmentMap"
})
export class EP205000 extends PXScreen {
**OpenForm: PXActionState;**
AssigmentMap = createSingle(EPAssignmentMap);
}

export class EPAssignmentMap extends PXView {
@controlConfig({editCommand: "OpenForm"})
## Part 3: Designing the Layout of an Acumatica ERP Form | 114

**GraphType: PXFieldState<PXFieldOptions.CommitChanges>;**
}

When you include the property of the PXActionState type for the action in the TypeScript code
of a form, this action is automatically bound by name to an action in the graph. The action is not
displayed on the form toolbar by default. To specify explicitly whether the button for the action is
displayed on the form toolbar, you can use the PXButton.DisplayOnMainToolbar property in
the graph’s action declaration.

Button for a Graph Action in HTML
If you need to use an action defined in a graph in your HTML code, you must specify the state attribute for the
button that corresponds to this action, as shown in the following code.

<qp-button id="buttonAddBlanketLine" state.bind="AddBlanketLineOK">
</qp-button>

In the code above, you have used the state attribute and specified the AddBlanketLineOK value for its bind
property, which is the name of the action that is defined in the graph.

Configuration of Button Properties
You use the actionConfig decorator to specify the properties of an action explicitly defined in a view class, such
as an action that corresponds to a button on the table toolbar or a button in a dialog box. The full list of properties
is defined in the IActionConfig interface. The following code uses this decorator.

@localizable
export class LocalizableStrings {
static btn_specifyDatabaseEngine = "Specify Database Engine";
}

export class AU209000 extends PXScreen {
localizableStrings = LocalizableStrings;

@actionConfig({text: LocalizableStrings.btn_specifyDatabaseEngine})
actionAddSqlAttribute: PXActionState;
}

You can also specify the properties of any button by using the config attribute of the qp-button tag in HTML, as
the following code shows.

<qp-button id="pdfPrevPageBtn"
config.bind="{images: { normal: 'svg:main@arrowLeft' } }">
</qp-button>

You can also handle the state and appearance of any action that corresponds to a button or command on the
table toolbar by using the actionsConfig property of the gridConfig decorator, as shown in the following
examples.

// Hides the Refresh button from the table toolbar.
@gridConfig({
actionsConfig: { refresh: { hidden: true } }
})
export class SOLine extends PXView
## Part 3: Designing the Layout of an Acumatica ERP Form | 115

// Adds the Custom Refresh button.
@gridConfig({
actionsConfig: {
refresh: {
renderAs: MenuItem.RENDER_TEXT,
images: {},
text: "Custom Refresh" }
}
})
export class POLine extends PXView

Standard Buttons of a Dialog Box
For the standard buttons of a dialog box—such as OK or Cancel—you must have the dialog-result attribute
specified. Also, if you do not specify the caption attribute, the button's caption will be set to the value of the
dialog-result attribute, and the caption will be localizable by general rules. You can override the default
caption by specifying the caption attribute. You can omit the caption attribute if its value is the same as the
value of the dialog-result attribute.
The following code example shows an approach to declaring the OK button of a dialog box that has a caption.

<qp-button id="buttonOK" caption="Confirm" dialog-result="OK"> </qp-button>

The following code example shows an approach to declaring the Cancel button of a dialog box that does not have a
caption.

<qp-button id="buttonCancel" dialog-result="Cancel"></qp-button>

For more details on buttons of a dialog box, see Dialog Box: Buttons on the Dialog Box.

Configuration of Button Connotation
You can configure a button connotation from frontend by using the class property of the qp-button tag. The
property can have the following values:
• major-button: The button is highlighted with blue, as shown in the following screenshot.

An example implementation of this button is shown in the following code.
<qp-button
id="btnOK" dialog-result="OK"
config.bind="{ validateInput: true }"
caption="Confirm"
class="major-button">
</qp-button>

• minor-button: The button does not have any frame, gray or blue. When a user hovers over the button
with this class, the button gets a gray frame. In the following screenshot, the Reset to Default and Manage
User-Defined Fields buttons have class="minor-button" specified. The Manage User-Defined Fields
button has a gray frame because a mouse hovers over it.
## Part 3: Designing the Layout of an Acumatica ERP Form | 116

By default, a button has a gray frame as shown in the following screenshot.

#### Activity 3.5.1: To Put a Button in the Summary Area

This activity will walk you through adding a button below an element in the Summary area of a form.

Story
Suppose that on the Repair Work Orders (RS301000) form, you need to place a button in the Summary area, below
to the Invoice Nbr. box. A user should be able to click the button to open the related invoice.

This functionality is normally implemented through a link within the box, but you’ll add a button for
educational purposes.

Process Overview
First, you need to implement an action in the backend. Then you’ll map the action in the screen class in TypeScript.
Finally, you’ll specify the button’s location by using the qp-button tag in HTML.

Step 1: Implementing an Action in the Backend
To implement an action that opens an invoice, do the following:
1. In the RSSVWorkOrderEntry graph (Actions section), define the OpenInvoice action, as shown in
the following code.

public PXAction<RSSVWorkOrder> OpenInvoice = null!;
[PXButton, PXUIField(DisplayName = "Open")]
protected virtual IEnumerable openInvoice(PXAdapter adapter)
{
if (WorkOrders.Current.InvoiceNbr == null)
throw new PXException("The invoice has not been created yet.");
var invoiceEntry = PXGraph.CreateInstance<SOInvoiceEntry>();
var invoice = invoiceEntry.Document
.Search<SOInvoice.refNbr>(WorkOrders.Current.InvoiceNbr);

invoiceEntry.Document.Current = invoice;
throw new PXRedirectRequiredException(invoiceEntry, true,
nameof(OpenInvoice))
{
Mode = PXBaseRedirectException.WindowMode.NewWindow
};
}

2. In the RSSVWorkOrderEntry_Workflow.cs file, add the action to the screen configuration and make
the action available only when the invoice exists: In the WithActions method, add the following code.

.WithActions(actions =>
{
...
## Part 3: Designing the Layout of an Acumatica ERP Form | 117

actions.Add(graph => graph.OpenInvoice,
action => action.IsDisabledWhen(!conditions.HasInvoice));
})

3. Rebuild your project.

If you refresh the Repair Work Orders (RS301000) form now, you’ll see that the command associated
with the action has appeared on the More menu. If you want to leave the command there, no frontend
code is needed.

Step 2: Mapping the Action in TypeScript
To map the action in TypeScript, you need to define a property of the PXActionState type with a name identical
to the PXAction property’s name in the backend. Do the following in the RS301000.ts file:
1. In the RS301000 screen class, add the OpenInvoice property, as shown below.

export class RS301000 extends PXScreen {
**OpenInvoice: PXActionState;**
...
}

2. Save your changes.

If you rebuild your code now, command associated with the action will no longer appear on the form,
including on the More menu.

Step 3: Specifying the Button’s Location in HTML
To specify the button’s location in HTML, you need to add the qp-button tag in the proper place. Because you
need to put the button below the Invoice Nbr. box, the qp-button tag should be nested in the InvoiceNbr
field tag. Do the following in the RS301000.html file:
1. In the fieldset with the fsColumnC-Order ID, find the InvoiceNbr field tag.
2. In the InvoiceNbr field tag, add the qp-button tag, as shown in the following code.

<field name="InvoiceNbr">
<qp-button state.bind="OpenInvoice"></qp-button>
</field>

If you build the code now, the button will be displayed on the same line as the Invoice Nbr.
box and take up half the space for the box, as shown below. This happens because nested
controls have col-6 class applied by default.

3. Specify that the button should be displayed on the next line aer the Invoice Nbr. box by specifying the
col-12 class, as shown in the following class.

<field name="InvoiceNbr">
<qp-button state.bind="OpenInvoice" class="col-12"></qp-button>
## Part 3: Designing the Layout of an Acumatica ERP Form | 118

</field>

4. Save your changes.

Step 4: Testing the Button
To test the added button, do the following:
1. Build your code by running the following command in the FrontendSources\screen folder of your
instance.

npm run build-dev --- --env customFolder=development screenIds=RS301000

2. On the Repair Work Orders (RS301000) form, open a repair work order.
3. In the Summary area of the form, verify that the Open button appears below the Invoice Nbr. box, as shown
in the following screenshot.

**Figure: The Summary area of the Repair Work Orders form**

Table (Grid): Layout Examples

In this topic, you can find examples of various layouts with tables.

Table in a Gray Section
Suppose that you need to place a table in a gray section, as shown in the following screenshot.

**Figure: A table in a gray section**
## Part 3: Designing the Layout of an Acumatica ERP Form | 119

To define a table in a gray section, you specify class="framed-section" for the qp-grid tag, as shown in
the following code.

<qp-grid slot="B" id="gridSerialNumbers"
view.bind="MasterSplits" class="framed-section">
</qp-grid>

Table with a Title
Suppose that you need to define a table with a title, as shown in the following screenshot.

**Figure: A table with a title**

To define a table with a title, you specify the caption attribute in the qp-grid tag, as shown in the following
code.

<qp-grid id="gridOrders"
view.bind="BlanketOrderChildrenDisplayList"
caption="Grid Caption">
</qp-grid>

Table with Elements Above It in a Gray Section
Suppose that for a table in a gray section, you need to place elements, such as boxes, above the table and inside
the gray section, as shown in the following screenshot.

**Figure: A table with boxes in a gray section**

To define a table with elements above it in a gray section, you do the following in the HTML code:
1. Add a qp-fieldset tag.
2. Specify the caption attribute for the qp-fieldset tag.
3. Put the following N + 1 field tags in the qp-fieldset tag, where N is the number of elements you need
to define above the table:
## Part 3: Designing the Layout of an Acumatica ERP Form | 120

• N fields for the elements above the table
• One field for the table with the replace-content and unbound attributes
4. Put the qp-grid tag inside the field with replace-content and unbound.

You do not need to specify any classes for coloring because the gray section is displayed for the entire
fieldset by default.

The following code implements this approach.

<qp-fieldset id="groupID" view.bind="View1" caption="Fieldset Caption">
<field name="Field1"></field>
<field name="Field2></field>
<field name="FakeField" replace-content unbound>
<qp-grid id="gridSomeGrid" view.bind="View2"></qp-grid>
</field>
</qp-fieldset>

You generally don't need to explicitly specify the width for the table. However, you may need to do
so in cases where the table's width doesn't automatically match the width of its outer container. You
can specify class="col-12" in the qp-grid tag to force the table to match the width of its outer
container. Thus, in the preceding code example, you would specify the qp-grid tag as follows.

<qp-grid id="gridSomeGrid" view.bind="View2" class="col-12"></qp-grid>

Table with a Title and Elements Above It in a Gray Section
Suppose that for a table in a gray section, you need to specify a table title; further suppose that you need to add
elements above the table inside the gray section, as shown in the following screenshot.

**Figure: A table with a title and with elements above it in a gray section**

To implement this layout, you need to do the following in HTML code:
1. Add a qp-fieldset tag.
2. Add the title for the fieldset by specifying the caption attribute for the qp-fieldset tag.
3. Put the following N + 1 field tags in the qp-fieldset tag, where N is the number of elements you need
to define above the table:
• N fields for the elements above the table
• One field for the table with the replace-content and unbound attributes
## Part 3: Designing the Layout of an Acumatica ERP Form | 121

4. Put the qp-grid tag inside the field with replace-content and unbound.
5. Add the title for the table by specifying the caption attribute for the qp-grid tag.

You do not need to specify any classes for coloring because the gray section is displayed for the entire
fieldset by default.

The following code implements this approach.

<qp-fieldset id="groupID" view.bind="View1" caption="Fieldset Caption">
<field name="Field1"></field>
<field name="Field2></field>
<field name="FakeField" replace-content unbound>
<qp-grid id="gridSomeGrid" view.bind="View2" caption="Grid Caption">
</qp-grid>
</field>
</qp-fieldset>

You generally don't need to explicitly specify the width for the table. However, you may need to do
so in cases where the table's width doesn't automatically match the width of its outer container. You
can specify class="col-12" in the qp-grid tag to force the table to match the width of its outer
container. Thus, in the preceding code example, you would specify the qp-grid tag as follows.

<qp-grid
id="gridSomeGrid"
view.bind="View2"
caption="Grid Caption"
class="col-12">
</qp-grid>

Table with a Fixed Height
By default, the grid is stretched over the whole width of the area. If a grid should not occupy the whole tab or form,
you specify class="fixed-height" for it, as shown in the following example.

<qp-grid id="gridSomeGrid" view.bind="GridView" class="fixed-height">
</qp-grid>

Table (Grid): Configuration of the Table Toolbar

In this topic, you can learn how to configure actions on the table toolbar.

Definition of Actions
The actions of the table toolbar must be defined in TypeScript. You use the PXActionState in the grid view
class, which is the inheritor of the PXView class. (See the example below.) Actions of the table toolbar are not
displayed on the form toolbar.

export class SOLine extends PXView {
**AddInvoice: PXActionState;**
}
## Part 3: Designing the Layout of an Acumatica ERP Form | 122

State and Appearance of Actions
You can also handle the state and appearance of any action that corresponds to a button or command on the
table toolbar by using the actionsConfig property of the gridConfig decorator, as shown in the following
examples.

// Hides the Refresh button from the table toolbar.
@gridConfig({
actionsConfig: { refresh: { hidden: true } }
})
export class SOLine extends PXView

// Adds the Custom Refresh button.
@gridConfig({
actionsConfig: {
refresh: {
renderAs: MenuItem.RENDER_TEXT,
images: {},
text: "Custom Refresh" }
}
})
export class POLine extends PXView

You can also use the actionConfig decorator to specify properties of an action that is explicitly defined in the
view class for the table.

More Button
If all buttons of the table toolbar cannot be displayed on the screen because of the screen’s width, you can make
the system show the More button in the table toolbar, as shown in the following screenshot.

**Figure: The More button**

To display the More button for the table toolbar, you use the wrapToolbar property of the gridConfig
decorator, as the following code shows.

@gridConfig({
wrapToolbar: true
})
export class SOLine extends PXView
## Part 3: Designing the Layout of an Acumatica ERP Form | 123

Menu Button
You may need to group multiple commands under one menu button, as shown with the Create Activity menu
button in the following screenshot.

**Figure: Menu button**

If you need to configure a menu button with static commands for the table toolbar, you use the topBarItems
property of the gridConfig decorator, as the following example shows.

@gridConfig({
topBarItems: {
**TestMenu: {**
type: "menu-options",
index: 1,
config: {
images: {
normal: "svg:main@external" },
options: {
first: {
text: "First",
commandName: "First"
},
second: {
text: "Second",
commandName: "Second"
}
}
}
}
}
})
export class SOLine extends PXView

If you need to define the list of menu commands dynamically, you use the PXAction.SetMenu method in
the constructor of the graph or in the overridden Initialize method in the graph extension, as shown in the
## Part 3: Designing the Layout of an Acumatica ERP Form | 124

following example. In this case, you do not need to specify the menu commands in the topBarItems property of
the gridConfig decorator.

public class MyGraphMaint : PXGraph<MyGraphMaint>
{
public PXAction<MyDAC> MyMenuAction;
private const string MenuAction1 = "FirstAction";
private const string MenuAction2 = "SecondAction";

public MyGraphMaint()
{
MyMenuAction.SetMenu(new[]
{
new ButtonMenu(MenuAction1, Messages.Command_MenuAction1, null),
new ButtonMenu(MenuAction2, Messages.Command_MenuAction2, null),
});
}
}

[PXLocalizable]
public static class Messages
{
public const string Command_MenuAction1 = "First Command";
public const string Command_MenuAction2 = "Second Command";
}

Table Toolbar Button That Opens a Dialog Box
You can add a button that opens a dialog box to the table toolbar by using the topBarItems property of the
gridConfig decorator. The action that corresponds to this button can be defined only in frontend and has no
corresponding action in the graph.
To implement an action that opens a dialog box, in the topBarItems property of the gridConfig decorator,
you specify the following:
1. The internal name of the action
2. The index of the action on the table toolbar
3. The configuration of the action:
• commandName: ExecuteCommand
• popupPanel: The name of the dialog box
• text: The text on the button
For example, suppose that you need to define an action that opens the PanelRef dialog box (which is defined
with the qp-panel control in HTML). An example of such action is shown in the following code.

@gridConfig({
topBarItems: {
**PanelRef: {**
index: 0,
config: {
commandName: "ExecuteCommand",
popupPanel: "PanelRef",
text: Labels.ReferenceDesignators,
}
}
...
## Part 3: Designing the Layout of an Acumatica ERP Form | 125

})

For details about wrappers for such action, see Testing of the Modern UI: Frontend Actions in Wrappers.

Standard Buttons
When you define a table on the form, a set of buttons is added to the table toolbar by default. The following table
lists the names of the buttons and names of corresponding actions in code.

Button Name                                                   Action Name

Refresh                                                       refresh

Add Row                                                       insert

Delete Row                                                    delete

Fit to Screen                                                 adjust

Export to Excel                                               exportToExcel

Load Records from File                                        import

Filter Setting                                                filter

You can refer to a standard button by its action name in the TypeScript code, for example, to modify visibility in the
actionConfig decorator. For more details, see State and Appearance of Actions.

Table (Grid): Configuration of the Search in the Table

To configure the search in the table, you need to specify fast filter settings for the table. A fast filter is applied to the
table when a user enters a value in the Search box of the table or in another box on an Acumatica ERP form if the
field that corresponds to this box is used for fast filtering.

Hiding the Search Box or Changing Its Position
By default, the Search box of a table is displayed on the filter bar of the table. You can hide the Search box or
display it on the table toolbar by using the showFastFilter property in the gridConfig decorator, as the
following example shows.

@gridConfig({
showFastFilter:
GridFastFilterVisibility.False
})
export class FieldValue
extends PXView

Including Columns in the List of Searched Fields
To include columns in the list of searched fields, you use the allowFastFilter property in the columnConfig
decorator for the needed columns or the fastFilterByAllFields property in the gridConfig decorator.
**Take into account the following considerations:**
## Part 3: Designing the Layout of an Acumatica ERP Form | 126

• By default, the search is performed in all string columns.
• To exclude particular columns from the search, fastFilterByAllFields is not needed; you need to
use allowFastFilter: false.
• To have only particular columns searched, you can set fastFilterByAllFields: false and
allowFastFilter: true for only these columns.

Using a Field Value for Fast Filtering
You may need to filter records in the table by a value that a user has specified in a box on the Acumatica ERP form.
To use a field value for fast filtering, you do the following:
1. Specify an alias for the view model of the qp-grid element, as the following code shows.

<qp-grid id="gridSiteStatus" view.bind="ItemInfo"
view-model.ref="gridSiteStatusVM">
</qp-grid>

2. Use this alias in the qp-fast-filter attribute of the field that should be used as a fast filter for the table,
as shown below.

<field name="Inventory" qp-fast-filter.bind="gridSiteStatusVM">
</field>

You can also use one field in multiple fast filters by providing an array of view model aliases in the
qp-fast-filter attribute, as the following code shows.

<field name="Inventory"
qp-fast-filter.bind="[gridSiteStatusVM, gridRelatedItemsVM]">
</field>

**Date and Time Control: Configuration**

In this topic, you can learn how to adjust a date and time control.

Date and Time Control Definition
To define a date and time control, in the data access class (DAC), you need to add one of the date and time
attributes (such as PXDate, PXDateAndTime, PXDBDate, PXDBTime, or PXDBDateAndTime) to the property
field, as shown in the following code.

public abstract class effectiveAsOfDate :
PX.Data.BQL.BqlDateTime.Field<effectiveAsOfDate> { }
[PXDBDate]
[PXUIField(DisplayName = "Effective As Of")]
public virtual DateTime? EffectiveAsOfDate { get; set; }

In the TypeScript and HTML code, you then define a field with no additional settings specified. For details, see UI
**Definition in HTML and TypeScript: General Information.**
## Part 3: Designing the Layout of an Acumatica ERP Form | 127

Separate Boxes for Date and Time
If you need to display the date and the time in two separate boxes, you define the date and time control, as shown
in the following example. You use a nested qp-field control with the timeMode property set to true in config.

<field name="ContactsExportDate_Date">
<qp-field
class="col-3 no-label"
control-state.bind=
"EmailSyncAccountFilter.ContactsExportDate_Time"
config-time-mode.bind="true">
</qp-field>
</field>

Input Mask and Display Mask
The date and time mask configuration are taken from the locale preferences and do not require any additional
setup. However, if you need to specify the input mask and the display mask, you specify the value of the
InputMask or DisplayMask property of the date and time attribute that is assigned to the DAC property field.
You use the standard and custom date and time format strings.
The following example shows the use of the InputMask and DisplayMask properties.

public abstract class parameter1 :
PX.Data.BQL.BqlDateTime.Field<parameter1> { }
[PXDateAndTime(DisplayMask = "D", InputMask ="d")]
[PXUIField(DisplayName = "Parameter 1")]
public virtual DateTime? Parameter1 { get; set; }

Relative Dates
The date and time control can display relative dates—that is, values such as @Today or @WeekStart (shown below).

**Figure: Relative dates**
## Part 3: Designing the Layout of an Acumatica ERP Form | 128

To specify that a user can enter relative dates in the control, you need to implement the RowSelected event
handler. In the event handler, you call the PXDatetimeFieldState.showRelativeDates method to
indicate that the values are allowed.
An example of such a handler is shown in the following code.

@handleEvent(CustomEventType.RowSelected, { view: "Rules" })
onEPRuleConditionSelected(
args: RowSelectedHandlerArgs<PXViewCollection<EPRuleCondition>>)
{
const ar = args.viewModel.activeRow;

ar.Value?.to(PXDatetimeFieldState).showRelativeDates();
}

Error, Warning, or Informational Notification: Configuration

In this topic, you can find information about how to configure an error, warning, or informational notification to be
displayed on an Acumatica ERP form.

Configuration from the Backend
To display an error, warning, or informational notification on an Acumatica ERP form, you generally do not need
to explicitly add the qp-info-box control in the frontend code of an Acumatica ERP form. Instead, you should
validate the data on the backend and throw an exception of the PXSetPropertyException type, as described
in Data Validation: Validation of Field Values and Data Validation: Validation of a Data Record. In this case, the
notification is displayed automatically for the field or data record for which the exception has been thrown.

Notification Defined in the Frontend
You may need to display a static notification on an Acumatica ERP form or display a notification in a specific place
on the form. In this case, you can explicitly define the qp-info-box control in the HTML code of an Acumatica
ERP form.
In the following example, the notification displays the message that is defined by the state of the field specified in
the control. You need to define this field in the data access class that provides data for the Acumatica ERP form and
in the TypeScript code of the form.

The notification is not displayed if the field specified in the state attribute does not have any error,
warning, or informational message attached (that is, its state is none).

<qp-info-box state.bind="QuickProcessParameters.AvailabilityMessage">
</qp-info-box>

The notification in the following code displays the error, warning, or informational message that is assigned to
any of the fields or records of the view specified in the control. If no error, warning, or informational messages are
assigned to the fields or records of the view, the notification is not displayed.

<qp-info-box view.bind="QuickProcessParameters"></qp-info-box>

You can also specify the type and text of the notification message directly in the control, as the following example
shows.

<qp-info-box caption="An informational message" type="info">
## Part 3: Designing the Layout of an Acumatica ERP Form | 129

</qp-info-box>

You define the position of the notification on an Acumatica ERP form by placing the qp-info-box control in the
needed location in HTML code of the form. By default, the notification is stretched through the whole container
where it is added. For example, if the qp-info-box tag is added inside the qp-template tag, the notification
has the same width as the whole template.

**Tab: Configuration**

In this topic, you can learn how to define the layout of a tab and how to adjust the tab visibility.

Tab Layout
**You organize the layout of a tab as follows:**
• If the tab will contain only a table, see Table (Grid).
• If the tab will not contain only a table, you use a nested qp-template tag. For details about the
organization of the layout with qp-template, see Form Layout: Predefined Templates.

Tab Layout in an Extension
The ref attribute specifies the ID of the tab that is defined in an extension, as shown in the following code.

<qp-tab ref="tabUserInfo_Content">
</qp-tab>

The tab in an extension can be defined as the following code shows. In this example, you need to use nested
template tags. The first template tag is a template for the whole extension, and the second one is a template
for the tab content.

<template>
<template id="tabUserInfo_Content">
<qp-template ...>
</qp-template>
</template>
</template>

Conditional Visibility of a Tab
To display the tab conditionally, you can use visible.bind='<condition>' for the qp-tab tag, as shown in
the following example.

<qp-tab id="tabDuplicates" caption="Duplicates" ref="tabDuplicates_Content"
visible.bind="Lead.DuplicateFound.value === true"></qp-tab>

However we recommend that you define conditional visibility of a tab in the graph code. For details, see
Configuration of the User Interface in Code.
## Part 4: Localizing the Modern UI | 130

## Part 4: Localizing the Modern UI
In this part, you will learn how to localize the Modern UI.

### Lesson 4.1: Localizing the Modern UI

In this lesson, you will learn which techniques are available for localization of the Modern UI.

**UI Localization: General Information**

Acumatica ERP provides built-in localization functionality, so you can easily translate Acumatica ERP into any
language without using third-party products. You can collect the strings used in the whole system or on a particular
form, and translate them for any locale available in Acumatica ERP. Once the translated strings are entered and
applied, the application does not require any recompilation or reinstallation.
For more information about how to use the built-in localization functionality, see Translation Process. To support
this functionality in the customization code, you must prepare data access classes (DACs) and the code.

Applicable Scenarios
Generally, you need to support localization of the user interface in the application code if you develop any code
that aﬀects the user interface of Acumatica ERP. Localization of the user interface is crucial if the code that you
develop is included in a customization project that is used in the situations such as the following:
• Your customization project is intended for use in multiple countries with diﬀerent languages.
• In the region where the customization project is intended to be distributed, there are legal requirements to
provide products and services in the local language.
• Your customization project is intended for use in an organization that operates in multiple countries.

Strings That Can Be Localized
By using the Translation Dictionaries (SM200540) form, you can add translations for the string constants that are
collected from the code of the application, and save them to the database. When a user signs in with a specific
language, the system loads the translations and displays the translated strings to the user. For more information on
localization, see Locales and Languages.
**The system collects for localization the string constants that are specified in the following items of the application:**
• The DisplayName property of the PXUIField attribute of the fields of data access classes (DACs) and
actions of graphs
• The AllowedLabels property of the PXStringList and PXIntList attributes
• The Values property of the classes that implement the ILocalizableValues interface
• The captions of form, grid, and panel controls and labels of input controls, which are specified in ASPX for
the Classic UI
• The UI names in the caption attribute of the respective tags in HTML for the Modern UI
• The containerName parameter of the viewInfo decorator in TypeScript for the Modern UI
• The titles of all forms in the site map, and workspace and category names
• Report elements (such as text box labels and diagram agendas)
• public const string fields of the classes marked with the PXLocalizable attribute
## Part 4: Localizing the Modern UI | 131

You can also translate user input to multiple languages and store translations in the database. For more
information about the localization of user input, see UI Localization: Multilanguage Fields.
**Related Links**
• Locales and Languages
• Translation Dictionaries

**UI Localization: TypeScript and HTML Code of the Modern UI**

The following items in the TypeScript and HTML code of the Modern UI are localized automatically—that is, you do
not need to specify anything for them in the TypeScript or HTML code:
• The UI names in the caption attribute of the respective tags in HTML
• The containerName parameter of the viewInfo decorator in TypeScript
If you want to localize any other text in the TypeScript or HTML code, you need to use one of the approaches
described in the following sections.

Localizing Strings in TypeScript Code
If you need to localize the value of an HTML attribute, such as text or toolTip, you need to declare a class with
the strings for localization and use the localizable decorator on this class, as shown in the following example.

import {
localizable
} from "client-controls";

@localizable
class ActionCaption {
static AddRelatedTableRelations = "Add Related Table";
static BrowseForRelation = "Add Relations";
static ShowAvailableValues = "Combo Box Values";
}

@localizable
class ActionToolTip {
static AddRelatedTableRelations = "Add a relation between tables";
static BrowseForRelation = "Add a related table for the selected table";
static ShowAvailableValues = "Display the list of combo box values";
}

@gridConfig({
preset: GridPreset.Details,
initNewRow: true,
autoAdjustColumns: true,
autoRepaint: ["JoinConditions", "RelatedTables", "TablesInformation"],
topBarItems: {
moveUpRelations: {...},
moveDownRelations: {...},
addRelatedTableRelations: {
index: 5,
config: {
commandName: "addRelatedTableRelations",
text: ActionCaption.AddRelatedTableRelations,
toolTip: ActionToolTip.AddRelatedTableRelations
## Part 4: Localizing the Modern UI | 132

}
}
}
})
export class GIRelation extends PXView { ... }

You can also use the localizable decorator to mark a localizable string, as shown in the following examples.
export class QpDacBrowserCustomElement {
@localizable
static NothingFoundMsg: string =
"No entries have been found for the {filter}";
}

You can also specify the context for the message in the parameter of the decorator, as shown in the following
code. The value of the parameter is displayed in the Key column of the Key-Specific Values tab of the Translation
Dictionaries (SM200540) form. For example, the value of the parameter can help to distinguish two messages if they
have the same identifiers and are declared in the classes with the same names.

@localizable("PX.Objects.AP.Messages")
class Messages {
static LinesHintSingleLine = "line selected";
}

Using Localizable Strings Defined in TypeScript in HTML Code
If in HTML code you need to use the localizable string that is defined in TypeScript, you need to do the following:
1. You define a property in the screen class that corresponds to the class with localizable messages, as shown
in the following example.

//The class with localizable messages
@localizable
export class LocalizableStrings {
static btnPrevDataField_tooltip = "<Tooltip text>";
}

//The screen class
@graphInfo({
graphType: 'PX.BusinessProcess.UI.MobileNotificationMaint',
primaryView: 'Notifications'
})
export class SM204004 extends PXScreen {
//The property that corresponds to the class with localizable messages
LocalizableStrings = LocalizableStrings;
...
}

2. You use this property in HTML code, as shown in the following example.

<qp-button id="btnPrevDataField"
caption="Insert Previous Data Field"
tooltip.bind = 'LocalizableStrings.btnPrevDataField_tooltip'>
</qp-button>
## Part 4: Localizing the Modern UI | 133

Localizing Strings in HTML Code
To make the content of a tag in HTML localizable, you mark the tag with the i18n attribute, as shown in the
following example. This is the standard attribute for the localization of HTML code.

<a id="UserPrefs_form2_OutlookLink"
href="~/../../../OutlookAddinManifest" i18n>
Download Outlook Add-In Manifest
</a>

You can also mark any attribute in HTML with the .i18n attribute suﬀix to make the value of the attribute
localizable, as shown in the following example.

<custom-tag text.i18n="Hello, World!"></custom-tag>
## Part 5: Troubleshooting the Modern UI | 134

## Part 5: Troubleshooting the Modern UI
In this part, you will learn how to troubleshoot the Modern UI.

### Lesson 5.1: Troubleshooting the Modern UI

In this lesson, you will learn how to do the following:
• Troubleshoot the frontend code in the browser
• Force UI changes to be displayed in the browser
• Specify the log level for building the source code
• Build the source code for a particular tenant

**Modern UI Troubleshooting: General Information**

If you experience issues with the code developed for the Modern UI, you may need to use the troubleshooting
techniques.

Applicable Scenarios
**You troubleshoot the Modern UI in the following cases:**
• UI components are not displayed correctly.
• UI components work incorrectly.
• Build of the Modern UI fails.

Modern UI Troubleshooting
To troubleshoot issues with the Modern UI, you can use the built-in browser tools. You can find brief descriptions of
particular troubleshooting scenarios in Modern UI Troubleshooting: Developer Tools in the Browser.
If for some reason the changes you have made to the code of the Modern UI are not displayed, you may find useful
the information in Modern UI Troubleshooting: Unreflected Changes in the Modern UI.
For details about the build options that may help you to find the issue with the Modern UI code, see Modern UI
**Troubleshooting: Build Options.**

**Modern UI Troubleshooting: Developer Tools in the Browser**

You may need to use the developer tools available in the web browser to troubleshoot the Modern UI. The sections
below provide details about troubleshooting in the Chrome web browser. The details for other web browsers may
be slightly diﬀerent.
To open the developer tools of the browser, you press F12 on the keyboard or select Developer tools in the menu
of the browser.
For information on debugging source code using the developer tools, see UI Events: Debugging the UI Code in the
Browser.
## Part 5: Troubleshooting the Modern UI | 135

Viewing the Data that Is Posted to the Server and Returned from the Server
On the Network tab of the developer tools, you can find information about the requests that have been used to
display the Acumatica ERP form. The requests that may be useful for debugging have the form ID as their name.
You can filter out these requests by using the Filter button on the toolbar of the Network tab and selecting the
Fetch/XHR filter.

You can select the Preserve Log check box on the toolbar of the Network tab to keep the requests
during navigation or a page reload.

When you open a form, you have the first GET request with structure as the name. This request retrieves all the
screen information from the server. If this request has already been executed, most of the data returned with this
request is cached. You can turn oﬀ this behavior by selecting the Disable cache check box on the toolbar of the
Network tab.
The POST requests with the form ID as the name are not cached. They contain information about the changes made
on the form and the responses to these changes from the server. The Payload tab provides information about what
has been posted to the server in JSON format. In the data field of the payload, you can see the data changes that
has been posted to the server. The server will use this data to update PXCache data. For example, the following
screenshot shows the data that is sent when the Repair Item check box is cleared on the form.

**Figure: Payload data**

The viewsParams and controlsParams fields contain information about the views and controls that need
to be reloaded. The viewsParams field contains the views that are initialized with the CreateSingle method
in TypeScript. The controlsParams field contains the views that are initialized with the CreateCollection
method and other controls, such as the form toolbar.
For the POST request, on the Preview tab (on the right pane of the Network tab), you can see the data that is
posted back from the server. In the fieldStates field, you see the field states for all fields that are defined in
the TypeScript code. You can use this data to check whether the correct field state has been sent from the server.
For example, in the following screenshot, the Repair Item Type box is unavailable for editing, as indicated by
enabled: false in the field state for the UsrRepairItemType field.
## Part 5: Troubleshooting the Modern UI | 136

**Figure: Field state**

For the GET request, in the fieldStates field on the Preview tab, you can see the default state of
the element.

Opening Developer Tools for a Form That Opens in a New Tab
By default, when an Acumatica ERP form is opened in a new tab, such as when you click on a link on a form, the
developer tools are not open for the form in the tab and you miss the request that loads the form for the first time.
You can turn on the opening of the developer tools for a new tab if the developer tools are open in the current tab.
In Chrome, you select the Auto-Open DevTools for popups check box on the Settings menu of the developer tools.

Importing and Exporting HAR Files
You can export information about all requests that have been made during the current session with developer tools
by using the Export HAR button on the toolbar of the Network tab of the developer tools. You may need to provide
this file to your Acumatica support provider. To import the HAR file, you use the Import HAR file button.

**Modern UI Troubleshooting: Unreflected Changes in the Modern UI**

In some cases, when you have made changes to the sources of the forms that have been migrated to the Modern UI
and rebuilt the sources, you may not see these changes reflected in the web browser when you open one of these
forms.
**The following actions may help you resolve this issue:**
• Restart your Acumatica ERP instance by doing one of the following:
• Modify and save the Web.config file.
• Click the Restart Application button on the Apply Updates (SM203510) form.
• Use the Web Server (IIS).
• Sign in to your Acumatica ERP instance as an administrative user, and navigate to the form whose changes
do not appear in the web browser. Press F12 on your keyboard to open the developer tools in the web
browser. On the Network tab, select the Disable cache check box, and refresh the page. If your changes to
the form still do not appear, proceed to the next instruction.
## Part 5: Troubleshooting the Modern UI | 137

The actions in this instruction are based on the assumption that you are using the Chrome
web browser. The specific actions may diﬀer for other web browsers.

• On the More menu of the Apply Updates form, click the Reset Caches command. Navigate back to the form
whose changes do not appear in the web browser, and refresh the page.

**Modern UI Troubleshooting: Build Options**

For troubleshooting errors during building of the Modern UI, you can use additional build options, which are
described in the following sections.

Log Level for Building the Source Code
You can specify the log level in the build command, as shown in the following example. The log level determines
the minimum threshold at which a logging method should be enabled.

npm run build-dev --- --env LOG_LEVEL=trace

**The following log levels are available:**
• fatal
• error
• warn
• info
• debug
• trace
For more details on log levels, see documentation of the pino logger.
When the build is completed, the links to the log files are displayed along with the result of the build.

Building of the Source Code for a Particular Tenant
**As described in Customization Project with UI Changes: How UI Customization Works, the source code for**
published changes to the Modern UI in a particular tenant is located in the FrontendSources\screen
\src\customizationScreens\<Tenant Name>\screens folder. For troubleshooting errors during
building of the Modern UI changes of a particular tenant of your instance, you can run the following command
in the FrontendSources folder, where TenantName is the tenant name in the Login Name box of the Tenants
(SM203520) form.

npm run build-dev --- --env tenant=TenantName

Before building the source code for changes to the Modern UI, be sure to follow the instructions
described in Performing the Prerequisite Actions.

You can use the tenant=TenantName parameter with any other parameters, such as screenIds or modules,
or with the watch command with parameters. For details about the parameters, see Modern UI Development:
Building the Source Code.
## Part 5: Troubleshooting the Modern UI | 138

By default, the system executes the npm run build command (instead of npm run build-
dev) during publication of a customization project with the Modern UI changes. You can adjust this
behavior by adding the respective keys in the Web.config file. For details about the keys, see
**Customization Project with UI Changes: General Information.**
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 139

## Part 6: Customizing Acumatica ERP Forms in HTML and
TypeScript
In this part, you will learn about how you can customize the Modern UI of Acumatica ERP in HTML and TypeScript.
You can also find information about how to include changes to the Modern UI in a customization project.

### Lesson 6.1: Customizing the Modern UI

In this lesson, you will learn how to add new elements, such as boxes and tabs, to the UI of an Acumatica ERP form
in TypeScript and HTML.

**UI Customization Development: General Information**

When you customize the UI of Acumatica ERP in TypeScript and HTML, you may add new Acumatica ERP forms or
customize existing ones. For details on implementing a new form, see UI Definition in HTML and TypeScript: General
Information. For a form whose UI you need to customize, you add TypeScript and HTML files for customization
extensions of the form. Each file name starts with the screen ID and ends with a postfix that indicates the purpose
of the extension, as you can see in the S0301000_Customization1.ts file name.

The Modern UI changes are defined for each tenant of an Acumatica ERP instance independently.

Applicable Scenarios
**You may need to customize the UI of Acumatica ERP in TypeScript and HTML if any of the following scenarios apply:**
• Your customization project is introducing capabilities that are not provided by Acumatica ERP.
• You are developing integration with an external system.
• Your customization project needs to support custom workflows that integrate multiple systems.

Extension in TypeScript
To define an extension of an existing Acumatica ERP form in TypeScript, you use TypeScript mixins. For details
about mixins, see https://www.typescriptlang.org/docs/handbook/mixins.html#alternative-pattern.
In the TypeScript file of an extension, you define an interface that extends the screen class and a class with the
same name the interface has.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 140

**We recommend that you make sure the following postfixes match: the posfix in the name of the**
TypeScript file with the extension, and the postfixes in the names of the classes and interfaces that
extend original classes.
In the examples below, the following should be declared in the GL401000_MultiCurrency.ts
file:
• The GL401000_MultiCurrency and GLHistoryEnqFilter_MultiCurrency
classes
• The GL401000_MultiCurrency and GLHistoryEnqFilter_MultiCurrency
interfaces
The _MultiCurrency postfix of the file matches the postfix of the classes and the interfaces.

Instead of extending the screen class, the interface can extend other extension classes or multiple classes (such
as the screen class and an extension class). The order of the applied extensions is defined similarly to the way it is
defined for graph extensions in C# code. For details about the order, see The Order in Which Extensions Are Loaded.
In the extension class, you do any of the following:
• Initialize new data views in the same way as you do in the screen class. For details about the screen class
and view classes, see Screen Class in TypeScript and View Classes in TypeScript.
• Optional: In the parameter of the featureInstalled decorator, specify the feature for which the
extension should be available.
• Optional: Define new actions in the same way as you do in the screen class. For details about action
definitions, see Action Definitions in TypeScript and Button: Configuration.
• Optional: Adjust the TypeScript code of the original form.
An example is shown in the following code.

import { featureInstalled, FeaturesSet } from "client-controls";
import { GL401000 } from "src/screens/GL/GL401000/GL401000";

export interface GL401000_MultiCurrency extends GL401000 { }
@featureInstalled(FeaturesSet.Multicurrency)
export class GL401000_MultiCurrency {
}

For each data view that you need to modify, you add an interface and a class for the extension data view, as shown
in the following example.

import { featureInstalled, FeaturesSet } from "client-controls";
import { GLHistoryEnqFilter } from "src/screens/GL/GL401000/GL401000";

export interface GLHistoryEnqFilter_MultiCurrency
extends GLHistoryEnqFilter { }
@featureInstalled(FeaturesSet.Multicurrency)
export class GLHistoryEnqFilter_MultiCurrency {
**ShowCuryDetail: PXFieldState<PXFieldOptions.CommitChanges>;**
}

For more examples of adjustments of TypeScript code, see UI Adjustments in HTML and TypeScript: TypeScript
Examples.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 141

Extension in HTML
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

#### Activity 6.1.1: To Add Elements to an Acumatica ERP Form

This activity will walk you through the process of adding elements to an existing Acumatica ERP form.

Story
Suppose that you need to add two elements to the Stock Items (IN202500) form, as shown in the following
screenshot.

**Figure: Elements to be added to the Stock Items form**

That is, the customization of this form will include adding the following elements to the Item Defaults section of
the General tab:
• The Repair Item check box, which will be used to indicate whether the selected stock item is a repair item.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 142

• The Repair Item Type box, which will hold the repair item type to which the repair item belongs. The box
will contain the following predefined options: Battery, Screen, Screen Cover, Back Cover, or Motherboard.
You have already implemented the backend of the form in the PhoneRepairShop customization project, which
includes the following additions:
• Two custom field declarations in the extension of the IN.InventoryItem data access class (DAC).
• One custom event handler, which you have added to the extension of the InventoryItemMaint graph.
You have used the RowSelected event handler to configure the UI presentation logic.
You have also already added the custom UsrRepairItem and UsrRepairItemType columns to the
InventoryItem database table of the application database.

Process Overview
You will create an extension of the Modern UI of the Stock Items (IN202500) form in TypeScript and HTML, build the
source code of the UI of the form, and test the changes.

Step 1: Creating Files for the Extension
To create the Modern UI for the new UI elements on the Stock Items (IN202500) form, you need to create TypeScript
and HTML files for the form.
In the FrontendSources\screen\src\development\screens\IN\IN202500\extensions folder,
create the following files:
• IN202500_PhoneRepairShop.ts
• IN202500_PhoneRepairShop.html

Step 2: Extending the Screen Class in TypeScript
To customize the Stock Items (IN202500) form in TypeScript, you need to extend the screen class of the form. Do the
following:
1. In the IN202500_PhoneRepairShop.ts file, add the following import directive. The directive
imports the IN202500 class, which is the screen class of the Stock Items form.

import {
IN202500,
} from "src/screens/IN/IN202500/IN202500";

2. Define the interface that extends the IN202500 screen class of the form and the class with the same name
as the interface name as follows.

export interface IN202500_PhoneRepairShop extends IN202500 { }
export class IN202500_PhoneRepairShop {

}

Step 3: Extending the View Class in TypeScript
To add elements to the Item Defaults section of the General tab of the Stock Items (IN202500) form in TypeScript,
you need to extend the view class that provides data for the Item Defaults section. Proceed as follows:
1. Review the IN202500.html file in the FrontendSources\screen\src\screens\IN\IN202500
folder. You can see that the property for the view class for the Item Defaults section of the General tab of
the form is ItemSettings, as shown in the following screenshot.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 143

**Figure: The property for the view class**

In the IN202500.ts file in the same folder, find the name of the view class that corresponds to this
property (see the following screenshot).

**Figure: The view class**

2. In the IN202500_PhoneRepairShop.ts file, update the list of import directives, as the following
code shows.

import {
IN202500,
ItemSettings
} from "src/screens/IN/IN202500/IN202500";
import {
PXFieldState,
PXFieldOptions,
} from "client-controls";

3. Add an interface and a class for the extension data view as follows.

export interface ItemSettings_PhoneRepairShop extends ItemSettings { }
export class ItemSettings_PhoneRepairShop {}

4. In the ItemSettings_PhoneRepairShop class, specify the properties for the UsrRepairItem and
UsrRepairItemType fields of the data view, as shown below. You use the name of the data field as the
property name.

export class ItemSettings_PhoneRepairShop {
**UsrRepairItem: PXFieldState<PXFieldOptions.CommitChanges>;**
**UsrRepairItemType: PXFieldState;**
}

For the UsrRepairItem field, changes should be committed to the server; therefore, you have used the
PXFieldOptions.CommitChanges option for the property type.
5. Save your changes.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 144

Step 4: Adjusting the Layout in HTML
You need to add two elements aer the Item Type box in the Item Defaults section of the General tab of the Stock
Items (IN202500) form. Do the following to adjust the layout in HTML:

1. Review the IN202500.html file in the FrontendSources\screen\src\screens\IN\IN202500
folder once again. You can see that the ItemType field is located in the fieldset with the fsItemDefaults-
General ID, as shown in the following screenshot.

**Figure: The ID of the fieldset**

2. In the IN202500_PhoneRepairShop.html file, which you have created earlier in this activity, add the
following code.

<template>
<field
after="#fsItemDefaults-General [name='ItemType']"
name="UsrRepairItem"
></field>
<field
after="#fsItemDefaults-General [name='UsrRepairItem']"
name="UsrRepairItemType"
></field>
</template>

You have inserted the UsrRepairItem field aer the ItemType field of the fsItemDefaults-
General fieldset, and the UsrRepairItemType field aer the UsrRepairItem field.
3. Save your changes.

Step 5: Building the Source Code
Build the source code of the Modern UI of the Stock Items (IN202500) form, including the customization code, by
executing the following command in the FrontendSources\screen folder.

npm run build-dev --- --env customFolder=development screenIds=IN202500

Step 6: Testing the Changes
To test your changes, do the following:
1. Open the Stock Items (IN202500) form in the Modern UI.
2. Select the BAT3310EX item.
3. Make sure that the Repair Item Type box in the Item Defaults section of the General tab is available
because the Repair Item check box is selected.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 145

4. Clear the Repair Item check box and make sure that the Repair Item Type box becomes unavailable for
editing. This functionality is implemented in the custom event handler in the backend code.
5. Do not save your changes.

#### Activity 6.1.2: To Add a Tab to an Acumatica ERP Form

This activity will walk you through the process of adding a tab to an existing Acumatica ERP form.

Story
Suppose that you need to add the new Compatible Devices tab to the Stock Items (IN202500) form, as shown in the
following screenshot. The tab will contain a table with the compatible serviceable devices for the selected repair
item. On this tab, managers of the Smart Fix company will list the devices that can be serviced by using the item.
You will place the new tab to the right of the Price/Cost tab. This tab will appear on the form only if the Repair Item
check box is selected on the General tab.

**Figure: A new tab on the Stock Items form**

You have already implemented the backend of the form in the PhoneRepairShop customization project, which
includes the following additions:
• The RSSVStockItemDevice data access class (DAC)
• The CompatibleDevices data view in the InventoryItemMaint_Extension graph extension
• The RowSelected<InventoryItem> event handler in the InventoryItemMaint_Extension
graph extension
You have also already added the RSSVStockItemDevice database table to the application database.

Process Overview
You will modify the extension of the Modern UI of the Stock Items (IN202500) form in TypeScript and HTML, build
the source code of the UI of the form, and test the changes.

Step 1: Modifying the Screen Extension in TypeScript
To add a new tab to the Stock Items (IN202500) form in TypeScript, you need to modify the screen extension that
you have added in Activity 6.1.1: To Add Elements to an Acumatica ERP Form. Do the following:
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 146

1. In the IN202500_PhoneRepairShop.ts file in the FrontendSources\screen\src
\development\screens\IN\IN202500\extensions folder, add createCollection to the list
of directives imported from client-controls.
2. In the IN202500_PhoneRepairShop class, define the property for the data view that corresponds to
the table on the tab by using the following code. Because the data view is used to display a table, you need
to initialize the property with the createCollection method. The input parameter of this method is an
instance of the view class that you will define in the next step.

export class IN202500_PhoneRepairShop {
CompatibleDevices = createCollection(RSSVStockItemDevice);
}

Step 2: Defining the View Class for the Tab in TypeScript
You need to define the view class for the Compatible Devices tab of the Stock Items (IN202500) form in TypeScript.
**Proceed as follows:**
1. In the IN202500_PhoneRepairShop.ts file, add PXView, gridConfig, and GridPreset to the
list of directives imported from client-controls.
2. Define the RSSVStockItemDevice view class as follows.

export class RSSVStockItemDevice extends PXView {
**DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;**
DeviceID_description: PXFieldState;
}

For the DeviceID field, changes should be committed to the server; therefore, you have used the
PXFieldOptions.CommitChanges option for the property type. The DeviceID_description
field is the description that is available through the selector control of the DeviceID field.
3. Add the gridConfig decorator to the view class, as the following code shows. In the gridConfig
decorator, you must specify the preset property. Because you need an editable table on the tab, you use
the Details preset.

@gridConfig({
preset: GridPreset.Details
})
export class RSSVStockItemDevice extends PXView {
**DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;**
DeviceID_description: PXFieldState;
}

4. Save your changes.

Step 3: Adjusting the Layout in HTML
You need to add the Compatible Devices tab aer the Price/Cost tab on the Stock Items (IN202500) form. Do the
following to adjust the layout in HTML:
1. Review the IN202500.html file in the FrontendSources\screen\src\screens\IN\IN202500
folder, and locate the qp-tab tag inside the qp-tabbar tag that defines the Price/Cost tab. The qp-tab
tag has the tab-PriceCost ID.
2. In the IN202500_PhoneRepairShop.html file, add the following code aer the second field tag.

<qp-tab
after="#tab-PriceCost"
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 147

id="tab-CompatibleDevices"
caption="Compatible Devices"
>
<qp-grid id="grid-CompatibleDevices" view.bind="CompatibleDevices">
</qp-grid>
</qp-tab>

You have inserted the tab aer the last tab of the tab bar.
3. Save your changes.

Step 4: Building the Source Code
Build the source code of the Modern UI of the Stock Items (IN202500) form, including the customization code, by
executing the following command in the FrontendSources\screen folder.

npm run build-dev --- --env customFolder=development screenIds=IN202500

Step 5: Testing the Changes
To test your changes, open the Stock Items (IN202500) form in the Modern UI and do the following:
1. Select the BAT3310EX item and make sure that the Compatible Devices tab is displayed for it. (The tab is
displayed because the Repair Item check box is selected on the General tab.)
2. On the Compatible Devices tab, add the Nokia3310 device to the list.
3. Save your changes.
4. In the Summary area, select the HEADSET item and make sure that the Compatible Devices tab is not
displayed for this item. (This is because the Repair Item check box is cleared.)

**UI Adjustments in HTML and TypeScript: HTML Examples**

You may need to add, remove, or replace particular UI elements to adjust the UI definition of the form. In this topic,
you can find examples of layout adjustment in HTML.

All tags that customize the original HTML code of an Acumatica ERP form must be located on the
highest level of the extension layout—that is, in the template tag of the highest level.

Adding Fields to the End of a Fieldset
Suppose that you need to add fields to a fieldset that is already defined on the form. The following code adds two
fields to the fieldset that has id="main". These fields will be added to the end of the fieldset, as shown in the
following screenshot.

**Figure: Two boxes at the end of the fieldset**
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 148

<template>
<template modify="#main">
<field name="SiteID"></field>
<field name="InventoryID"></field>
</template>
</template>

Modifying Fields in a Fieldset
To add a field in the middle of a fieldset, in one of the following attributes of the field tag, you use a CSS selector
that specifies the field relative to which you need to place the new elements:
• before: Places the element before the element referenced in this attribute.
• after: Places the element aer the element referenced in this attribute.
• append: Places the element aer all child elements of the element referenced in this attribute.
• prepend: Places the element before all child elements of the element referenced in this attribute.
• modify: Modifies the attribute values of the element referenced in this attribute.
• remove: Removes the element referenced in this attribute.
• replace: Replaces the element referenced in this attribute.
In the following example, the FieldName field is inserted aer the OriginalFieldName field of the
secondary fieldset.

<template>
<field name="FieldName" after="#secondary [name='OriginalFieldName']">
</field>
</template>

Reordering Fieldsets
To modify the order of fieldsets on an Acumatica ERP form, you add the qp-fieldset tag, specify the ID of
the fieldset that you want to move in the modify attribute, and specify the new location by using the after or
before attributes. In the following example, the fieldset with the SomeID ID is placed aer the fieldset with the
AnotherID ID.

<template>
<qp-fieldset modify="#SomeID" after="#AnotherID"></qp-fieldset>
</template>

Modifying the Layout Based on a Condition
To modify the layout only if a condition is fulfilled, you add the if.bind attribute to the container tag that
you want to modify. In the following code, the tab is added only if the check box that corresponds to the
ShowPutAway field is selected.

<template>
<qp-tab after="#tabPutAway" id="tabTransfers" caption="Transfers"
if.bind="HeaderView.ShowPutAway.value == true">
<qp-grid id="formTransfers" view.bind="RelatedTransfers">
</qp-grid>
</qp-tab>
</template>
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 149

**UI Adjustments in HTML and TypeScript: TypeScript Examples**

You may need to adjust the TypeScript code of the form, such as to add or replace a decorator. In this topic, you can
find examples of TypeScript adjustment.
Suppose that the following view class is defined for an Acumatica ERP form. It can be defined directly in the code of
the form or in a reusable UI definition. The following sections describe how to adjust this definition in an extension.

@featureInstalled("PX.Objects.CS.FeaturesSet+CommerceIntegration")
export class Address extends PXView {
**OverrideAddress: PXFieldState<PXFieldOptions.CommitChanges>;**
AddressLine1: PXFieldState<PXFieldOptions.CommitChanges>;
AddressLine2: PXFieldState<PXFieldOptions.CommitChanges>;
**City: PXFieldState<PXFieldOptions.CommitChanges>;**
**State: PXFieldState<PXFieldOptions.CommitChanges>;**
**PostalCode: PXFieldState<PXFieldOptions.CommitChanges>;**
**CountryID: PXFieldState<PXFieldOptions.CommitChanges>;**
**Latitude: PXFieldState;**
**Longitude: PXFieldState;**
}

Adding Fields
Suppose that for a particular form you need to add a field to a view class in an extension. You create an extension of
the view class and add the needed field.
The following code extends the Address view class, whose definition is shown in the previous section, and adds
the AddressLine3 field to it.

export interface RS203040_AddressWithLine3 extends Address { }
export class RS203040_AddressWithLine3 {
AddressLine3: PXFieldState<PXFieldOptions.CommitChanges>;
}

Adding Field Options
Suppose that for a particular form, you need to specify PXFieldOptions.CommitChanges for a field that is
already defined in a view class of the form. You create an extension of the view class and add this field with the
PXFieldOptions.CommitChanges option.
The following code extends the Address view class, whose definition is available in the beginning of this topic,
and specifies the PXFieldOptions.CommitChanges option for the Latitude field.

export interface RS203040_AddressWithLatitudeCommitChanges extends Address { }
export class RS203040_AddressWithLatitudeCommitChanges {
**Latitude: PXFieldState<PXFieldOptions.CommitChanges>;**
}

Replacing Field Options
Suppose that for a particular form, you need to remove PXFieldOptions.CommitChanges for a field that
is already defined in a view class of the form. You create an extension of the view class and add this field with the
fieldOptions decorator assigned.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 150

The following code extends the Address view class, whose definition is available in the beginning of this topic,
and removes the PXFieldOptions.CommitChanges option for the AddressLine2 field.

export interface RS203040_AddressWithoutCommit extends Address { }
export class RS203040_AddressWithoutCommit {
@fieldOptions({ commitChanges: false })
AddressLine2: PXFieldState<PXFieldOptions.CommitChanges>;
}

Adding a Decorator for a Field or Class
Suppose that for a particular form, you need to add a decorator for a field of a view class or for the view class itself.
To add a decorator for a view class, you create an extension for the view class and add the needed decorator for it.
To add a decorator for a field of the class, you extend the view class and add this field with the needed decorator.
If a decorator is specified for the field or class and the added decorator is the same as the original one, the options
specified in the added decorator completely override the options specified in the original decorator.
The following code extends the Address view class and specifies the controlConfig decorator for the State
field.

export interface RS203040_AddressWithEditableState extends Address { }
export class RS203040_AddressWithEditableState {
@controlConfig({allowEdit: true, })
**State: PXFieldState<PXFieldOptions.CommitChanges>;**
}

Removing a Decorator from a Field or Class
Suppose that for a particular form, you need to remove the decorator from a field of a view class or from the view
class itself. To remove the decorator from a view class, you extend the view class and add the removeDecorator
decorator for it. To remove the decorator from a field of the class, you extend the view class and add this field with
the removeDecorator decorator.
The following code extends the Address view class and removes the featureInstalled decorator from the
class.

export interface RS203040_AddressNoFeature extends Address { }
@removeDecorator("featureInstalled")
export class RS203040_AddressNoFeature {
}

Modifying a Decorator for a Field or Class
Suppose that for a particular form, you need to modify the options specified in the decorator for a field of a view
class or for the view class itself. To modify a decorator for a view class, you extend the view class and add the
updateDecorator decorator for it. To modify a decorator for a field of the class, you extend the view class and
add this field with the updateDecorator decorator.
The options specified in the updateDecorator decorator are merged with the original options of the decorator.
**The following rules are used during the merge:**
• If the original decorator specifies an option and this option is overridden in the updateDecorator
decorator, the new value is used.
• If the original decorator specifies an option and this option is not defined in the updateDecorator
decorator, the value from the original decorator is used.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 151

The following code extends the WorkbenchTreeNode view class and updates the options specified in the
treeConfig decorator of the class.

export interface RS203040_WorkbenchTreeNodeExt extends WorkbenchTreeNode { }
@updateDecorator("treeConfig", "topBarItems", {
**Rename: {**
config: {
commandName: "rename",
id: "rename",
text: "Rename",
images: { normal: "control@EditN" }
}
},
})
export class RS203040_WorkbenchTreeNodeExt {
}

### Lesson 6.2: Including the Modern UI Changes in a Customization Project

In this lesson, you will learn how to include changes to the Modern UI in a customization project.

**Customization Project with UI Changes: General Information**

To distribute the changes that you have made to the Modern UI of your Acumatica ERP instance to other Acumatica
ERP instances, you need to include your Modern UI files in a customization project. You include the Modern UI files
in a customization project on the Modern UI Files page of the Customization Project Editor.

The Modern UI changes are applied to each tenant of an Acumatica ERP instance independently.

Applicable Scenarios
You include changes to the Modern UI in a customization project if you need to use these changes in another
Acumatica ERP instance.

Preparation for Publishing a Customization Project with Modern UI Files
For the publication of customization projects with Modern UI files, Node.js must be installed, including the node
version manager (nvm) and node package manager (npm). The Acumatica ERP Configuration wizard installs the
needed version of Node.js if the Install NodeJS check box is selected on the Website Configuration page of the
wizard.

If you want to use the version of Node.js that has already been installed in your system, you can
clear the Install NodeJS check box and add the following key to the appSettings section of
the Web.config file of your instance: <add key="NodeJs:NodeJsPath" value="C:
\Program Files\NodeJs"/>, where value specifies the path to the location where Node.js
has been installed.

Before publishing a customization project with Modern UI files, you may need to further configure the Acumatica
ERP instance by specifying the following keys in the Web.config file of the instance:
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 152

• NodeJs:NpmCachePath: Specifies the path for the npm cache, such as C:\instances\site
\App_Data\npm-cache. This key is mostly intended for production use.
• NodeJs:NodeTempPath: Specifies the path to the Node.js temporary folder. You might need to use this
parameter if a customization project was designed for one version of Node.js and you are upgrading to
Acumatica ERP with a diﬀerent version Node.js. You can specify the path similar to the one specified in the
existing web.config file. An example of the path is shown below.
<add key="NodeJs:NoteTempPath" value="C:\Acumatica\NodeTemp\PhoneRepairShop" />

The parameter is specified in the web.config file by default.

• NodeJs:DevBuild: If the value is true, turns on developer mode, which will be used while the
Customization Project Editor compiles the UI sources.
• NodeJs:CompileAllScreens: If the value is false, during publication of customization projects,
compiles the Modern UI source code only for the Acumatica ERP forms whose Modern UI source code has
been modified. If the value is true, all Modern UI source code is compiled.

Adding of Files to a Customization Project
To add Modern UI files to a customization project, you click the Modern UI Files node in the navigation pane of the
Customization Project Editor. On the toolbar of the Modern UI Files page, you click Add New Record; in the dialog
box that opens, you select the Modern UI files that you need to include in the customization project.

Publishing of a Customization Project with Modern UI Files
You can publish a customization project with Modern UI files in the same way as you publish any other
customization project. For details about how to publish a customization project, see Publishing Customization
Projects.

If any error related to the Modern UI files occurs during the publication of the customization project, you can find
the log of the compilation of the Modern UI files in the App_Data\logs folder of the instance.

**Customization Project with UI Changes: How UI Customization Works**

In this topic, you can find information about how a customization project that includes changes to the Modern UI
works, including how it is published and unpublished.

Where the Source Code of the Modern UI Changes Is Stored
When a customization project that contains changes to the Modern UI is published for a particular tenant, the
system copies the UI changes included in the customization project to the FrontendSources\screen
\src\customizationScreens\<Tenant Name> folder. The FrontendSources\screen\src
\customizationScreens folder contains a subfolder for each tenant for which a customization project with
the Modern UI changes is published.
Each tenant folder contains the Screens folder. The customizationScreens\<Tenant Name>\screens
folder contains subfolders with two-letter names. For each new form or each form whose UI has been customized,
the subfolder contains a folder with the screen ID as the folder name, such as S0301000. This folder includes the
following files and folders:
• For a form whose UI has been customized, the extensions folder, which contains TypeScript and HTML
files for customization-related extensions of the form.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 153

• For a new form, the HTML and TypeScript files that have the screen ID as the file name, such as
SO304000.ts and SO304000.html. The views.ts file and the Extensions folder may also be
included for the form.
Below you can see an example of the hierarchy of the files and folders with code that customizes the Modern UI.

Site
- FrontendSources\screen\src\customizationScreens
- - Tenant 1
- - - screens
- - - - SO
- - - - - S0301000
- - - - - - extensions
- - - - - - - SO301000_customization1.html
- - - - - - - SO301000_customization1.ts
- - - - - SO304000
- - - - - - extensions (optional)
- - - - - - - SO304000_extension1.html
- - - - - - - SO304000_extension1.ts
- - - - - - SO304000.html
- - - - - - SO304000.ts
- - - - - - views.ts (optional)

Where the Compiled UI Changes Are Saved
When a customization project that contains changes to the Modern UI is published for a particular tenant,
the system takes the original UI sources and the UI sources from the FrontendSources\screen\src
\customizationScreens\<Tenant Name> folder, compiles them, and produces the files that include all UI
changes from the customization project in the Scripts\Screens\<Tenant Name> folder. The files outside of
the Scripts\Screens\<Tenant Name> folder remain unchanged.
Below you can see an example of the hierarchy of the files and folders with the compiled sources of the Modern UI.

Site
- Scripts
- - Screens
- - - Tenant 1
- - - - S0301000.hashcode.bundle.js
- - - - S0301000.html
- - - - S0304000.hashcode.bundle.js
- - - - S0304000.html
- - - S0301000.hashcode.bundle.js
- - - S0301000.html

How the UI Customization Project is Unpublished
When a customization project that contains changes to the Modern UI is unpublished for a particular tenant,
the system removes the Scripts\Screens\<Tenant Name> and customizationScreens\<Tenant
Name> folders.

How a Form Works Aer the Publication of a UI Customization Project
When a user opens an Acumatica ERP form, the system first looks for the sources of the form in the Scripts
\Screens\<Tenant Name> folder. If the files for this form exist in this folder, the system displays the form with
the UI changes from the customization project. If no files for this form are found in the folder, the system displays
the original form.
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 154

#### Activity 6.2.1: To Include Source Files in a Customization Project

This activity will walk you through the process of including source files in a customization project.

Story
Suppose that for the Smart Fix company, you’ve migrated multiple forms to the Modern UI and added elements to
the Modern UI of particular forms. You need to include all these changes in a customization project to be able to
distribute the changes to the Acumatica ERP instances that are used by employees of the Smart Fix company.

Process Overview
You will rebuild all your changes and include them in a customization project.

Step 1: Building the Source Files
To build the source code of the Modern UI for the current tenant, including the customization code, execute the
following command in the FrontendSources folder.

npm run build-dev

Step 2: Including Files in the Customization Project
You use the development folder located in the FrontendSources\screen\src\ folder of your instance to
customize forms for the Modern UI. Once you've finished, you can include these files in your customization project.
To redistribute the Modern UI customization files, you need to include them in the customization project with the
backend customization code as follows:
1. On the Customization Projects (SM204505) form, click the PhoneRepairShop project name to open this
customization project.
2. On the navigation pane of the Customization Project Editor, which opens, click Modern UI Files. The
Modern UI Files page opens.
3. On the page toolbar, click Add New Record.
4. In the Add Files dialog box, select the Selected check box in the rows with the following files:
• development\screens\IN\IN202500\extensions\IN202500_PhoneRepairShop.html
• development\screens\IN\IN202500\extensions\IN202500_PhoneRepairShop.ts
• development\screens\RS\RS101000\RS101000.html
• development\screens\RS\RS101000\RS101000.ts
• development\screens\RS\RS201000\RS201000.html
• development\screens\RS\RS201000\RS201000.ts
• development\screens\RS\RS202000\RS202000.html
• development\screens\RS\RS202000\RS202000.ts
• development\screens\RS\RS301000\RS301000.html
• development\screens\RS\RS301000\RS301000.ts
• development\screens\RS\RS501000\RS501000.html
• development\screens\RS\RS501000\RS501000.ts
## Part 6: Customizing Acumatica ERP Forms in HTML and TypeScript | 155

5. Click Save.

**Customization Project with UI Changes: Custom Plugin**

You can implement a custom plugin by using TypeScript and HTML. To apply it to your instance, the plugin files
need to be included into your customization and published to the instance. To include the plugin files into the
customization project, do the following:
1. Create a folder for your plugin files in the development folder of your instance. An example for the
giftCard folder is shown below.

FrontendSources/screen/src/development/plugins/giftCard

2. Put the plugin files in the new folder.
3. In the Customization Project Editor, open the Modern UI Files page.
4. On the Modern UI Files page, add the files from the new folder.
## Part 7: Using Advanced Techniques | 156

## Part 7: Using Advanced Techniques
You can reuse the UI definition of an Acumatica ERP form or a part of a form in other Acumatica ERP forms. This
approach may be useful if you need to create a form that is almost identical to another Acumatica ERP form or you
need to reuse a part of a form in multiple forms. In this part, you will learn how to reuse UI definitions in HTML and
TypeScript.
The Acumatica ERP user interface implements handling of many runtime events. You may need to define custom
handlers of UI events that the built-in system behavior does not cover. This part also describes some of these
scenarios and the ways you can manage them.

### Lesson 7.1: Reusing UI Definitions

In this lesson, you will learn how to do the following:
• Reuse the whole UI definition of an Acumatica ERP form for another form
• Create and use a reusable UI definition with and without parameters
• Add, remove, or replace parts of form layout in a reusable UI definition
• Adjust TypeScript code of a reusable UI definition

**Reusing of UI Definitions: General Information**

**You can reuse a UI definition as follows:**
• To reuse a TypeScript declaration, extend a screen class or a class that derives from a screen class.
• To reuse an HTML declaration or any part of it, use one of the following approaches:
• Add the qp-include tag
• Specify the template tag’s id in the ref attribute of the control tag

Applicable Scenarios
**You reuse a UI definition when:**
• You need to implement two almost-identical Acumatica ERP forms, such as Site Map (SM200520) and Portal
Map (SM200521). You reuse the UI definition of one form to define the other.
• You need to implement multiple Acumatica ERP forms with similar UI definitions, such as Scan and Receive
(IN301020), Scan and Issue (IN302020), Scan and Transfer (IN304020), and Scan and Count (IN305020). You
implement a reusable UI definition for the common parts and adjust the definition for each form.
• You need to use identical form components—such as sections, tabs, or dialog boxes—on multiple forms.

Use of the Whole UI Definition of a Form
A reusable UI definition that defines an entire form usually contains the declaration of a screen class and view
classes. So to use it for your form, you need your screen class to extend the screen class of the UI definition. This
way, your screen class will have the same logic and properties (that is, views) as the original screen class. To reuse
the HTML template of the UI definition, you need to add the qp-include tag with the reference to the original
HTML template.
To reuse the UI definition of a form, you need to reuse both the TypeScript declaration and the HTML code:
## Part 7: Using Advanced Techniques | 157

1. In the TypeScript file, import the classes from the reusable UI definition, as shown below. You need to
import the screen class and all view classes you plan to use.

import { SM200520 } from "../SM200520/SM200520";

2. In the TypeScript file, extend the screen class defined in the reusable definition. You must specify the graph
type and primary view of the new form in the graphInfo decorator.

In the Modern UI, each Acumatica ERP form must use its own graph type.

For example, suppose that a reusable definition declares the SM200520 screen class, and you need
to reuse it for the screen with SM200521 screen ID. You define the SM200521 class that extends the
SM200520 class and specify the graph for the SM200521 screen, as shown below.

@graphInfo({
graphType: "PX.SiteMap.Graph.PortalMapMaint"
primaryView: "SiteMap",
})
export class SM200521 extends SM200520 {}

You can use an exact copy of the original class or modify the UI definition inherited from the base class.
3. In the HTML file for the new form, insert the HTML code of the existing form by using the qp-include tag,
as shown below.

<template>
<qp-include url="../SM200520/SM200520.html">
</template>

Use of a Reusable UI Definition That Defines Part of a Form
A reusable UI definition that defines a part of a form usually contains declarations of view classes but no screen
class. So to use the definition for your form, you need to import the view classes from the reusable definition and
initialize the views in your screen class. To reuse the HTML template of the UI definition, you need to add the qp-
include tag with a reference to the original HTML template.
To use a reusable UI definition that defines part of a form, such as a fieldset:
1. In the TypeScript file of the form where you need to insert the reusable UI definition, import the view
classes, as shown below.

import { Address } from "src/screens/common/form-address/form-address";

2. In your form’s screen class, initialize the views and specify the imported view classes as parameters.

@graphInfo({ ... })
export class AM310000 extends PXScreen {
@viewInfo({ containerName: "Ship-To Address" })
ShippingAddress = createSingle(Address);
}

3. In the HTML code of the form where you need to insert the reusable UI definition, add the qp-include tag
with a reference to this reusable UI definition. If the reusable definition has parameters, provide their values
in the qp-include tag.
## Part 7: Using Advanced Techniques | 158

You can determine whether the reusable definition has parameters by reviewing the qp-
include-parameters tag in the HTML template of the reusable definition.

The following example shows the insertion of a reusable UI definition with parameters. (For details, see
**Reusing of UI Definitions: Reusable UI Definitions with Parameters.)**

<div slot="B">
<qp-include
url="src/screens/common/form-address/form-address.html"
fs-id="formB"
address-view="ShippingAddress"
fs-caption="Ship-To Address">
</qp-include>
</div>

Adjusting of the Reusable UI Definition
Aer inserting a reusable UI definition into a form’s UI definition, you may need to add, remove, or replace
particular elements to adjust the definition. Do the following:
1. In the TypeScript code of the form, you define the elements that aren’t included in the reusable UI
definition, as shown below. You can add more view classes, initialize views, or define more logic (for
example, in event handlers).

export class IN202520 extends BarcodeProcessingScreen {
@viewInfo({ containerName: "Scan Header" })
HeaderView = createSingle(ScanHeader);
}

**You can find examples of TypeScript code adjustments in UI Adjustments in HTML and TypeScript: TypeScript**
Examples.
2. Adjust the HTML template as follows:
a. Add the tags you need to modify aer or inside the qp-include tag.
If the reusable UI definition defines a whole form—that is, your screen class extends the screen class
of the UI definition—adjust the layout by adding tags aer the qp-include tag. These tags must be
located in the top-level template tag of the HTML file.
If the reusable UI definition defines a part of a form (for example, a fieldset), adjust the layout by adding
tags inside the qp-include tag.
b. Specify the attribute that indicates the type of modification, which can be one of these:
• before: Places the element before the element referenced in this attribute.
• after: Places the element aer the element referenced in this attribute.
• append: Places the element aer all child elements of the element referenced in this attribute.
• prepend: Places the element before all child elements of the element referenced in this attribute.
• modify: Modifies the attribute values of the element referenced in this attribute.
• remove: Removes the element referenced in this attribute.
• replace: Replaces the element referenced in this attribute.
c. As the value of the attribute, specify a CSS selector that defines the element relative to which you need to
place the new element, such as #main or #secondary [name='OriginalFieldName']".
## Part 7: Using Advanced Techniques | 159

**You can use the following approaches when specifying the CSS selector:**
• Specifying the exact location of the element, such as the fieldset’s ID and the field’s
name.
If no element satisfies the CSS selector, the build process fails. For example, suppose
that you want to place a box right aer the specific box in the specific fieldset of a form.
You specify the exact location of the new field in the CSS selector, including the fieldset’s
ID and the field’s name. If in a future version of Acumatica ERP, the field relative to
which the new field is placed is moved to another fieldset, the build process will fail for
this CSS selector.
• Specifying only the field’s name.
If more than one item satisfies the specified CSS selector, the build process fails. For
example, suppose that you want to place a new box right aer a specific box and it
doesn’t matter where this specific box is located on the form. You specify only the field’s
name in the CSS selector. If the field is moved to another fieldset in a future version of
Acumatica ERP, the build process will be successful for this CSS selector.

Below you can see an example of adjusting a reusable UI definition that defines a whole form. The code
adds three boxes in various fieldsets of the reusable UI definition and adds a tab with a table.

<template>
<qp-include url="../../barcodeProcessing/BarcodeProcessingScreen.html"></qp-
include>

<field append="#fsColumnA-Header" name="RefNbr" config-allow-edit.bind="true"></
field>
<field append="#fsColumnA-Header" name="SiteID"></field>

<field append="#fsColumnB-Header" name="Mode"></field>

<qp-tab id="tabIssue" before="#tab-Logs" caption="Issue">
<qp-grid id="gridPicked" view.bind="transactions"></qp-grid>
</qp-tab>
</template>

The following example shows an adjustment of a reusable UI definition that defines a part of a form. The
code modifies a field in the reusable UI definition.

<qp-include url="src/screens/common/form-address/form-address.html"
fs-id="groupAccountAddress"
address-view="DefAddress"
fs-caption="Account Address"
fs-wg-container="DefAddress_DefAddress"
override-fieldname
view-on-map-action-name="ViewMainOnMap"
>
<field modify="#groupAccountAddress [name='CountryID']"
after="#groupAccountAddress [name='PostalCode']"></field>
</qp-include>

You add as many adjustments as you need. You can find examples of layout adjustments in UI Adjustments in
**HTML and TypeScript: HTML Examples.**
## Part 7: Using Advanced Techniques | 160

Predefined Reusable UI Definitions
**A number of predefined reusable UI definitions are available in the following locations:**
• The FrontendSources/screen/src/screens/common folder of the Acumatica ERP instance
• The common folder for particular functionality, such as the FrontendSources/screen/src/
screens/IN/common folder of the Acumatica ERP instance for inventory functionality
You can use these reusable UI definitions in your UI customization projects.

**Reusing of UI Definitions: Creation of a Reusable UI Definition**

To create a reusable definition. you need to do the following:
1. Define the TypeScript file with view classes and an optional screen class.
2. Define the layout of the reusable component in the HTML template.

The sections below describe how to create reusable definitions depending on whether you’re reusing an entire
screen or only specific components.

Defining the Reusable Definition for a Whole Screen
**To define a reusable UI definition based on a whole screen:**
1. In a TypeScript file, declare an abstract class that extends the PXScreen class, as shown below.

export abstract class BarcodeProcessingScreen extends PXScreen {
}

2. In the abstract class, define the views and logic that will be reused, as the following example shows.

import { ScanInfo, ScanLogs } from "./views";

export abstract class BarcodeProcessingScreen extends PXScreen {
@viewInfo({ containerName: "Scan Information" })
Info = createSingle(ScanInfo);

@viewInfo({ containerName: "Scan Logs" })
Logs = createCollection(ScanLogs);

@handleEvent(CustomEventType.GetRowCss, { view: 'Logs' })
getLogsRowCss(args: RowCssHandlerArgs) {
if (args?.selector?.row?.MessageType.value === 'ERR') {
return 'excessedLine startedLine';
}
else if (args?.selector?.row?.MessageType.value === 'WRN') {
return 'startedLine';
}

return undefined;
}
}

3. If you need to create view classes, define them as usual. The following code shows the definition of the view
class that’s used in the previous code example.
## Part 7: Using Advanced Techniques | 161

export class ScanInfo extends PXView {
**Mode: PXFieldState<PXFieldOptions.Disabled>;**
**Message: PXFieldState<PXFieldOptions.Disabled>;**
**MessageSoundFile: PXFieldState<PXFieldOptions.Disabled>;**
**Instructions: PXFieldState<PXFieldOptions.Disabled>;**
**Prompt: PXFieldState<PXFieldOptions.Disabled>;**
}

For details about inserting the reusable UI definition of a whole screen, see Use of the Whole UI Definition of a Form.

Defining the Reusable Definition for Part of a Screen
To define a reusable definition that contains independent components, such as a dialog box or a fieldset, without a
screen class:
1. Define the view classes for the controls that are displayed in the reusable component.

export class ChangeIDParameters extends PXView {
@fieldConfig({controlType: "qp-mask-editor"})
**CD: PXFieldState;**
}

2. If needed, define an abstract class and initialize the view there.

export abstract class ChangeIDBase {
@viewInfo({ containerName: "Specify New ID" })
ChangeIDDialog = createSingle(ChangeIDParameters);
}

You don’t need to define the abstract class and initialize the view there if you’re providing a
view as a parameter. The fields in the uninitialized view should be identical to the fields in the
actual view provided as a parameter.

For details about inserting the reusable UI definition that defines part of a screen, see Use of a Reusable UI
Definition That Defines Part of a Form.

Defining the HTML Template
In the HTML file with the same name as the TypeScript file, you define the reusable HTML code, as shown in the
following example.

<template>
<require from="screens/barcodeProcessing/styles.css"></require>

<qp-template id="HeaderView_formHeader" name="7-10-7" wg-container
class="equal-height">
<qp-fieldset id="main" view.bind="HeaderView" slot="A">
<field name="Barcode"
control-type="qp-barcode-input"
config-sound-control.bind="Info.MessageSoundFile"
config-submit-command.bind="Scan">
</field>
</qp-fieldset>

<qp-fieldset id="secondary" view.bind="HeaderView" slot="B"
## Part 7: Using Advanced Techniques | 162

class="no-label">
<field name="Message" config-rows.bind="3"></field>
</qp-fieldset>

<qp-fieldset id="third" view.bind="HeaderView" slot="C"
class="no-label">
<field name="ProcessingSucceeded"
config.bind="{
label: '',
renderStyle: 'button',
checkImages: { normal: 'main@Success' },
uncheckImages: { normal: 'main@Fail' }
}"
config-class.bind="'ProcessingStatusIcon'">
</field>
</qp-fieldset>

<qp-fieldset id="info" view.bind="Info">
<field name="MessageSoundFile" show.bind="false"></field>
</qp-fieldset>
</qp-template>

<qp-tabbar id="mainTab">
<qp-tab id="tabLogs" caption="Scan Log">
<qp-grid id="gridLogs" wg-container="grid4" view.bind="Logs">
</qp-grid>
</qp-tab>
</qp-tabbar>
</template>

This code implements a Summary area with three columns and a tab bar with a single tab.
You can also define the layout in the nested template tag and later insert it by using the ref attribute—for
example, inside a qp-tab tag. In this case, don’t use hyphens in the template ID because TypeScript will try to
parse it as a mathematical statement. Instead of a hyphen, use an underscore, as shown in the following code.

<template>
<template id="content_Approval">
...
</template>
<template>

Then you can reuse the template, as shown below.

<qp-tab id="tab-Approval" caption="Approval" ref="content_Approval"></qp-tab>

You can define the parameters of a reusable UI definition in the qp-include-parameters tag. For
details, see Reusing of UI Definitions: Reusable UI Definitions with Parameters.

For details about adjusting the HTML template of a reusable UI definition, see Adjusting of the Reusable UI
Definition.
## Part 7: Using Advanced Techniques | 163

**Reusing of UI Definitions: Reusable UI Definitions with Parameters**

You can define a reusable UI definition with string parameters—such as a view value—and then provide values for
these parameters when you’re referencing the reusable UI definition.

Defining a Reusable UI Definition with Parameters
To define parameters in a reusable UI definition, you add the parameter names in the qp-include-
parameters tag, which is inside the template tag of the reusable UI definition. You define parameters by using
the mustache.js library. For details about the library, see https://github.com/janl/mustache.js.
You can define a parameter as being required by using the required modifier for the parameter. When this
modifier is specified, a developer must provide a value for the parameter while inserting the reusable UI definition
in a particular form. You can also specify the default value for a parameter.
To reference the parameters in the tags of the reusable UI definition, you specify the parameter name in double
braces, for example, "{{id}}".
The following code shows an example of the Address section. In this example, the id, address-view, and wg-
container parameters are required. The caption parameter has Address as the default value.

<template>
<qp-include-parameters
id.required
address-view.required
caption="Address">
</qp-include-parameters>

<qp-fieldset
id="{{id}}"
view.bind="{{address-view}}"
caption="{{caption}}"
>
<field name="FakeField" unbound>
<qp-address-lookup class="col-12" view.bind="{{address-view}}">
</qp-address-lookup>
</field>
...
</qp-fieldset>
</template>

Inserting a Reusable UI Definition with Parameters
When you want to insert a reusable UI definition, you specify the parameters in the qp-include tag.
The following example shows the insertion of the reusable UI definition for the Address section, whose HTML code
is defined in the previous section.

<div slot="B">
<qp-include url="../../../common/forms/form-address/form-address.html"
id="formA"
address-view="AddressCurrent"
></qp-include>
</div>
## Part 7: Using Advanced Techniques | 164

### Lesson 7.2: Handling UI Events

In this lesson, you will learn how to do the following:
• Handle UI events
• Work with data that is unrelated to any view by using event handling

**UI Events: General Information**

In the Modern UI of , you can handle UI events in frontend code to make runtime changes that aren’t possible
through built-in UI functionality.

Applicable Scenarios
**You handle UI events if you need to implement scenarios such as these:**
• Changing the CSS of a specific table cell or row on an Acumatica ERP form
• Passing to an Acumatica ERP form data that is unrelated to any data view
• Passing large binary data to an Acumatica ERP form
• Calculating a value that is only displayed on an Acumatica ERP form and is not saved to the database

Event Handling
You use the handleEvent decorator to mark a handler of a runtime event for a specified view or field or with no
relation to a view or field.
Only event types defined in the CustomEventType enum are supported.

Be aware that any logic that you implement using event handlers is available only in Modern UI forms
and is not accessible through the web services or the mapping of the mobile application.

Sample Event Handlers
The Acumatica ERP source code provides sample event handlers for the Sales Orders (SO301000) form. You can find
these in the following location of any instance.

\FrontendSources\screen\src\screens\SO\SO301000\extensions\SO301000_FieldsSample.ts

To test the event handlers, do the following:
1. Uncomment the SO301000_FieldsSample interface.
2. Uncomment the SO301000_FieldsSample class.
3. Rebuild the code.
## Part 7: Using Advanced Techniques | 165

**UI Events: Changing of the CSS for a Row of a Table**

You can implement an event handler for the GetRowCss event to change the CSS for any row of a table, such as
the bold rows shown in the following screenshot.

**Figure: Bold rows in the table**

In the PO302020 screen class in the following example, the Received view property is defined for the
ReceiptSplitsForReceive view class. In this screen class, the getReceivedRowCss method has
the handleEvent decorator, which indicates that the method handles the GetRowCss event for the
Received view instance. The handler method returns the name of the CSS class to be applied to the row of the
ReceiptSplitsForReceive type.

export class PO302020 extends PXScreen {
Received = createCollection(ReceiptSplitsForReceive);
@handleEvent(CustomEventType.GetRowCss, { view: "Received" })
getReceivedRowCss(args: RowCssHandlerArgs<ReceiptSplitsForReceive>): string {
const split = args?.selector?.row;
if (split != null && split.ReceiveQty.value > split.Qty.value) {
return "bold-row";
}
return undefined;
}
}

You can use the CSS classes that are defined in the FrontendSources\screen\static
\custom.css file or define your own classes in a CSS file for the form, such as in EP503010.css.

**UI Events: Debugging the UI Code in the Browser**

You can debug any typescript code including the code of event handlers using developer tools.
On the Sources tab of the developer tools, you can find the source code of the form. You can use this code for
debugging.

For easier debugging, you need to build the UI source code with the build-dev command. For
details about building the source code, see Modern UI Development: Building the Source Code.

You can set a breakpoint in the code. When the breakpoint is hit, in the Console tab in the bottom of the Sources
tab, you can type the name of the variable to see its value, type an expression to evaluate it, or change the value of
a variable to check how the system works with this value.
## Part 7: Using Advanced Techniques | 166

For example, in the following screenshot, the TypeScript code of the Bills and Adjustments (AP301000) form is open,
as Item 1 shows. A breakpoint is set for a line in the code (Item 2). This breakpoint is hit during the opening of
the form, and the debugger has stopped on this line. In the console, the row.DeferredCode name has been
entered, and the value of this variable is displayed (Item 3).

**Figure: Debugging in the web browser**

#### Activity 7.2.1: To Implement and Debug an Event Handler

This activity will walk you through implementing an event handler and debugging it by using the developer tools in
the browser.

Story
Suppose that you need to highlight the second line in a table in bold and print the message in the Console window
when it happens.

Process Overview
You will implement the GetRowCss event handler, in which you will print the message in the Console window and
return the new CSS class for the second line of a table. Then you will debug the code by using the developer tools.

Step 1: Implementing the Event Handler
To implement the GetRowCss event handler, do the following:
1. In the RS301000 screen class of the RS301000.ts file, add the following event handler.

export class RS301000 extends PXScreen {
...

@handleEvent(CustomEventType.GetRowCss, { view: "RepairItems" })
getTransactionsRowCss(args: RowCssHandlerArgs) {
}
}
## Part 7: Using Advanced Techniques | 167

You’ve used the handleEvent decorator to indicate that the method is an event handler. As the first
parameter of the decorator, you’ve specified the type of the event, and in the second parameter, you’ve
specified the view for which the event handler is intended.
2. In the body of the event handler, check the number of the row, print the message in the Console window,
and return the name of the CSS class in string format.

@handleEvent(CustomEventType.GetRowCss, { view: "RepairItems" })
getTransactionsRowCss(args: RowCssHandlerArgs) {
if (args?.selector?.rowIndex === 1) {
console.log("Row CSS changed", args);
return "bold-row";
}
return undefined;
}

You can find the names of the available CSS classes in the FrontendSources/screen/
static/custom.css file.

3. Rebuild your code.

Step 2: Test the Event Handler
To test the event handler, do the following:
1. Refresh the Repair Work Orders (RS301000) form.
2. Create a repair work order with the following settings:
• Customer ID: C000000001
• Service: BATTERYREPLACE
• Device: NOKIA3310
3. On the Repair Items tab, two lines appear.
The second line is bold, as shown below.

**Figure: The bold line on the Repair Items tab**

Step 3: Debugging the Code
To debug the code of the event handler, do the following:
1. Open the developer tools of your browser.
2. Open the Sources tab (Item 1 below).
## Part 7: Using Advanced Techniques | 168

**Figure: The developer tools window**

3. On the Page tab (Item 2 above), locate the source code of the RS301000 screen according to the following
path.

top/main (RS301000.html)/screen/src/development/screens/RS/RS301000/RS301000.ts

You can also search for the needed line on the Search tab (Item 4) of the developer tools.

4. Put the breakpoint on line 24 (Item 3).
5. Open the Repair Work Orders (RS301000) form.
6. Create a new repair work order with the same settings as you entered in the previous step.
7. When the breaking point is hit, go through the steps for the first line and the second line of the table on the
Repair Items tab.
8. On the Console tab, notice the messages from the event handler, as shown below.

**Figure: The Console tab**

**UI Events: Changing of Availability of a UI Element**

To change the availability of a UI element, you can implement an event handler for the RowSelected or
CurrentRowChanged event. If you implement any of these event handlers, the availability of the element is
## Part 7: Using Advanced Techniques | 169

defined on the client side without both communication with the server side and the committing of changes to the
server. Therefore implementation of these event handlers can improve the performance of the application.

Changing Availability of a Button
To change the availability of a button, for example, depending on a row selected in the table or a change in the
table, you need to do the following:
1. Declare the object of the PXActionState type in the screen or view class.

export class SOLine extends PXView {
**POSupplyOK: PXActionState;**
}

2. Implement an event handler for the RowSelected or CurrentRowChanged event.
In the following example, a handler for the RowSelected event is used to define the availability of the
button that corresponds to the POSupplyOK action. The POSupplyOK action is defined in the SOLine
view class, whose instance is available through the Transactions view property.

export class SO301000 extends PXScreen {
Transactions = createCollection(SOLine);
@handleEvent(CustomEventType.CurrentRowChanged, { view: "Transactions" })
onSOLineChanged(args: CurrentRowChangedHandlerArgs<PXViewCollection<SOLine>>) {
const model = (<any>args.viewModel as SOLine);
const ar = args.viewModel.activeRow;

if (model.POSupplyOK) model.POSupplyOK.enabled = !!ar?.POCreate.value;

if (model.ItemAvailability)
model.ItemAvailability.enabled = !!ar?.IsStockItem.value;
if (model.SOOrderLineSplittingExtension_ShowSplits)
model.SOOrderLineSplittingExtension_ShowSplits.enabled = !!ar;
}
}

**UI Events: Handling of Data That Is Unrelated to Any View**

For a callback request that the client sends to the server, the server processes the request, prepares the data to
be returned, and injects this data in the final JSON response that is sent to the client. For this callback processing,
Acumatica ERP provides a number of hooks. You can attach data handlers to these hooks and implement custom
code in these data handlers. The attached handlers can prepare data and execute update operations. When a
request comes to the server, the system calls the hooks one by one. The handlers are executed independently
of one another. You can execute diﬀerent code on diﬀerent hooks to execute a particular code fragment before
another one.
You can use these hooks to pass data that is unrelated to any view of the graph to the TypeScript code of the
Acumatica ERP form that corresponds to the graph. In this case, you should implement the approach that is
described in this topic.

Implementing the Base Abstract Class
The BaseCustomDataHandler<TGraph> and BaseCustomDataHandler<TGraph, TParams> abstract
classes are base classes that provide the methods to obtain data that is not PXView-based from a graph or submit
this data to the graph. These classes are used during client callback.
## Part 7: Using Advanced Techniques | 170

You need to create a data handler class that inherits from one of these base classes. In the data handler class, you
override the method that you want to call to obtain or submit the data. The following code shows an example of
the implementation of the class for the SiteMapMaint graph, which means that this handler is executed only in
the context of the SiteMapMaint graph or its descendents.

using PX.Api.TSBasedScreen.Interfaces;
using PX.SiteMap.Graph;

namespace PX.Api.TSBasedScreen.Objects.Handlers.SM200520;

internal class SM200520Handler : BaseCustomDataHandler<SiteMapMaint>
{
protected override void CollectData(SiteMapMaint graph, dynamic result)
{
result.RefreshSitemap = graph.IsSiteMapAltered;
}
}

In the CollectData method in the code above, the value of the RefreshSitemap parameter has been
specified. The method will be executed when the server collects the data that is sent back to the client during the
callback execution. The specified value of the RefreshSitemap parameter will be passed to the UI, along with
other data returned by the server.
Aer you have defined the class that inherits from the BaseCustomDataHandler<TGraph> or
BaseCustomDataHandler<TGraph, TParams> abstract class, the service for dependency injection is fully
defined. You need to register the service.

Registering the Service
You register the service that you have implemented for dependency injection. For details about dependency
injection, see Dependency Injection: General Information. You use the RegisterCustomDataHandler method to
register the data handler class.
The following code shows an example of this registration.

using Autofac;
using PX.Api.TSBasedScreen.Interfaces;

namespace PX.Api.TSBasedScreen.Objects
{
public class ServiceRegistration : Module
{
protected override void Load(ContainerBuilder builder)
{
builder.RegisterCustomDataHandler<SM200520Handler>();
}
}
}
## Part 7: Using Advanced Techniques | 171

You can specify another name for the data handler during registration, as shown in the following
code. This name will be used in the frontend code.

builder.RegisterCustomDataHandler<RefreshSiteMapHandler<SiteMapMaint>>(
name: "SM200520RefreshSiteMapHandler");

You may need to specify the name for the data handler during registration if you have implemented
a generic data handler with a graph type as the type parameter and you want to register this generic
event handler for multiple graphs.

Changing the TypeScript Code
In the screen class in the TypeScript code of the Acumatica ERP form, you add a method and specify the
customDataHandler decorator for it. As the name of the method you must specify the name that has been used
during the registration of the data handler class for dependency injection in the backend code. By default, this
name is the name of the data handler class.
This method will be executed aer each callback from the server, and it will process the data that is received from
the server. The method is strongly typed. In the following code, the result from the server contains one parameter,
whose value is assigned in the class that implements the base abstract class in the backend code.

@customDataHandler()
SM200520Handler(result: { RefreshSitemap: boolean }) {
if (result.RefreshSitemap) {
refreshMenu();
}
}

### Lesson 7.3: Using View Parameters

Suppose that one control depends on parameters from another control. For example, depending on the value
selected in a tree, a diﬀerent view should be displayed in the table. In this lesson, you will learn how to implement
this scenario.

**UI Definition in HTML and TypeScript: View Parameters in the viewInfo Decorator**

Suppose that one control depends on parameters from another control. For example, depending on the value
selected in a tree (qp-tree), a diﬀerent view should be displayed in the table.
In this case, you can define a list of parameters in the viewInfo decorator for the view of the dependent control
and then specify them in the view delegate in the graph.

Declaring View Parameters
To define parameters in the viewInfo decorator, you do the following:
1. In the viewInfo decorator, specify the parameters property with an array.

{parameters : []}

2. Declare each parameter in the array by creating the ControlParameter object with the following values:
## Part 7: Using Advanced Techniques | 172

a. The name of the parameter (it will be used later in the view delegate)
b. The view where the parameter is located
c. The name of the field in this view that holds the parameter value

The following code shows a declaration of the parent parameter for the Items view. The parent parameter is
defined by the WFStageID field in the Nodes view.

@viewInfo({ parameters: [ new ControlParameter("parent", "Nodes", "WFStageID") ] })
Items = createCollection(FSWFStage);

This functionality was implemented by using the PXControlParam tag in the Classic UI. For
details about conversion to the Modern UI, see Reference for the parameters Property of the viewInfo
Decorator.

Using View Parameters in the View Delegate
To use the view parameter in the graph, you declare the view delegate with the same parameter. Inside the view
delegate, you can use this parameter to return a diﬀerent query. For details on defining data view delegates, see
Filtering Records Dynamically with Data View Delegates.

The following code shows an example of a data view delegate for the Items view. Note that the parent
parameter is declared in the view delegate. This parameter will have the value specified in the TypeScript
declaration.

protected virtual IEnumerable items([PXInt] int? parent)
{
NodeFilter.Current.ParentWFStageID = (parent == null) ? RootNodeID : parent;

PXResultset<FSWFStage> bqlResultSet;

if (parent == null || parent == RootNodeID)
{
bqlResultSet = PXSelect<FSWFStage,
Where<
FSWFStage.wFID, Equal<Current<SelectedNode.wFID>>,
And<FSWFStage.parentWFStageID,
Equal<Current<SelectedNode.parentWFStageID>>>>>
.Select(this);
}
else
{
bqlResultSet = PXSelect<FSWFStage,
Where<
FSWFStage.wFID, Equal<Current<SelectedNode.wFID>>,
And<FSWFStage.parentWFStageID,
Equal<Required<FSWFStage.parentWFStageID>>>>>
.Select(this, parent);
}

return bqlResultSet;
}

As a result, you can display a diﬀerent grid depending on the node selected in the tree. The following screenshot
shows the example implemented in the code above. (In the Workflow Stages tree, the MRO node is selected.)
## Part 7: Using Advanced Techniques | 173

**Figure: A grid displayed depending on a node of a tree**

Reference for the parameters Property of the viewInfo Decorator
The following table shows the correspondence between PXControlParam and the TypeScript elements that are
involved in configuring view parameters.

ASPX                                                    TypeScript

**PXControlParam                                          ControlParameter: The class that is used to create**
an instance of a view parameter.
<px:PXGrid ID="grid" ...>
<Levels>                                              @viewInfo({
<px:PXGridLevel                                       parameters: [
DataMember="Items">                                   new ControlParameter(
...                                                     "parent",
</px:PXGridLevel>                                         "Nodes",
</Levels>                                                   "WFStageID"
<Parameters>                                              )
<px:PXControlParam                                    ]
ControlID="tree"                                  })
Name="parent"                                     Items = createCollection(FSWFStage);
PropertyName="SelectedValue"
Type="String" >
</px:PXControlParam>
</Parameters>
...
</px:PXGrid>

ControlID                                               viewName (the second parameter of the Control-
Parameter constructor): The name of the view on
ControlID="tree"                                       which the current view depends.

Name                                                    name (the first parameter of the ControlParame-
ter constructor): The name of the parameter.
Name="parent"
## Part 7: Using Advanced Techniques | 174

ASPX                           TypeScript

PropertyName                   fieldName( the third parameter of the Control-
Parameter constructor): The name of the field in the
PropertyName="SelectedValue"   view specified in the second parameter.
## Part 8: Testing the Modern UI | 175

## Part 8: Testing the Modern UI
In this part, you can find information on how to maintain the testability of forms that have tests created in the
Classic UI and how to migrate these tests to the Modern UI.

### Lesson 8.1: Testing the Modern UI

In this lesson, you will learn how to do the following:
• Get the structure of WG containers for a form
• Specify the names of WG containers
• Update tests for the Modern UI

**Testing of the Modern UI: General Information**

This topic describes WG containers and the WG containers’ structure that is used in tests for both the Classic UI and
the Modern UI.

Applicable Scenarios
**You need to learn about WG containers when you are doing the following:**
• Testing forms written in the Modern UI by using tests created for the same forms in the Classic UI
• Migrating tests from the Classic UI to the Modern UI

Overview of WG Containers
WG stands for wrapper generator. A wrapper is a part of tests that functions as an interface for tests for the UI of
Acumatica ERP. A wrapper describes the structure of UI elements located in diﬀerent containers on the form—that
is, it describes the structure of containers and elements they include.
A container is an entity that contains fields. Thus, a WG container is a container of fields that are used in tests,
and the wrapper describes the structure of WG containers. Every WG container is referred to by a name. The
name of the WG container is generated automatically unless it is specified explicitly. For a container that has both
the view name and the ID specified, the name of the WG container is generated according to the following rule:
<view_name>_<container_id>.
The following screenshot shows an example of a file that describes WG containers.
## Part 8: Testing the Modern UI | 176

**Figure: The structure of WG containers**

The generation of wrappers does not depend on any features being enabled or disabled.

Access to the Wrapper of a Form
You can access a wrapper of any form that has been switched to the Modern UI by using a URL with the following
format: <instance_URL>/ui/wrappers/<Screen_ID>. For example, you would access a wrapper for
the Invoices and Memos (AR301000) form on the PhoneRepairShop instance with the following URL: http://
localhost/PhoneRepairShop/ui/wrappers/AR301000.
The following code is the beginning of the JSON file that is opened by this URL. The file shows the structure of WG
containers of this form.

{
"containers": [
{
"id": "formDocument",
"name": "Document_form",
"controlType": "QP-TEMPLATE",
"fields": [
{
"id": "edDocument-DocType",
"fieldName": "DocType",
"viewName": "Document",
"displayName": "Type",
"controlType": "qp-drop-down",
"dataType": "String",
"containerPath": [
"formDocument"
],
"supportLocalizationPanel": false
},
{
"id": "edDocument-RefNbr",
"fieldName": "RefNbr",
"viewName": "Document",
"displayName": "Reference Nbr.",
"controlType": "qp-selector",
"dataType": "String",
## Part 8: Testing the Modern UI | 177

"containerPath": [
"formDocument"
],
"supportLocalizationPanel": false
},
...]
}
]
}

Structure of UI Containers
In ASPX files, tags, such as PXFormView and PXGrid, are used to define containers for tests.
In the Modern UI, the fields are organized in containers diﬀerently. For example, in the Classic UI, the form
contained three columns inside one PXFormView, as shown in the following example.

<px:PXFormView ID="form" runat="server" DataSourceID="ds"
Style="z-index: 100" Width="100%"
DataMember="Filter" Caption="Selection" DefaultControlID="edAction">
<Template>
<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
<px:PXDropDown CommitChanges="True" ID="edAction" runat="server"
DataField="Action" />
<px:PXSelector CommitChanges="True" ID="edStatementCycleId" runat="server"
DataField="StatementCycleId" />
<px:PXDateTimeEdit CommitChanges="True" ID="edStatementDate" runat="server"
DataField="StatementDate" />
<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" />
<px:PXSegmentMask CommitChanges="True" ID="edBranchID" runat="server"
DataField="BranchID" />
<px:PXSegmentMask CommitChanges="True" ID="edOrganizationID" runat="server"
DataField="OrganizationID" />
<px:PXCheckBox CommitChanges="True" ID="chkCuryStatements" runat="server"
DataField="CuryStatements" />
<px:PXCheckBox CommitChanges="True" ID="chkShowAll" runat="server"
DataField="ShowAll" />
<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="SM" />
<px:PXCheckBox CommitChanges="True" ID="chkPrintWithDeviceHub" runat="server"
DataField="PrintWithDeviceHub" AlignLeft="true" />
<px:PXCheckBox CommitChanges="True" ID="chkDefinePrinterManually" runat="server"
DataField="DefinePrinterManually" AlignLeft="true" />
<px:PXSelector CommitChanges="True" ID="edPrinterID" runat="server"
DataField="PrinterID" />
<px:PXTextEdit CommitChanges="true" ID="edNumberOfCopies" runat="server"
DataField="NumberOfCopies" />
<px:PXLayoutRule runat="server" StartRow="true" LabelsWidth="SM" ControlSize="XXL" />
<px:PXTextEdit CommitChanges="True" ID="edStatementMessage" runat="server"
DataField="StatementMessage" TextMode="MultiLine" Height="55px" />
</Template>
</px:PXFormView>

In the Modern UI, every such column is a separate qp-fieldset container, as shown in the following code.

<qp-template name="top-17-17-14" id="formFilter" wg-container="Filter_form">
<qp-fieldset slot="A" id="columnFirst" view.bind="Filter">
<field name="Date"></field>
## Part 8: Testing the Modern UI | 178

<field name="VendorID"></field>
<field name="CustomerID"></field>
</qp-fieldset>
<qp-fieldset slot="B" id="columnSecond" view.bind="Filter">
<field name="CreateOnHold"></field>
<field name="CreateInSpecificPeriod"></field>
<field name="FinPeriodID"></field>
</qp-fieldset>
<qp-fieldset slot="C" id="columnThird" view.bind="Filter">
<field name="ProjectID"></field>
<field name="CopyProjectInformation"></field>
</qp-fieldset>
</qp-template>

qp-fieldset is a minimum container entity in the Modern UI. The wrapper generator treats it as a container
entity. As a result, the structure of the containers has changed.
If you do not specify WG containers explicitly, the wrapper will not be generated correctly for a test written for the
**Classic UI to work: Instead of one container with fields of the old PXFormVIew tag, you will get three containers**
with fields (one for each qp-fieldset).

Summary
Current tests rely on the structure of the UI elements on the form of the Classic UI.
In order for these tests to work property, wrappers need to describe the same structure of the form. But in the
Modern UI, the structure of the form is diﬀerent. So for the current tests to work on the Modern UI, you need to
specify the names of WG containers explicitly for most of the tags (except those for which the names are generated
properly). You can specify names of WG containers in several ways. For more information on specifying the names
of WG containers, see Testing of the Modern UI: Names of WG Containers.

**Testing of the Modern UI: Names of WG Containers**

To maintain the testability of the forms in the Modern UI, the names of WG containers must be the same as the
names of respective wrapper containers for the Classic UI. By default, the WG container names for qp-fieldset
and qp-grid are generated automatically so that they are the same as the names in the Classic UI.
**The name of the WG container is generated as follows: <ViewName>_<ElementID>, where ElementID is the**
value of the id attribute specified in the tag.
For complex cases or when the element IDis diﬀerent from the one specified in the Classic UI, you can specify the
name of the WG container manually in HTML by using the following approaches.

The wg-container Attribute Without a Name
To make the WG container name have the <ViewName>_<ElementID> format, you add the wg-container
attribute to a tag that has the view and ID specified and do not specify the name of the WG container.
The wrapper generator will generate a single container for all fields inside this tag, ignoring all containers inside the
tag.
## Part 8: Testing the Modern UI | 179

In the Modern UI, the values of the id attribute are visible to users for personalization, so they should
be easy to understand. Therefore, we recommend that you specify the legacy name of the container in
the wg-container attribute, as described in the following sections, and create a new user-friendly
identifier. For instructions on specifying the id values, see the page corresponding to the UI element
in UI Component Guide.

The wg-container Attribute for the qp-template Tag
To assign the same WG container name to all fields inside all fieldsets of the qp-template tag, you specify
the wg-container attribute in the qp-template tag. The resulting name will have the following format:
<Template_id>, where <Template_id> is the value of the id attribute of the qp-template tag.
The following table shows the usage of the wg-container attribute in the qp-template tag of the HTML
template and the corresponding tag in the ASPX file.

HTML                                                      ASPX

<qp-template
name="7-10-7"                                           <px:PXFormView
id="Document_form"                                        ID="form"
wg-container                                              runat="server"
class="equal-height">                                     DataSourceID="ds"
<qp-fieldset                                              Style="z-index: 100"
slot="A"                                                Width="100%"
id="first"                                              DataMember="Document"
view.bind="Document">                                   Caption="Order Summary"
<field name="OrderType"></field>                        ...>
...                                                     ...
</qp-fieldset>                                          </px:PXFormView>
<qp-fieldset
slot="B"
id="second"
view.bind="Document">
<field
name="CustomerID"
config-allow-edit.bind="true">
</field>
...
</qp-fieldset>
<qp-fieldset
slot="C"
label-size="col-lg-6"
id="summary"
view.bind="Document"
caption="Summary"
class="highlights-section">
<field name="OrderQty"></field>
...
</qp-fieldset>
</qp-template>

This approach is useful when the fields used to be in the same WG container but ended up separated in multiple
fieldsets of the same template, such as three columns in the Summary area.
## Part 8: Testing the Modern UI | 180

The wg-container Attribute with the Explicitly Specified Name
To make the name of WG container exactly the same as it was in the Classic UI, you specify it as a value of the wg-
container attribute. All nested controls will have the same WG container name that is specified in the parent tag.
The following table shows how to specify the name of the WG container in the HTML file and the corresponding
code in ASPX.

HTML                                                     ASPX

<qp-fieldset                                              <px:PXFormView
slot="C"                                                  ID="VoucherDetails"
id="groupVoucherDetails"                                  runat="server"
view.bind="Voucher"                                       DataMember="Voucher"
wg-container="VoucherDetails_Voucher"                     DataSourceID="ds">
caption="Voucher Details">                                <Template>
<field name="VoucherBatchNbr"></field>                      <px:PXTextEdit .../>
<field name="WorkBookID"></field>                           <px:PXTextEdit .../>
</qp-fieldset>                                              </Template>
</px:PXFormView>

Names of WG Containers Inside a Fieldset
In the qp-fieldset tag, if you need to specify the names of the WG container for multiple fields, you put these
fields inside the using tag and specify the name of the WG container in the using tag. This approach may be
useful if fields from diﬀerent WG containers are included in the same fieldset.
The following table shows an example of the using tag in HTML and the corresponding code in ASPX.

HTML                                                     ASPX

<qp-fieldset ...>                                         <px:PXTab ...>
<field name="ClassID"></field>                            <px:PXSelector .../>
<using                                                    <px:PXFormView ID="formDetails2"
wg-container="Details_formDetails2"                       runat="server" DataSourceID="ds"
view="AssetDetails">                                      DataMember="Details" ...>
<field name="PropertyType"></field>                       <Template>
<field name="Status"></field>                               <px:PXLayoutRule .../>
</using>                                                      <px:PXDropDown .../>
<field name="AssetTypeID"></field>                            <px:PXDropDown .../>
...                                                         </Template>
</qp-fieldset>                                              </px:PXFormView>
<px:PXSelector ... />
...
</px:PXTabItem>

The wg-name Attribute
To specify the name of the element inside a container, for the qp-field and qp-button tags, you can use the
wg-name attribute.
The following table shows usage of the wg-name attribute and the corresponding code in ASPX.
## Part 8: Testing the Modern UI | 181

HTML                                                       ASPX

<field                                                      <px:PXLayoutRule
name="CuryOrigDocAmt">                                      runat="server" Merge="True" />
<qp-button                                                  <px:PXNumberEdit
id="buttonAdjustDocumentAmount"                             ID="edCuryOrigDocAmt"
wg-name="btnAdjustDocAmt"                                   runat="server"
state.bind=                                                 DataField="CuryOrigDocAmt"
"model.viewModel.AdjustDocAmt"                           CommitChanges="True"/>
class="col-2">                                            <px:PXButton
</qp-button>                                                  ID="btnAdjustDocAmt"
</field>                                                        CommandName="AdjustDocAmt"
CommandSourceID="ds"
runat="server"
.../>
<px:PXLayoutRule runat="server" />

WG Containers for the qp-info-box Control
By default, the qp-info-box control are not included into wrappers. If you want to include the qp-info-box
control into wrappers, specify the WG attributes manually, as shown in the following example. For more details
about the qp-info-box control, see Error, Warning, or Informational Notification.

HTML                                                       ASPX

<qp-info-box                                                <px:PXFormView
caption.bind=                                               ID="FormAccountTypeChange" ...
"AccountTypeChangePrepare.Message.val-                    DataMember="AccountTypeChangePrepare">
ue"                                                           <Template>
id="messageLabel"                                             <px:PXLayoutRule .../>
type="info"                                                   <px:PXLabel runat="server"
wg-container=                                                   ID="messageLabel"
"AccountTypeChangePrepare_FormAccount-                        SkinID="Label" Height="100%"
TypeChange"                                                       OnDataBinding=
wg-name="MessageLabel_">                                          "messageLabel_DataBinding"/>
</qp-info-box>                                                </Template>
</px:PXFormView>

**Testing of the Modern UI: Update of Tests Written for the Classic UI**

In some cases, wrapper generation may fail due to changes between the Classic UI and the Modern UI. In that case,
you need to update tests as described in this topic.

Table Filters
In the Classic UI, table filters are displayed as tabs, such as All Records and Ready to Process in the following
screenshot.
## Part 8: Testing the Modern UI | 182

**Figure: Table filters in the Classic UI**

The test code for these filters looks as follows.

Screen.Grid.AllRecords();
Screen.Grid.ReadyToProcess();

In the Modern UI, filter tabs do not exist. Instead, table filters are presented as a menu, as shown in the following
screenshot.

**Figure: Table filters in the Modern UI**

You should change the test code for such filters to the following.

Screen.Grid.SelectFilter("Filter Name Here");

User-Defined Fields
In the Classic UI, user-defined fields were located on a separate tab of the applicable data entry form. The name
of the tab was User-Defined Fields (Attributes in the test code). So to test the user-defined field, the test code
had to open the tab and find the specified control.
In the Modern UI, no such tab is displayed. All user-defined fields are added to the fieldsets manually by the user
and displayed among the pre-defined fields.
To support backward compatibility with the Classic UI, in the test code, you need to use the
GetUDF<Type>(UDF_Name, Container_Name) method to access the user-defined field. In the method,
Type is the type of the control for the user-defined field. In the method parameters, you specify the name of the
user-defined field and the name of the container (fieldset) where the user-defined field is located on the Modern UI
form.
## Part 8: Testing the Modern UI | 183

Suppose that the selection of a value for a user-defined field on the form in the Classic UI looks as follows.

OrderSo.Attributes.DynamicControl<Selector>(UDF_Name).Select("UDF_Value")

For the Modern UI, the selection of a value for a user-defined field should look as follows.

OrderSo.GetUDF<Selector>(UDF_Name, Container_Name).Select("UDF_Value")

When you are using the GetUDF method, for the Classic UI, the behavior will be the same as before: The method
will open the User-Defined Fields tab, find the specified control, and select the value.
For the Modern UI, the method searches for the specified user-defined field in the specified container. If the field is
found, the method selects the field value. If the field is not found, the method opens the personalization dialog box;
it then finds the user-defined field in the list, adds it to the container, saves the changes, and selects the value in the
user-defined field.

**Testing of the Modern UI: Frontend Actions in Wrappers**

If an action is declared only in frontend and does not have a corresponding action in the graph, this action is also
included in wrappers and can be used in tests. The name of such action in wrappers is exactly the same as the
name of the action in TypeScript.
For example, suppose that the action is declared in TypeScript, as shown in the following code.

export class AUSchedule extends PXView {
**ViewScreen: PXActionState;**
...
}

The name of the action in wrappers would be ViewScreen. You cannot change the name for the action in
wrappers.
If you have a frontend action (that is, an action that is added through the topBarItems property of the
gridConfig decorator), you need to declare this action in a view class, as shown in the code example above.
**Related Links**
• Table Toolbar Button That Opens a Dialog Box
**Appendix: Initial Configuration | 184**

**Appendix: Initial Configuration**
If for some reason you cannot complete the instructions in To Deploy an Instance for the Training Course, you can
create an Acumatica ERP instance and manually publish the needed customization project as described in this
topic.

Step 1: Deploying the Needed Acumatica ERP Instance for the Training Course
**You deploy an Acumatica ERP instance and configure it as follows:**
1. To deploy a new application instance, open the Acumatica ERP Configuration wizard, and do the following:
a. On the Database Configuration page, type the name of the database: SmartFix_T290.
b. On the Website Configuration page, make sure the Install NodeJS and Use Modern UI as Default check
boxes are selected.
c. On the Tenant Setup page, create a tenant with the T100 dataset inserted by specifying the following
settings:
• Tenant Name: Company
• New: Selected
• Insert Data: T100
• Parent Tenant ID: 1
• Visible: Selected
d. On the Instance Configuration page, in the Local Path of the Instance box, select a folder that’s outside
of the C:\Program Files (x86), C:\Program Files, and C:\Users folder. We recommend
that you store the website folder outside of these folders to avoid an issue with permission to work in
these folders when you perform customization of the website.
The system creates a new Acumatica ERP instance, adds a new tenant, and loads the selected data to it.
2. Sign in to the new tenant by using the following credentials:
• Username: admin
• Password: setup
Change the password when the system prompts you to do so.
3. In the top right corner of the Acumatica ERP screen, click the username and then My Profile. The User
Profile (SM203010) form opens. On the General Info tab, under Personal Settings, select YOGIFON in the
Default Branch box; then click Save on the form toolbar.
In subsequent sign-ins to this account, you’ll be signed in to this branch.
4. Optional: Add the Customization Projects (SM204505), Site Map (SM200520), and Generic Inquiry (SM208000)
forms to your favorites. For details about how to add a form to your favorites, see The Acumatica ERP UI:
Favorites.

Step 2: Publishing the Required Customization Project
You load the customization project with the results of the T240 Processing Forms training course and publish this
project as follows:
1. On the Customization Projects (SM204505) form, create a project with the name PhoneRepairShop, and
open it.
2. In the menu of the Customization Project Editor, click Source Control > Open Project from Folder.
**Appendix: Initial Configuration | 185**

3. In the dialog box that opens, specify the path to the ModernUI\T290\SourceFiles
\PhoneRepairShop folder, which you have downloaded from Acumatica GitHub, and click OK.
4. Bind the customization project to the source code of the extension library as follows:
a. Copy the ModernUI\T290\SourceFiles\PhoneRepairShop_Code folder to the App_Data
\Projects folder of the website.

By default, the system uses the App_Data\Projects folder of the website as the parent
folder for the solution projects of extension libraries.
**If the website folder is outside of the C:\Program Files (x86), C:\Program**
Files, and C:\Users folders, we recommend that you use the App_Data\Projects
folder for the project of the extension library.
**If the website folder is in the C:\Program Files (x86), C:\Program Files, or C:**
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
