//* Procedures.h	  
//* Procedures define file for Win32 on C/C++
//* History:
//* Date		Author	Comment
//* 2/4/2004	KS	Initial

#ifndef _PROCEDURES_H_
#define _PROCEDURES_H_

#ifdef __cplusplus
extern "C" {
#endif

LONG ParseLine(LPCTSTR ln, CStringArray &fary, LPCTSTR seps=_T(" ,\x9"));
void DebugDB(_com_error &e, LPCTSTR pfm);
//CString TimeString(const COleDateTime& dt);
LONG SysCall(LPCTSTR cmd);

#ifdef __cplusplus
}
#endif

#endif /* _PROCEDURES_H_ */
