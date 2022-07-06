This solution performs extraction of necessary fields 
Input
DataExtractor_Example_Input.csv as a parameter.

Processing
Our task is to extract 3 simple fields and a complex field, then output our own smaller “clean” CSV file. 
Simple fields (just output these values!) 
•	ISIN
•	CFICode
•	Venue

Complex field: AlgoParams  - Extract the value of PriceMultiplier. 
For example, the following string, the extracted value should be  25.0
xxxx|;PriceMultiplier:25.0|;xxxx
The CSV header for this value should be called Contract Size

Output 
The CSV output file with header titles: 
•	ISIN
•	CFICode
•	Venue
•	Contract Size