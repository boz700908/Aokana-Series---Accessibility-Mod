# 创建GitHub Release的步骤

## 方法1：通过GitHub网页界面

1. 访问仓库页面：https://github.com/boz700908/Aokana-Series---Accessibility-Mod

2. 点击右侧的 "Releases" 或访问：
   https://github.com/boz700908/Aokana-Series---Accessibility-Mod/releases

3. 点击 "Draft a new release" 按钮

4. 填写以下信息：
   - **Tag**: v1.0.0 (已创建)
   - **Release title**: Aokana Accessibility Mod v1.0.0
   - **Description**: 复制 RELEASE_NOTES_v1.0.0.md 的内容

5. 上传文件：
   - 点击 "Attach binaries"
   - 上传 `AokanaAccess-v1.0.0.zip`

6. 勾选 "Set as the latest release"

7. 点击 "Publish release"

## 方法2：使用GitHub CLI (gh)

如果已安装GitHub CLI，可以运行：

```bash
gh release create v1.0.0 \
  AokanaAccess-v1.0.0.zip \
  --title "Aokana Accessibility Mod v1.0.0" \
  --notes-file RELEASE_NOTES_v1.0.0.md
```

## 发布包内容

AokanaAccess-v1.0.0.zip 包含：
- Mods/AokanaAccess.dll (73KB)
- UserData/AokanaAccess/ (本地化文件)
- README.txt (安装说明)

## 发布后

发布完成后，用户可以通过以下链接下载：
https://github.com/boz700908/Aokana-Series---Accessibility-Mod/releases/tag/v1.0.0
