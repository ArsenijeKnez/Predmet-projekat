#include "ReadFromFIle.h"

char* ReadFromFile(const std::string& filename) {
    const std::streamsize maxLength = 1024; 
    std::ifstream inFile(filename, std::ios::binary | std::ios::ate);

    if (inFile.is_open()) {
       
        std::streamsize fileSize = inFile.tellg();
        std::streamsize length = (fileSize > maxLength) ? maxLength : fileSize;
        inFile.seekg(0, std::ios::beg);

      
        char* buffer = new char[length + 1]; 
        if (inFile.read(buffer, length)) {
            buffer[length] = '\0';
            std::cout << "Data read from " << filename << std::endl;
            inFile.close();
            return buffer;
        }
        else {
            std::cerr << "Failed to read data from file: " << filename << std::endl;
            inFile.close();
            delete[] buffer;
            return nullptr;
        }
    }
    else {
        std::cerr << "Failed to open file: " << filename << std::endl;
        return nullptr;
    }
}
