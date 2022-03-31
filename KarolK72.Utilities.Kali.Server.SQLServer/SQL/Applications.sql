﻿BEGIN TRAN;

CREATE TABLE Applications(
	ApplicationID BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ApplicationName VARCHAR(255) NOT NULL,
	ApplicationGUID UNIQUEIDENTIFIER NOT NULL,
	DateTimeCreated DATETIME NOT NULL,
	DateTimeModified DATETIME NOT NULL,
	RowVer INT NOT NULL
);

CREATE INDEX ix_applications_guid ON Applications(ApplicationGUID);

COMMIT TRAN;