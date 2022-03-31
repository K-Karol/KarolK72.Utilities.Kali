BEGIN TRAN;

CREATE TABLE KaliLogs(
	KaliLogID BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	LogLevel INT NOT NULL,
	Category VARCHAR(255),
	EventID INT,
	EventName TEXT,
	RenderedMessage TEXT NOT NULL,
	Scopes TEXT,
	ExceptionJSON TEXT,
	DateTimeCreated DATETIME NOT NULL,
	DateTimeModified DATETIME NOT NULL,
	RowVer INT NOT NULL
);

CREATE INDEX ix_kalilog_category ON KaliLogs(Category);

CREATE INDEX ix_kalilog_datetimecreated ON KaliLogs(DateTimeCreated);

CREATE INDEX ix_kalilog_datetimemodified ON KaliLogs(DateTimeModified);

COMMIT TRAN;