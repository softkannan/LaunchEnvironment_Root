void Inject_Loader( const DllPayload& Payload, const std::string& Path )
{
  STARTUPINFOA StartupInfo = {0};
  PROCESS_INFORMATION ProcessInformation;
 
  // initialize the structures
  StartupInfo.cb = sizeof(StartupInfo);
 
  // attempt to load the specified target
  if ( CreateProcessA(
      Path.c_str(),
      NULL,
      NULL,
      NULL,
      FALSE,
      CREATE_SUSPENDED,
      NULL,
      NULL,
      &StartupInfo,
      &ProcessInformation
    ) )
  {
    Handle hProcess( ProcessInformation.hProcess );
 
    // wait for the process to done
    try
    {
      // locate the entry point
      OptionalHeader optionalheader = PortableExecutable::FromFile( Path.c_str() ).NtHeaders.OptionalHeader;
      LPVOID entry = (LPVOID)(optionalheader.ImageBase + optionalheader.AddressOfEntryPoint);
 
      // patch the entry point with infinite loop
      PageProtect protect( hProcess, entry, 2, PAGE_EXECUTE_READWRITE );
 
      std::string oep = VMemory::Read( hProcess, entry, 2 );
      VMemory::Write( hProcess, entry, "\xEB\xFE" );            // JMP $-2
 
      // resume the main thread
      ResumeThread( ProcessInformation.hThread );
 
      // wait until the thread stuck at entry point
      CONTEXT context;
 
      for ( unsigned int i = 0; i < 50 && context.Eip != (DWORD)entry; ++i )
      {
        // patience.
        Sleep(100);
 
        // read the thread context
        context.ContextFlags = CONTEXT_CONTROL;
        GetThreadContext( ProcessInformation.hThread, &context );
      }
      if ( context.Eip != (DWORD)entry )
      {
        // wait timed out
        throw "entry point blockade timed out";
      }
 
      // inject DLL payload into remote process
      Inject_CreateRemoteThread( Payload, hProcess );
 
      // pause and restore original entry point
      SuspendThread( ProcessInformation.hThread );
      VMemory::Write( hProcess, entry, oep );
 
      // you are ready to go
      ResumeThread( ProcessInformation.hThread );
    }
    catch ( ... )
    {
      // terminate the newly spawned process
      TerminateProcess( hProcess, -1 );
 
      // rethrow the exception to top-level handler
      throw;
    }
  }
  else
  {
    // are you sure this is a valid target ?
    throw "unable to load the specified executable";
  }
}