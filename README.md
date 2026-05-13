# HW4_BeepPlayer

## 專案簡介

`HW4_BeepPlayer` 是一個使用 **C# Windows Forms** 製作的簡易電子琴程式。程式提供八個音階按鈕，使用者可以點擊按鈕播放 `Do、Re、Mi、Fa、Sol、La、Si、Do` 八個音階。

本專案另外加入了右側樂器動畫效果。當使用者按下音階按鈕時，程式會播放對應音高，旁邊的鼓棒也會產生敲擊動畫，讓電子琴在操作時具有簡單的視覺回饋。

---

## 功能特色

- 使用 Windows Forms 建立圖形化介面
- 提供八個音階按鈕
- 可播放 `Do、Re、Mi、Fa、Sol、La、Si、Do`
- 使用 `SoundPlayer` 播放程式產生的 WAV 音訊
- 不依賴 Windows `Beep`，減少按鍵沒聲音的問題
- 按下音階按鈕時會有鼓棒敲擊動畫
- 視窗大小改變時，控制項會依比例縮放
- 專案內包含 `sample_media/XMAS.WAV` 作為附帶音效資源

---

## 開發環境

| 項目 | 內容 |
|---|---|
| 開發工具 | Visual Studio |
| 程式語言 | C# |
| 專案類型 | Windows Forms App |
| 目標框架 | .NET Framework 4.7.2 |
| 作業系統 | Windows |

---

## 專案結構

```text
HW4_BeepPlayer/
├── HW4_BeepPlayer.sln
├── HW4_BeepPlayer.csproj
├── Program.cs
├── frmBeepPlayer.cs
├── Properties/
│   └── AssemblyInfo.cs
├── sample_media/
│   └── XMAS.WAV
├── screenshots/
│   └── main_window.png
├── .gitignore
└── README.md
```

---

## 主要檔案說明

| 檔案 / 資料夾 | 說明 |
|---|---|
| `HW4_BeepPlayer.sln` | Visual Studio 方案檔，用來開啟整個專案 |
| `HW4_BeepPlayer.csproj` | C# 專案設定檔 |
| `Program.cs` | 程式進入點，負責啟動 Windows Forms 主視窗 |
| `frmBeepPlayer.cs` | 主要視窗、音階播放與鼓棒動畫邏輯 |
| `Properties/` | 專案屬性與組件資訊 |
| `sample_media/XMAS.WAV` | 專案附帶的 WAV 音效資源 |
| `screenshots/main_window.png` | 程式執行畫面截圖 |
| `.gitignore` | Git 忽略設定，用來排除編譯檔與暫存檔 |
| `README.md` | 專案說明文件 |

---

## 執行說明

### 方法一：使用 Visual Studio 執行

1. 下載或 clone 本專案到電腦。
2. 使用 Visual Studio 開啟專案中的方案檔：

```text
HW4_BeepPlayer.sln
```

3. 開啟後，確認 Visual Studio 上方的執行設定為：

```text
Debug / Any CPU
```

4. 按下 Visual Studio 上方的「開始」按鈕，或使用快捷鍵：

```text
F5
```

5. 程式啟動後，即可使用簡易電子琴。

---

## 操作說明

| 按鈕 | 功能 |
|---|---|
| `Do` | 播放 Do 音階 |
| `Re` | 播放 Re 音階 |
| `Mi` | 播放 Mi 音階 |
| `Fa` | 播放 Fa 音階 |
| `Sol` | 播放 Sol 音階 |
| `La` | 播放 La 音階 |
| `Si` | 播放 Si 音階 |
| `Do` | 播放高音 Do 音階 |

按下任一音階按鈕後，程式會播放對應頻率的聲音，右側的鼓棒動畫也會同步觸發。

---

## 音階頻率設計

本專案使用八個常見音階頻率作為電子琴按鍵音高。

| 音階 | 頻率 |
|---|---:|
| Do | 523 Hz |
| Re | 587 Hz |
| Mi | 659 Hz |
| Fa | 698 Hz |
| Sol | 784 Hz |
| La | 880 Hz |
| Si | 988 Hz |
| 高音 Do | 1046 Hz |

---

## 程式設計說明

### 音效播放方式

本專案沒有直接使用 Windows 的 `Console.Beep()`，而是透過程式產生 WAV 音訊資料，再使用 `System.Media.SoundPlayer` 播放。

這樣設計的優點是：

- 減少部分電腦按下按鈕卻沒有聲音的問題
- 可以自行控制頻率與播放時間
- 不需要額外安裝音效套件
- 適合用於 Windows Forms GUI 程式

程式會根據按鈕對應的頻率產生正弦波音訊，並寫入記憶體中的 WAV 格式資料，再交給 `SoundPlayer` 播放。

---

### 樂器動畫設計

右側的樂器動畫使用 `Panel` 的 `Paint` 事件繪製。畫面中包含鼓面與鼓棒，當使用者按下音階按鈕時，會啟動 `Timer`，讓鼓棒在不同位置之間切換，模擬敲擊效果。

動畫流程如下：

1. 使用者按下音階按鈕。
2. 程式啟動樂器動畫。
3. `Timer` 定時觸發畫面重繪。
4. 鼓棒位置在上方與敲擊位置之間切換。
5. 動畫結束後，鼓棒恢復原本狀態。

---

### 視窗縮放設計

本專案在表單載入時會記錄控制項的初始位置與大小。當視窗大小改變時，程式會依照目前視窗大小與初始大小的比例，自動調整按鈕與樂器面板的位置和尺寸。

這樣可以讓介面在不同視窗大小下仍保持基本排列。

---

## 執行截圖

<img width="982" height="265" alt="image" src="https://github.com/user-attachments/assets/1bf65d9f-81f0-4b84-ba00-3be09c5f229a" />

---

