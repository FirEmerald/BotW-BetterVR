#pragma once

template <DXGI_FORMAT T>
class Swapchain {
public:
    Swapchain(uint32_t width, uint32_t height, uint32_t sampleCount, uint32_t arraySize = 1);
    ~Swapchain();

    void PrepareRendering();
    ID3D12Resource* StartRendering();
    void FinishRendering();

    XrSwapchain GetHandle() const { return m_swapchain; };
    ID3D12Resource* GetTexture() const { return m_swapchainTextures[m_swapchainImageIdx].Get(); };

    DXGI_FORMAT GetFormat() const { return m_format; };
    [[nodiscard]] uint32_t GetWidth() const { return m_width; };
    [[nodiscard]] uint32_t GetHeight() const { return m_height; };
    [[nodiscard]] uint32_t GetArraySize() const { return m_arraySize; };

private:
    XrSwapchain m_swapchain = XR_NULL_HANDLE;
    uint32_t m_width;
    uint32_t m_height;
    uint32_t m_arraySize = 1;
    DXGI_FORMAT m_format;

    std::vector<ComPtr<ID3D12Resource>> m_swapchainTextures;
    uint32_t m_swapchainImageIdx = 0;
};