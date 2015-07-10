# Dynamic-Entity-Framework
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
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
6 - Use select * from Test             = dc.AllData();
7 - delete from Test where ID = @id    = dc.Delete(ID);
8 - Insert into Test values(....)      = dc.Insert(test);   // Test test = new Test(); fill data inside it
9 - Update from Test Set ....          = dc.Update(test)    // Test test = new Test(); fill data inside it
10 - select * from Test where ID = @id = dc.Find(ID);

#Video Tutorial in youtube
https://www.youtube.com/watch?v=h3K6pip60JI
