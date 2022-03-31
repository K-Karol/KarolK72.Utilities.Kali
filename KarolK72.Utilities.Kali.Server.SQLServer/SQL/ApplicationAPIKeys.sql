﻿BEGIN TRAN;

CREATE TABLE ApplicationAPIKeys(
	ApplicationAPIKeyID BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ApplicationID BIGINT NOT NULL FOREIGN KEY REFERENCES Applications(ApplicationID),
	KeyName VARCHAR(255) NULL,
	KeyHash VARCHAR(255) NOT NULL,
	LastLogin DATETIME NOT NULL,
	DateTimeCreated DATETIME NOT NULL,
	DateTimeModified DATETIME NOT NULL,
	RowVer INT NOT NULL
)

CREATE INDEX ix_applicationapikeys_KeyHash ON ApplicationAPIKeys(KeyHash);

COMMIT TRAN;