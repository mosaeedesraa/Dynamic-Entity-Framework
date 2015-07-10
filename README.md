# Dynamic Entity Framework

Call sql tables and stored procedures easily and fast

#Installation : 

1 - Create database (Sample) and Tables (dbo.Test)

2 - Create class (class name : Test) {table name = test  ====== class name = test}

3 - Create properties (their names = columns names)

// SQl scripts for table

CREATE TABLE [dbo].[Test](

	[ID] [int] IDENTITY(1,1) NOT NULL,
	
	[Name] [nvarchar](50) NOT NULL,
	
	[Age] [int] NOT NULL,
	
	[Address] [nvarchar](50) NOT NULL,
	
 CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED 
 
([ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 

IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO



4 - Test Class : 
 public class Test
    {
    
        public Int32 ID { get; set; }
        
        public string Name { get; set; }
        
        public Int32 Age { get; set; }
        
        public string Address { get; set; }
        
    }
    
    
    5 - Call Entity : 
    

        public static string ConnectionString =  
        WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        
        
        // call entity for Test table
        
        Tables<Test> dc = new Tables<Test>(ConnectionString); 
        
6 - In SQL :  Use select * from Test             

In Entity :   dc.AllData();

7 - In SQL :  delete from Test where ID = @id    

In Entity : dc.Delete(ID);

8 - In SQL :  Insert into Test values(....)      

In Entity  : dc.Insert(test);   // Test test = new Test(); fill data inside it

9 - In SQL  : Update from Test Set ....          

In Entity      : dc.Update(test)    // Test test = new Test(); fill data inside it

10 - In SQL : select * from Test where ID = @id 

In Entity   : dc.Find(ID);

#Nuget Package : 

https://www.nuget.org/packages/DynamicEntity/


#Video Tutorial in youtube

https://www.youtube.com/watch?v=nC_8IFQowAg&feature=youtu.be

