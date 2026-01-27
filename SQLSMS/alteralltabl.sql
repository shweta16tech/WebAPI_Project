ALTER TABLE users
ADD 
    createdat DATETIME NOT NULL DEFAULT GETUTCDATE(),
    createdby NVARCHAR(100) NULL,
    modifiedat DATETIME NULL,
    modifiedby NVARCHAR(100) NULL;

ALTER TABLE customer
ADD 
    createdat DATETIME NOT NULL DEFAULT GETUTCDATE(),
    createdby NVARCHAR(100) NULL,
    modifiedat DATETIME NULL,
    modifiedby NVARCHAR(100) NULL;


ALTER TABLE product
ADD 
    createdat DATETIME NOT NULL DEFAULT GETUTCDATE(),
    createdby NVARCHAR(100) NULL,
    modifiedat DATETIME NULL,
    modifiedby NVARCHAR(100) NULL;

ALTER TABLE sales
ADD 
    invoicedate DATETIME NULL,          
    createdat DATETIME NOT NULL DEFAULT GETUTCDATE(),
    createdby NVARCHAR(100) NULL,
    modifiedat DATETIME NULL,
    modifiedby NVARCHAR(100) NULL;


