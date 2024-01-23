# Import Data by Excel Format
Simple project for importing data with excel format

# How it works?
You can find the excel file called "Excel.csv" from _**ExcelFile**_ folder under _**EngeryExercise**_ folder for program testing. 
This file contains the data related account_id, value and date. 

# Setup
To setup the local database in Visual Studio, you need to following the step as below:


1. Select "_**SQL Server Object Explorer**_" under at the "_**View**_" tab
<img width="358" alt="280491714-42220366-ba55-4759-8368-fceec64390c5" src="https://github.com/karinahuen/ExcelEngeryExercise/assets/107039794/a370e5c3-5661-4767-bb5f-8f4153414fcd">
<br /><br />

2. Afterward, the "_**SQL SERVER OBJECT EXPLORER navigation bar**_" will pop up on the left-hand side. Then, right-click on the database under the server MSSQLLocalDB and select "_**Publish Data-tier Application**_"
<img width="321" alt="280491726-960920e7-13de-4c16-beb9-d7fba6f530f8" src="https://github.com/karinahuen/ExcelEngeryExercise/assets/107039794/6562565c-820d-48c1-9348-66cf54b82e31">
<br />

3. Upon opening it, in "_**File on disk**_" locate the "_**EnergyExerciseContext.dacpac**_" file that you included during the cloning of the documents.

4. Then, enter "_**EnergyExerciseContext**_" for the "_**database name**_" and press the "_**publish button**_"

5. It may display the following message. Please press "_**Yes**_" to proceed.

6. After the "_**Data Tools Operation**_" is complete, you can click the "_**Refresh**_" button in the "_**SQL Server Object Explorer**_" Once you open "_**Databases**_" the "_**EnergyExerciseContext**_" database will appear.

7. You can see the table, store procedure and data in "_**SQL Server Object Explorer**_"

