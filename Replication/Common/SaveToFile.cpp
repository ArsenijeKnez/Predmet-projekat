#include "SaveToFile.h"

std::string GetCurrentTimestamp() {
    std::time_t now = std::time(nullptr);
    std::tm timeInfo;
    char buf[100];

    localtime_s(&timeInfo, &now);

    if (std::strftime(buf, sizeof(buf), "%Y-%m-%d %H:%M:%S", &timeInfo)) {
        return std::string(buf);
    }
    return "";
}

void SaveToFile(const char* data, int length, const std::string& filename) {
    std::ofstream outFile(filename, std::ios::binary | std::ios::app);
    if (outFile.is_open()) {
        std::string timestamp = GetCurrentTimestamp();
        std::string dataWithTimestamp = timestamp + " " + std::string(data, length) + "\n";

        outFile.write(dataWithTimestamp.c_str(), dataWithTimestamp.size()); 
        outFile.close();

        std::cout << "Data saved to " << filename << std::endl;
    }
    else 
    {
        std::cerr << "Failed to open file: " << filename << std::endl;
    }
}
