USE master;

IF DB_ID('Digital_Matter_Devices') IS NOT NULL
	DROP DATABASE Digital_Matter_Devices;
GO

create database Digital_Matter_Devices;
GO

USE Digital_Matter_Devices;

-- Create Firmware Table
/*
Firmware_ID Unique variable to identify firmware
Version variable to identify the version of the firmware
Release_Date variable to identify the age of the firmware
Description variable to describe the function of the firmware
*/
CREATE TABLE Firmware(
    Firmware_ID INT NOT NULL PRIMARY KEY IDENTITY,
    Version NVARCHAR(50) NOT NULL,
    Release_Date DATE NOT NULL,
    Description NVARCHAR(MAX) NULL
);

-- Create Groups Table
/*
Group_ID unique variable to identify and link group ID's
Group_Name variable to give a discriptive group name
Parent_Group_ID variable to establish parent link
links the group ID to a parent group
*/
CREATE TABLE Groups(
    Group_ID INT NOT NULL PRIMARY KEY IDENTITY,
    Group_Name NVARCHAR(255) NOT NULL,
    Parent_Group_ID INT NULL,
    FOREIGN KEY (Parent_Group_ID) REFERENCES Groups(Group_ID)
);

--Creates Devices Table
/*
Device_ID unique variable that identifies each device, PRIMARY to allow for indevidual identification
Device_Name variable to allow users to identify device model
Device_Type variable to identify type of device
Group_ID variable to link device to group
Frimware_Id variable to identify the firmware running on the device
Establish link between groups and firmware table
*/
CREATE TABLE Devices(
	Device_ID INT NOT NULL PRIMARY KEY IDENTITY,
	Device_Name NVARCHAR(255) NOT NULL,
	Device_Type NVARCHAR(100) NOT NULL,
	Group_ID INT NOT NULL,
	Firmware_ID INT NOT NULL,
	FOREIGN KEY (Group_ID) REFERENCES Groups(Group_ID),
	FOREIGN KEY (Firmware_ID) REFERENCES Firmware(Firmware_ID)
);

-- Insert Firmware versions
INSERT INTO Firmware(Version, Release_Date, Description)
VALUES
    ('v1.0', '2022-01-01', 'STM32 Base tracking'),
    ('v2.0', '2022-05-01', 'STM32 Battery optimization');

-- Insert data into Groups Table
INSERT INTO Groups(Group_Name, Parent_Group_ID)
VALUES
    ('STM32', NULL),
    ('STM32H7', 1),
    ('STM32F7', 1);

-- Insert sample data into Devices Table
INSERT INTO Devices(Device_Name, Device_Type, Group_ID, Firmware_ID)
VALUES
    ('Device 1', 'Type A', 2, 1),
    ('Device 2', 'Type B', 3, 2);