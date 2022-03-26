CREATE TABLE KaliLogs(
	KaliLogID BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	LogLevel INT NOT NULL,
	Category TEXT,
	EventID INT,
	EventName TEXT,
	RenderedMessage TEXT NOT NULL,
	Scopes TEXT,
	ExceptionJSON TEXT,
	DateTimeCreated DATETIME NOT NULL,
	DateTimeModified DATETIME NOT NULL,
	RowVer INT NOT NULL
)