# caseStudy
This is a case study for EnterBridge

Dependencies for the project:
- .NET SDK, version 10.0
- Microsoft.EntityFrameworkCore, version 10.0.9
- Microsoft.SqlServer LocalDb

  To run it, I pulled the branch from my github onto Visual Studio and ran it locally.

  Key decisions:
 An hour can fly by really quickly so I tried to focus on the most basic and important parts of the application. I used Codex to create an application that is able to list the pricing for all of the products from the EnterBridge API annd capabilities to log sales.
I believed those were the two most important functionalities of the application and once those two were working at the most basic level, I added more detail to each one. For the list pricing, I made sure there was a history log of each price change for each product and also made sure there were only unique items in the list. For sales, I added reoccurring sales flags for quick access and edit buttons for each log in case any information needed to be changed.
My main thought process for building a project is to start at the most basic block and add complexity later on, once the basic ideas are working well.
