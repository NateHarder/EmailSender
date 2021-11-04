# EmailSender

To run this project, clone the repository to your local machine and open the EmailLibrary Solution. In the appsettings.json file, you must specify the path to the .mdf database file in the
AttachDbFilename field, and modify the Email, Host, Password and Username fields to the desired credentials. Then, run the project by clicking the green play button at the top of Visual Studio.
The window will open and allow you to type in the Recipient, Subject, and Body of an email, and you can send the email using the send button. The details of the email are logged in the
EmailDatabase.mdf file. Before running the project, make sure that the connection to the EmailDatabaseFile is working by refreshing the connection in the "Server Explorer" view.