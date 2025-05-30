# swipelab-server

Server meant to work tougether with the UI at: https://github.com/andreeabirdie/swipelab-web

Database structure:
![SwipeLab](https://github.com/user-attachments/assets/7da37d6f-c64d-43c7-b14e-728f52d89f58)


In order to make the application run, a postgresql database connection url has to be provided allong with all the other missing fields marked with <...>
(Gemini API key, azure blob storage container, new relic license key)

Example flow in the application:

![SequenceDiagram](https://github.com/user-attachments/assets/9cb16372-0341-48be-a18f-37f636cf5cd8)
