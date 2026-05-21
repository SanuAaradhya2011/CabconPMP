#include "stdafx.h"
#include "Procedures.h"

#ifdef __cplusplus
extern "C" {
#endif

// Split a line into fields delimited by seperators
LONG ParseLine(LPCTSTR ln, CStringArray &fary, LPCTSTR seps)
{
	CString strLn=ln;
	fary.RemoveAll();

	while(!strLn.IsEmpty())
	{
		int idx=fary.Add(strLn.SpanExcluding(seps));

		if(fary[idx].GetLength()==strLn.GetLength()) break;
		strLn.Delete(0, fary[idx].GetLength());
		strLn.Delete(0, strLn.SpanIncluding(seps).GetLength());
	}
	return fary.GetSize();
}

void DebugDB(_com_error &e, LPCTSTR pfm)
{
	CString s, str;
	str.Format(_T("Error in [%s]:\n"), pfm);
	s.Format(_T("Code = %08lx\n"), e.Error()); str+=s;
	s.Format(_T("Code meaning = %s\n"), e.ErrorMessage()); str+=s;
	s.Format(_T("Source = %s\n"), (LPCTSTR) e.Source()); str+=s;
	s.Format(_T("Description = %s\n"), (BSTR) e.Description()); str+=s;
	AfxMessageBox(str);
}
/*
LONG SysCall(LPCTSTR cmd)
{
	DWORD exitCode;
	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	ZeroMemory(&si, sizeof(si));
	si.cb=sizeof(si);

	CString wkdir=_T("c:\\programme\\emh\\camcal\\");
	CString thecmd=cmd;
AfxMessageBox(thecmd);
	if(CreateProcess(NULL, thecmd.GetBuffer(1), NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi))
//	if(CreateProcess(NULL, (TCHAR*)cmd, NULL, NULL, FALSE, 0, NULL, wkdir, &si, &pi))
	{
		WaitForSingleObject(pi.hThread, INFINITE);
		if(0==GetExitCodeProcess(pi.hProcess, &exitCode))
		{
			CString erm;
			erm.Format(_T("Get Exit Code Process error=%d"), GetLastError());
			AfxMessageBox(erm);
		}
	}
	return (LONG) exitCode;
}
*/
/*CString TimeString(const COleDateTime& dt)
{
	CString t="";
	return (t=dt.Format(_T("%d-%m-%Y %H:%M:%S")));
}
*/
#ifdef __cplusplus
}
#endif
