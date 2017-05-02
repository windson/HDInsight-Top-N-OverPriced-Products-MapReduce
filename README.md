# HDInsight-Top-N-OverPriced-Products-MapReduce
Top N OverPriced Products Using HDInsight streaming MapReduce Job
This repository deals with retrieving Top N OverPriced Products (mentioned in detail in the problem description below). The reviews are present in dataset reviews.csv and its meta is present in metadata.csv

## Problem Statement

We will be finding the products that are termed as expensive by the users.
We have defined a very small dictionary of terms: [expensive, costly, high-priced, pricey, overpriced, at a premium] which then will be used to find top-N products which are considered overpriced by the users.
(For the programming part, N should be user given. In the report show the top-15 overpriced products.)
Compute the top-N products across all categories. Output should be in the format: <<category, Product_Id, Title>, rev_count> where rev_count represents count of reviews which consider the product as overpriced.
[Note: We need to compute the top-N products across all categories and not for each category.]

### Datasets

The meta of files in the dataset is as follows

In reviews.csv, each line contains comma-separated values in the following order:

User_Id:
Product_Id:
User_Name:
Up_votes: Total number of up-votes given to the review
Overall_votes: Total votes including up-votes and down-votes given to the review
Review_Text: Review written by the user
Rating: Rating given to the product by the user
Summary: Title given to the review by the user
Unix_Review_Time: Time at which review was written in UNIX time format
Review_Date: Date on which review was submitted
In metadata.csv, each line contains comma-separated values in the following order:

Product_Id: Id of the product
Title: Name of the product
Price: Price of product in US dollars
imUrl: url for the product image
Sales_Rank:
Brand:
Category: Category of the product
Problem Statement

We will be finding the top-N overpriced products across all categories which has overpriced in its reviews. (For the programming part, N should be user given. In output folder the top-15 products are reported.)

### Mapper
The Mapper takes in the metadata.csv as the input for processing and stream productId, price, title and category to the reducer for processing

### Reducer
The reducer processes the streaming data from the mapper and calculates the top N expensive products which as the term 'overpriced' in its review text. It also takes N as the input argument which indicates the number of expensive products to be returned as a result.

### Setup

Spinup the HDInsight Cluster on Azure You can check for reference here. I have created this cluster on Linux VM. Choose Azure Storage as Default Storage.

Compile and build the projects in TopNExpensiveProducts.sln and (in either release or debug mode but I prefer release mode for production purposes).

Now upload TopNExpensiveMapper.exe and TopNExpensiveReducer.exe to the default azure storage location configured with HDInsight using the Server Explorer. Also upload Reviews.csv file to directory of your choice. For time being I upload it to /reviews/input/Reviews.csv For beginners follow this link to upload files to HDInsight which provides various interfaces to upload data to an HDInsight cluster.

### Commands

      yarn jar /usr/hdp/current/hadoop-mapreduce-client/hadoop-streaming.jar -files wasbs:///TopNExpensiveMapper.exe,wasbs:///TopNExpensiveReducer.exe -mapper TopNExpensiveMapper.exe -reducer "TopNExpensiveReducer.exe 15" -input /reviews/input/metadata.csv -output /reviews/output
           
#### Details of Command

The command sends various arguemnts to hadoop-streaming.jar file with yarn as interface that processes the map reduce streaming job.

-files takes map and reduce executables indicating there location on wasbs (Windows Azure Storage Blob).

-mapper with the name of the executable of mapper process.

-reducer takes the name of the executable of reducer process. Note that here we are also passing integer arguemnt 15 to get top 15 expensive products across all categories. If not argument passed then default N value will be taken as 15

-input is the location of the data to be processed.

-output is the desired location to store the processed data. This needs to be a fresh location every time we run the map reduce process.

After running the command the output folder will contain a text file named part-00000



### Output

To speed up the process we are reading the reviews.csv directly in the reducer to get the product details like review_text and Count of Reviews. The output of the mapreduce process gives a file named part-00000 and is located in the output folder which can be viewed by executing the command below.
      
      hdfs dfs -text /reviews/output/part-00000
