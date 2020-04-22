# 5/24/2018	Kannan K	created.

# compiler name
#CC = g++
# linker name
#LINK = g++
#output filename
OUTPUTNAME := $(CurrentWorkingRootFolderName)
# specify target os / platform ex: WIN32, LINUX, OSX etc
#TARGET_PLATFORM:=WIN32
# specify target process architecture ex: WIN32, IA32, AMD64, ARM
#TARGET_ARCHITECTURE:=AMD64
#obj file directory
OBJDIR := obj
#output file directory
BINDIR := bin
#local include directories
include_dirs := include src
#compiler flag
CCFLAGS := -c
CCFLAGS += $(addprefix -I ,$(include_dirs))
# add target platform and architecture
CCFLAGS += $(addprefix -D ,$(TARGET_PLATFORM))
CCFLAGS += $(addprefix -D ,$(TARGET_ARCHITECTURE))
#lib files if any
LIBFILES :=
LINKFLAGS += $(addprefix -I ,$(LIBFILES))
#debug config switch
DEBUGFLAGS := -g
#source directories
SRCPATH = src
#Add depend directories / lib directories
libraries := 

#Select the output file name based on OS it is compiled. 
ifeq ($(TARGET_PLATFORM),WIN32)
	TARGETNAME =  $(OUTPUTNAME).exe
else
	TARGETNAME =  $(OUTPUTNAME)
endif

#choose directory seperator based on the OS it get compiled
ifeq ($(OS),Windows_NT)
	DIRSEP := \
else
	DIRSEP := /
endif

#select appropriate directory removal command
ifeq ($(OS),Windows_NT)
	DELCMD := rd /s /q
else
	DELCMD := rm -r
endif
#select appropriate directory create command
ifeq ($(OS),Windows_NT)
	MKDIRCMD := mkdir
else
	MKDIRCMD := mkdir -p
endif
#compile list of directories needs to be created
directory = $(OBJDIR)
directory += $(BINDIR)
#compile final executable name
EXEC := $(BINDIR)$(DIRSEP)$(TARGETNAME)
#collect all source code files in the current directory
SOURCES_CPP = $(wildcard $(SRCPATH)$(DIRSEP)*.cpp)
SOURCES_CC = $(wildcard $(SRCPATH)$(DIRSEP)*.c)
#compile all version of object file name
OBJECTS_CPP = $(subst $(SRCPATH)$(DIRSEP),$(OBJDIR)$(DIRSEP),$(SOURCES_CPP:.cpp=.o)) 
OBJECTS_CC = $(subst $(SRCPATH)$(DIRSEP),$(OBJDIR)$(DIRSEP),$(SOURCES_CC:.c=_cc.o))
OBJECTS = $(OBJECTS_CPP) $(OBJECTS_CC)

OBJECTS_CPP_DEBUG = $(subst $(SRCPATH)$(DIRSEP),$(OBJDIR)$(DIRSEP),$(SOURCES_CPP:.cpp=_debug.o))
OBJECTS_CC_DEBUG = $(subst $(SRCPATH)$(DIRSEP),$(OBJDIR)$(DIRSEP),$(SOURCES_CC:.c=_cc_debug.o))
OBJECTS_DEBUG = $(OBJECTS_CPP_DEBUG) $(OBJECTS_CC_DEBUG)

#$@ The filename representing the target.
#$% The filename element of an archive member specification.
#$< The filename of the first prerequisite.
#$? The names of all prerequisites that are newer than the target, separated by spaces.

#phony is always out of date and will always cause the target file to be remade
.PHONY: all debug clean $(libraries)

# Main Release target
all: $(directory) $(libraries) $(OBJECTS)
	$(LINK) $(LINKFLAGS) $(OBJECTS) -o $(EXEC)
# Debug target
debug: $(directory) $(libraries) $(OBJECTS_DEBUG)
	$(LINK) $(LINKFLAGS) $(OBJECTS_DEBUG) -o $(EXEC)

# run make on depend directories
$(libraries):
	echo $@
#$(MAKE) --directory=$@

# To obtain cpp files release object files
$(OBJDIR)$(DIRSEP)%.o:$(SRCPATH)$(DIRSEP)%.cpp
	$(CC) $(CCFLAGS) $< -o $@
# To obtain c files release object files
$(OBJDIR)$(DIRSEP)%_cc.o:$(SRCPATH)$(DIRSEP)%.c
	$(CC) $(CCFLAGS) $< -o $@
# To obtain cpp files debug object files
$(OBJDIR)$(DIRSEP)%_debug.o:$(SRCPATH)$(DIRSEP)%.cpp
	$(CC) $(DEBUGFLAGS) $(CCFLAGS) $< -o $@
# To obtain c files debug object files
$(OBJDIR)$(DIRSEP)%_cc_debug.o:$(SRCPATH)$(DIRSEP)%.c
	$(CC) $(DEBUGFLAGS) $(CCFLAGS) $< -o $@
#if directories not exist then make it
$(directory):
	$(MKDIRCMD) $@
# To remove generated files
clean: cleanobj cleanbin
# remove directories
cleanobj:
	$(DELCMD) $(OBJDIR)
cleanbin:
	$(DELCMD) $(BINDIR)
	