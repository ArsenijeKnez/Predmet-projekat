#include <iostream>
#include <fstream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include <cstring>
#include <ctime>

#pragma comment(lib, "Ws2_32.lib")

std::string GetCurrentTimestamp();

void SaveToFile(const char* data, int length, const std::string& filename);