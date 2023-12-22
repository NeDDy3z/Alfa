# School Schedule Genrator

## Installation

- Extract the zip file **SchedukeGenerator_vX.zip**
- You'll need to create a **.json** file with all subjects
    - The file must be named **classes.json**
    - The file must be in the same directory as the **Alfa.exe**
    - The file must have the following structure:
    ```json
    [
        {
            "name": "PV",
            "teacher": "Vondřej Srnadík",
            "classroom": "69a",
            "floor": 4,
            "theory": false
       }
    ]
    ```

- Optionally you can edit the App.config file to change the default values of the application in the <appSettings>
  section.
    - Change the value of the **filepath** key to change the default path of the classes.json file
    - Do not set the value of the **threads** key to more than the number of cores of your CPU
  ```xml
    <appSettings>
        <add key="filepath" value="classes.json" />
        <add key="threads" value="4" />
        <add key="timeout" value="10" />
    </appSettings>
    ```
- **Run the *Alfa.exe***

## Usage

The program will generate all possible schedules, it'll rate them and then it'll print the best five.

After executing the program the console will open and you'll be asked whether you want to load the data from the *
*App.config** file or input custom data.

```
Schedule Generator
Load App.config? [Y/N]:
```

- Y - starts the generation with the data from the **App.config** file
- N - you'll be asked to input the data

Enter the filepath of json file which contains all subjects in following formats *C:\file.json* , *file.json*

```
Subjects file path [JSON]:
```

Enter the number of threads you want to use

```
Threads [2-16]:
```

Enter the timeout in seconds (for how long the program will try to generate schedules)

```
Timer [1-10000 s]:
```

Then you will see the following message, meaning the program is generating the schedules

```
13:59:09 - 14:00:49
Generating..
```

After the generation is done, the program will print the best five schedules

```
#1 [4850]
     1.    2.    3.    4.    5.    6.    7.    8.    9.   10.       
Po: A   | PIS | TV  | TV  | C   | M   | TP  |     |     |     |     
Út: DB  | PV  | AM  | CIT | CIT | PSS | PSS |     |     |     |     
St: WA  | WA  | A   | PIS | PV  | PV  | M   |     |     |     |     
Čt: M   | M   | A   | C   | C   | PSS | WA  |     |     |     |     
Pá: PIS | PIS | DB  | DB  | AM  | A   |     |     |     |     |     

#2 [-95150]
     1.    2.    3.    4.    5.    6.    7.    8.    9.   10.       
Po: C   | A   | TV  | TV  | CIT | CIT | AM  |     |     |     | 
Út: PSS | A   | PIS | PSS | PSS | M   | DB  |     |     |     | 
St: PIS | PIS | AM  | M   | M   | TP  |     |     |     |     | 
Čt: WA  | C   | PV  | PV  | PIS | A   | A   |     |     |     | 
Pá: C   | WA  | WA  | DB  | DB  | M   | PV  |     |     |     | 

#3 [-95250]
     1.    2.    3.    4.    5.    6.    7.    8.    9.   10.
Po: M   | A   | PSS | PSS | WA  | WA  |     |     |     |     | 
Út: C   | C   | M   | M   | TV  | TV  | A   |     |     |     | 
St: PIS | PIS | PIS | CIT | CIT | A   | AM  |     |     |     | 
Čt: TP  | C   | DB  | DB  | PV  | PV  | WA  |     |     |     | 
Pá: AM  | PV  | DB  | M   | PIS | PSS | A   |     |     |     | 

#4 [-96350]
     1.    2.    3.    4.    5.    6.    7.    8.    9.   10.
Po: PSS | C   | M   | DB  | PIS | PIS | A   |     |     |     | 
Út: M   | TV  | TV  | PV  | A   | DB  | DB  |     |     |     | 
St: WA  | WA  | CIT | CIT | WA  | PIS |     |     |     |     | 
Čt: A   | PIS | M   | TP  | PSS | PSS | C   |     |     |     | 
Pá: M   | C   | A   | PV  | PV  | AM  | AM  |     |     |     | 

#5 [-97350]
     1.    2.    3.    4.    5.    6.    7.    8.    9.   10.
Po: C   | WA  | DB  | DB  | CIT | CIT | PIS |     |     |     | 
Út: C   | TV  | TV  | M   | A   | PV  | PV  |     |     |     | 
St: PSS | PSS | M   | PIS | A   | AM  |     |     |     |     | 
Čt: WA  | WA  | A   | A   | TP  | M   |     |     |     |     | 
Pá: C   | M   | PIS | PIS | PSS | AM  |     | DB  | PV  |     | 

Generated: 1446210 schedules
Time elapsed: 00:01:40

Press any key to exit...
```

## License

Střední průmyslová škola elektrotechnická, Ječná 30 Praha 2, 120 00