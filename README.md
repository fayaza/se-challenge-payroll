### Documentation:

Please commit the following to this `README.md`:

1. Instructions on how to build/run your application
1. Answers to the following questions:
   - How did you test that your implementation was correct?
   - If this application was destined for a production environment, what would you add or change?
   - What compromises did you have to make as a result of the time constraints of this challenge?

## Submission Instructions

1. Clone the repository.
1. Complete your project as described above within your local repository.
1. Ensure everything you want to commit is committed.
1. Create a git bundle: `git bundle create your_name.bundle --all`
1. Email the bundle file to [dev.careers@waveapps.com](dev.careers@waveapps.com) and CC the recruiter you have been in contact with.
### Code Architecture
Following folder structure has been used in order to segregate different concepts
1. **Contorller:** Main endpoint for the API which will have one POST method to load and save th efile content and one GET method to generate the structured report.
2. **Model:** Contains the classes that are both bind with the database or custom created
3. **Utility:** Consits of helper classes that can be called multiple time like checking nulls. Also contains logic to generate logic of the report
4. **Temp:** tempoary storage of uploaded file which should be deleted once the data is loaded into database
5. **Respository:** Handle to manage the communication between the controller and the database layer. The controller should not connect directly to database
## Environment Setup

The solution has been created in .NET C# using dotnet webapi 3.1 with MySQL as database. Therefore below steps must be performed in order to setup locally and run the application.

1. Installed the prerequisites.
2. Pull the solution to the local file system
3. Setup and configure the database
4. Setup and run the application

**1. Installed the prerequisites** 

- Install the .NET Core 3.1 
https://docs.microsoft.com/en-us/dotnet/core/install/macos
- The application does not depend any specific MySQL server so latest can be install https://dev.mysql.com/downloads/mysql/
- (Optional) Alternatively, XAMPP can be use which will also install mysql can done can be accessible by http://localhost:81/phpmyadmin/ (where 81 is my custom port, default will be 80)
- (Optional) Mysql can be used in order to modify, run and view the database. I personally like SQLyog and SQLWorkbench where the free trail version will suffice.
https://www.mysql.com/products/workbench/
- Visual Studio for Mac or VS Code can be use as an editor to review or update the code
https://code.visualstudio.com/download
- Git CLI can be use for pulling of the code and running the comamnds 
https://git-scm.com/downloads

Verify the installation of dotnet by running the below command
**dotnet --version** or **dotnet --info**
![img_dotnet_version](https://raw.githubusercontent.com/fayaza/se-challenge-payroll/master/setup_images/dotnet_version.png)

**2. Pull the solution to the local file system**
Once everything is installed and working fine, load the respective solution. It will have PayrollAPI which is a webapi and PayrollAPI.Test which is the unit test application.
![img_solution_explorer](https://raw.githubusercontent.com/fayaza/se-challenge-payroll/master/setup_images/solution_explorer.png)

**3. Setup and configure the database**
.sql file will be found in DBScript folder which contains the database schema script to be setup first. Once run, you will have a database entitled "payroll" with three tables
![img_payroll_db](https://raw.githubusercontent.com/fayaza/se-challenge-payroll/master/setup_images/payroll_db.PNG)

You must review and change if needed the connection information which can be find in _appsettings.json_. The connection information in real case should be encrypted

**4. Setup and run the application**
The application also wrapped with SwaggerUI which is also a new for me but my previous experience using POSTMAN and SOAPUI helped me to quickly understand and setup.
The instance once run on port 5001 which can be change from _Properties/launchsettings.json_

Open the cmd or git bash as admin , move to the PayrollAPI root directly and run the following commands:

**dotnet restore** which will restore the dependecies

![img_dotnet_restore](https://raw.githubusercontent.com/fayaza/se-challenge-payroll/master/setup_images/dotnet_restore.png)

**dotnet build** which will build the solution to will check for complier error

![img_dotnet_build](https://raw.githubusercontent.com/fayaza/se-challenge-payroll/master/setup_images/dotnet_build.png)

**dotnet run** which will open the instance

![img_dotnet_run](https://raw.githubusercontent.com/fayaza/se-challenge-payroll/master/setup_images/dotnet_run.png)

once the application is running, you can use:
- https://localhost:5001/index.html - this will open SwaggerUI for you for easy access of API
- https://localhost:5001/api/payroll/report - Alternatively, the GET API which can be run directly
- https://localhost:5001/api/payroll/upload - use Postman or any other API for the POST API by providing the file as input

![img_postman_upload](https://raw.githubusercontent.com/fayaza/se-challenge-payroll/master/setup_images/postman_upload.png)






## Discussion Points
1. Answers to the following questions:
- **How did you test that your implementation was correct?**
Following approaches used to run the testing process:
    1. The code holds a response object for every breakpoint which can deviate from the expected result. Few instructions mentioend in the requirement treated as the assumption like the application will not check the file name formating etc
    2. Developer testing has been done by executing all the scenerio to make sure it return expected response message in different situation
    3. The response message comprises of "status" which can be error/success and a "message" to display meaningful information. It response will return appropoate status code as well like 200, 500,422 etc
    4. Unit test has been added in the solution to execute the API with the concept of AAA (arrange, act, assert) and matching the expectation with the actual result
- **If this application was destined for a production environment, what would you add or change?**
    1. Adding the authorization on the API so it can only be access by authorize people. Also API request can be alter to add more setting in order to avoid injection and hijacking.
    2. Continuous monitoring system/process should be in placed in order to check the status and mitigate the risk before it actuall happen. For example, page size can be defined in order to restrict the result in response loading time is more than 5 seconds
    3. Securing the connection with HTTPS and database with proper authorization and authentication. Migration scripts can be made for easy setup
    4. More testing technique can be part of the process to ensure the longitivity of the application. Performance test, integration and stress testing are examples
    5. Ensure proper major and minor releases and rollback plans
 - **What compromises did you have to make as a result of the time constraints of this challenge?**
 The understanding of the requirement and making its logic was not a difficult part for me to implement. I spend more time than I was expected in order to identify the solution which can be easily setup in other environment. Since I did not worked on MacOS or Linux, I wanted to use such language which is easily setup with less configuration. WebAPI 3.1 and dotnet CLI is something in which new to be and that excel my knowledge in this proces.
