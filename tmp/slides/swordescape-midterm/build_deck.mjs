const fs = await import("node:fs/promises");
const path = await import("node:path");
const { Presentation, PresentationFile } = await import("@oai/artifact-tool");

const W = 1280;
const H = 720;
const ROOT = process.cwd();
const OUT_DIR = path.join(ROOT, "Presentation", "midterm");
const SCRATCH_DIR = path.join(ROOT, "tmp", "slides", "swordescape-midterm");
const PREVIEW_DIR = path.join(SCRATCH_DIR, "preview");
const VERIFICATION_DIR = path.join(SCRATCH_DIR, "verification");
const INSPECT_PATH = path.join(SCRATCH_DIR, "inspect.ndjson");
const DESKTOP_CAPTURE_DIR = path.join(ROOT, "..", "Desktop", "여기임");

const IMG = {
  menuBg: path.join(ROOT, "Assets", "Resources", "MainMenu", "moonlit-forest-gate-bg.png"),
  sword: path.join(ROOT, "Assets", "sword.png"),
  batFrame: path.join(ROOT, "tmp", "slides", "swordescape-midterm", "bat-frame.png"),
  tile29: path.join(ROOT, "Assets", "MR-Platformer-PixelAssets-v1", "Main", "Tiles", "Tile-29.png"),
  tile30: path.join(ROOT, "Assets", "MR-Platformer-PixelAssets-v1", "Main", "Tiles", "Tile-30.png"),
  tile31: path.join(ROOT, "Assets", "MR-Platformer-PixelAssets-v1", "Main", "Tiles", "Tile-31.png"),
  tile32: path.join(ROOT, "Assets", "MR-Platformer-PixelAssets-v1", "Main", "Tiles", "Tile-32.png"),
  woodPlatform: path.join(ROOT, "Assets", "MR-Platformer-PixelAssets-v1", "Main", "Objects", "Obj-Big-Wood-Platform-02.png"),
  mainMenu: path.join(DESKTOP_CAPTURE_DIR, "스크린샷 2026-04-20 211441.png"),
  stage1: path.join(DESKTOP_CAPTURE_DIR, "스크린샷 2026-04-20 222000.png"),
  stage2: path.join(DESKTOP_CAPTURE_DIR, "스크린샷 2026-04-20 222020.png"),
  stage3: path.join(DESKTOP_CAPTURE_DIR, "스크린샷 2026-04-20 222057.png"),
  stage4: path.join(DESKTOP_CAPTURE_DIR, "스크린샷 2026-04-20 222139.png"),
  stage5: path.join(DESKTOP_CAPTURE_DIR, "스크린샷 2026-04-20 222221.png"),
};

const BG = "#FFFFFF";
const TOPBAR = "#EEF1F4";
const INK = "#151922";
const SUB = "#5A6470";
const LINE = "#D7DDE4";
const CARD = "#F8FAFC";
const GREEN = "#27B1A1";
const ORANGE = "#F26A1B";
const GOLD = "#D5A238";
const BLUE = "#4F78A5";
const RED = "#B3534D";
const PURPLE = "#765A8C";
const FONT = "Malgun Gothic";

const inspectRecords = [];

const STAGES = [
  {
    no: 4,
    title: "스테이지 1",
    screenshot: IMG.stage1,
    accent: GREEN,
    label: "Stage 1",
    subtitle: "튜토리얼 성격의 첫 스테이지",
    theme: "기본 이동과 검 수집 흐름을 익히는 입문 구간",
    layout: [
      "넓은 평지와 낮은 난이도의 기본 지형으로 시작",
      "플레이어가 검 3개를 모아 문으로 이동하는 흐름을 바로 이해하도록 구성",
      "조작 방법과 게임 목표를 자연스럽게 익히도록 설계",
    ],
    core: [
      "밝고 부드러운 배경 톤으로 시작 구간 분위기 강조",
      "적과 함정을 최소화해 게임 규칙 이해에 집중",
      "전체 게임 구조를 소개하는 역할의 스테이지",
    ],
  },
  {
    no: 5,
    title: "스테이지 2",
    screenshot: IMG.stage2,
    accent: ORANGE,
    label: "Stage 2",
    subtitle: "기본 점프와 함정 회피를 익히는 확장 구간",
    theme: "좁은 발판과 가시 함정으로 점프 정확도를 요구하는 구간",
    layout: [
      "발판 간격을 조금 벌려 단순 이동보다 점프 타이밍이 중요하도록 구성",
      "가시 함정을 넣어 실수 시 바로 리스크를 체감하도록 설계",
      "튜토리얼 이후 본격적인 플랫폼 구간으로 난이도를 한 단계 상승",
    ],
    core: [
      "Stage 1보다 강한 긴장감을 주는 지형 배치",
      "플레이어가 점프 감각을 익히는 첫 본격 구간",
      "단순 이동에서 정확한 컨트롤로 넘어가는 전환점",
    ],
  },
  {
    no: 6,
    title: "스테이지 3",
    screenshot: IMG.stage3,
    accent: BLUE,
    label: "Stage 3",
    subtitle: "적과 아이템 활용을 섞은 중반 구간",
    theme: "적 회피, 검 수집, 아이템 획득을 함께 판단해야 하는 스테이지",
    layout: [
      "이동 경로 근처에 적과 검, 아이템을 함께 배치해 판단 요소 강화",
      "점프만이 아니라 적의 위치를 보고 움직여야 하도록 설계",
      "중반부부터 게임 템포가 빨라지는 구조로 구성",
    ],
    core: [
      "아이템과 적이 동시에 등장해 플레이 흐름이 다양해짐",
      "플레이어가 리스크와 보상을 동시에 고려해야 하는 구간",
      "후반 스테이지 전 난이도를 끌어올리는 연결 역할",
    ],
  },
  {
    no: 7,
    title: "스테이지 4",
    screenshot: IMG.stage4,
    accent: RED,
    label: "Stage 4",
    subtitle: "최소 요구사항을 종합한 본격 도전 스테이지",
    theme: "고정 루트형 적, 추적형 적, 함정, 아이템을 모두 넣은 종합 구간",
    layout: [
      "구간별로 점프력 증가 아이템과 이동속도 증가 아이템, 무적 아이템을 활용하도록 구성",
      "점프 아이템을 먹어야만 높은 절벽을 넘어갈 수 있게 설계",
      "이동형 발판까지 포함해 기다림과 정확한 타이밍을 동시에 요구",
    ],
    core: [
      "최소 요구사항에 있던 요소들을 한 스테이지 안에 통합",
      "아이템을 실제로 사용해야 진행 가능한 구조로 재미 강화",
      "후반 보스전 직전 준비 단계의 난도 높은 스테이지",
    ],
  },
  {
    no: 8,
    title: "스테이지 5",
    screenshot: IMG.stage5,
    accent: PURPLE,
    label: "Stage 5",
    subtitle: "박쥐떼 추격 연출이 핵심인 보스전형 탈출 스테이지",
    theme: "뒤에서 따라오는 박쥐떼를 피해 긴박하게 탈출하는 추격전 구간",
    layout: [
      "플레이어 뒤에서 박쥐떼가 일정 속도로 추격하는 보스전형 연출 적용",
      "점프 아이템과 속도 증가 아이템을 활용해 빠르게 통과해야 하는 구조",
      "마지막 검을 획득하면 박쥐떼가 멈추고 소멸하며 클리어가 연출되도록 구성",
    ],
    core: [
      "직접 보스를 만들기보다 추격 연출로 긴장감 있는 후반부 완성",
      "전용 브금과 클리어 연출을 넣어 최종 스테이지 분위기 강화",
      "전체 프로젝트에서 가장 강한 압박감을 주는 마무리 구간",
    ],
  },
];

async function pathExists(filePath) {
  try {
    await fs.access(filePath);
    return true;
  } catch {
    return false;
  }
}

async function ensureDirs() {
  await fs.mkdir(OUT_DIR, { recursive: true });
  await fs.mkdir(SCRATCH_DIR, { recursive: true });
  await fs.mkdir(PREVIEW_DIR, { recursive: true });
  await fs.mkdir(VERIFICATION_DIR, { recursive: true });
}

async function readImageBlob(imagePath) {
  const bytes = await fs.readFile(imagePath);
  return bytes.buffer.slice(bytes.byteOffset, bytes.byteOffset + bytes.byteLength);
}

function solidLine(fill = "#00000000", width = 0) {
  return { style: "solid", fill, width };
}

function addShape(slide, geometry, left, top, width, height, fill, stroke = "#00000000", strokeWidth = 0) {
  return slide.shapes.add({
    geometry,
    position: { left, top, width, height },
    fill,
    line: solidLine(stroke, strokeWidth),
  });
}

function recordText(slideNo, role, text, left, top, width, height) {
  inspectRecords.push({
    kind: "textbox",
    slide: slideNo,
    role,
    text: String(text ?? ""),
    bbox: [left, top, width, height],
  });
}

function addText(
  slide,
  slideNo,
  text,
  left,
  top,
  width,
  height,
  { size = 20, color = INK, bold = false, align = "left", role = "text", valign = "top" } = {},
) {
  const box = addShape(slide, "rect", left, top, width, height, "#00000000");
  box.text = String(text ?? "");
  box.text.fontSize = size;
  box.text.color = color;
  box.text.bold = bold;
  box.text.typeface = FONT;
  box.text.alignment = align;
  box.text.verticalAlignment = valign;
  box.text.insets = { left: 0, right: 0, top: 0, bottom: 0 };
  recordText(slideNo, role, text, left, top, width, height);
  return box;
}

async function addImage(slide, slideNo, imagePath, left, top, width, height, fit = "contain", role = "image") {
  if (!(await pathExists(imagePath))) {
    return null;
  }

  const image = slide.images.add({
    blob: await readImageBlob(imagePath),
    fit,
    alt: `${role}-${slideNo}`,
  });
  image.position = { left, top, width, height };
  inspectRecords.push({
    kind: "image",
    slide: slideNo,
    role,
    path: imagePath,
    bbox: [left, top, width, height],
  });
  return image;
}

function addBase(slide) {
  slide.background.fill = BG;
  addShape(slide, "rect", 0, 0, W, 74, TOPBAR);
  addShape(slide, "rect", 104, 158, 56, 6, GREEN);
  addShape(slide, "rect", 154, 158, 52, 6, ORANGE);
}

function addTitle(slide, slideNo, title, subtitle = "") {
  addText(slide, slideNo, title, 104, 194, 780, 60, {
    size: 34,
    bold: true,
    role: "title",
  });
  if (subtitle) {
    addText(slide, slideNo, subtitle, 104, 280, 1020, 32, {
      size: 18,
      color: SUB,
      role: "subtitle",
    });
  }
}

function addPanel(slide, left, top, width, height, fill = CARD, stroke = LINE, strokeWidth = 1.2) {
  return addShape(slide, "roundRect", left, top, width, height, fill, stroke, strokeWidth);
}

function addTag(slide, slideNo, text, left, top, width, fill, color = BG) {
  addShape(slide, "roundRect", left, top, width, 30, fill);
  addText(slide, slideNo, text, left, top + 4, width, 20, {
    size: 12,
    color,
    bold: true,
    align: "center",
    role: "tag",
  });
}

function addBulletCard(slide, slideNo, left, top, width, height, title, lines, accent) {
  addPanel(slide, left, top, width, height, BG, LINE, 1.1);
  addShape(slide, "rect", left, top, 8, height, accent);
  addText(slide, slideNo, title, left + 24, top + 18, width - 48, 30, {
    size: 20,
    bold: true,
    role: "card title",
  });
  addText(
    slide,
    slideNo,
    lines.map((lineText) => `- ${lineText}`).join("\n"),
    left + 24,
    top + 62,
    width - 44,
    height - 82,
    {
      size: 16,
      color: SUB,
      role: "card body",
    },
  );
}

async function addFramedScreenshot(slide, slideNo, imagePath, left, top, width, height, accent, role) {
  addPanel(slide, left, top, width, height, "#F3F6F9", LINE, 1.1);
  addShape(slide, "rect", left + 18, top + 18, width - 36, height - 36, "#1A1E24");
  await addImage(slide, slideNo, imagePath, left + 18, top + 18, width - 36, height - 36, "contain", role);
  addShape(slide, "roundRect", left + 18, top + 18, 130, 28, accent);
}

async function addStageSlide(slide, stage) {
  addBase(slide);
  addTitle(slide, stage.no, stage.title, stage.subtitle);
  addTag(slide, stage.no, stage.label, 1018, 194, 116, stage.accent);

  await addFramedScreenshot(slide, stage.no, stage.screenshot, 700, 182, 448, 260, stage.accent, "stage screenshot");
  addText(slide, stage.no, "실제 플레이 캡처", 728, 202, 120, 18, {
    size: 12,
    bold: true,
    color: BG,
    role: "screenshot label",
  });

  addBulletCard(slide, stage.no, 104, 344, 500, 118, "스테이지 테마", [stage.theme], stage.accent);
  addBulletCard(slide, stage.no, 104, 486, 500, 164, "구성 방식", stage.layout, stage.accent);
  addBulletCard(slide, stage.no, 660, 470, 500, 180, "핵심 요소", stage.core, stage.accent);
}

function addNotes(slide, text) {
  slide.speakerNotes.setText(text);
}

async function buildSlides() {
  const p = Presentation.create({ slideSize: { width: W, height: H } });

  {
    const slideNo = 1;
    const slide = p.slides.add();
    addBase(slide);
    addText(slide, slideNo, "SWORD ESCAPE", 104, 210, 520, 68, {
      size: 40,
      bold: true,
      role: "cover title",
    });
    addText(slide, slideNo, "검을 모아 탈출하는 기사 판타지 플랫포머", 104, 288, 560, 30, {
      size: 20,
      color: SUB,
      role: "cover subtitle",
    });
    addText(
      slide,
      slideNo,
      "플레이어는 각 스테이지에서 검 3개를 모아 문으로 이동하며 다음 구간으로 진행합니다.\n기본 점프 액션 위에 함정, 적, 아이템, 추격 연출을 결합해 후반으로 갈수록 긴장감이 커지도록 설계했습니다.",
      104,
      360,
      500,
      126,
      { size: 18, role: "cover body" },
    );
    addPanel(slide, 664, 142, 500, 396, "#EAF0F5", LINE, 1.2);
    await addImage(slide, slideNo, IMG.menuBg, 684, 162, 460, 356, "cover", "cover background");
    addPanel(slide, 104, 566, 1060, 92, BG, LINE, 1.1);
    addText(slide, slideNo, "프로젝트 요약", 128, 590, 140, 24, {
      size: 14,
      color: GREEN,
      bold: true,
      role: "cover label",
    });
    addText(
      slide,
      slideNo,
      "단순 수집형 맵이 아니라 점프 액션과 추격 탈출 연출을 결합해 짧은 플레이 안에서도 스테이지마다 다른 긴장감을 느끼도록 제작했습니다.",
      128,
      620,
      980,
      24,
      { size: 18, role: "cover summary" },
    );
    addNotes(slide, "게임 제목과 전체 콘셉트를 소개하는 슬라이드");
  }

  {
    const slideNo = 2;
    const slide = p.slides.add();
    addBase(slide);
    addTitle(slide, slideNo, "메인 메뉴 화면", "실제 게임 시작 화면 캡처와 포함된 기능을 함께 설명");
    addBulletCard(slide, slideNo, 104, 348, 456, 220, "포함 기능", [
      "게임 시작 버튼으로 바로 Stage 1 진입",
      "도움말 팝업으로 이동, 점프, 선택 키 안내",
      "게임 나가기 버튼 추가",
      "한글 폰트 깨짐 문제를 수정해 메뉴 가독성 확보",
    ], GREEN);
    addText(
      slide,
      slideNo,
      "메인 메뉴는 코드로 직접 구성해 해상도가 달라도 배치가 유지되도록 만들었고, 판타지 분위기의 배경과 함께 게임 콘셉트가 바로 보이도록 정리했습니다.",
      104,
      600,
      980,
      28,
      { size: 16, color: SUB, role: "menu caption" },
    );
    await addFramedScreenshot(slide, slideNo, IMG.mainMenu, 610, 170, 554, 430, BLUE, "main menu screenshot");
    addText(slide, slideNo, "실제 메인 UI 캡처", 638, 190, 132, 18, {
      size: 12,
      bold: true,
      color: BG,
      role: "menu screenshot label",
    });
    addNotes(slide, "실제 메인 메뉴 화면과 포함된 기능 설명");
  }

  {
    const slideNo = 3;
    const slide = p.slides.add();
    addBase(slide);
    addTitle(slide, slideNo, "조작 방법", "이동, 점프, 아이템 활용을 중심으로 기본 플레이 방법 설명");
    addBulletCard(slide, slideNo, 104, 350, 330, 228, "기본 조작", [
      "A / D 또는 방향키로 좌우 이동",
      "Space로 점프",
      "메뉴에서는 C 키로 항목 선택",
    ], GREEN);
    addBulletCard(slide, slideNo, 474, 350, 330, 228, "게임 진행", [
      "각 스테이지에서 검 3개를 모으면 문이 열림",
      "문에 닿으면 다음 스테이지로 이동",
      "적과 함정을 피하면서 목적지까지 도달해야 함",
    ], GOLD);
    addBulletCard(slide, slideNo, 844, 350, 330, 228, "추가 기능", [
      "점프력 증가 아이템으로 높은 지형 돌파",
      "이동속도 증가 아이템으로 추격 구간 대응",
      "무적 아이템으로 위험 구간 통과",
    ], ORANGE);
    addNotes(slide, "조작 방법과 기본 플레이 구조 설명");
  }

  for (const stage of STAGES) {
    const slide = p.slides.add();
    await addStageSlide(slide, stage);
    addNotes(slide, `${stage.title}의 테마, 구성 방식, 핵심 요소 설명`);
  }

  {
    const slideNo = 9;
    const slide = p.slides.add();
    addBase(slide);
    addTitle(slide, slideNo, "추가 구현 요소", "최소 요구사항 외에 직접 추가한 기능과 넣은 이유 정리");
    addBulletCard(slide, slideNo, 104, 350, 330, 228, "추가 기능 1", [
      "박쥐떼 추격 연출 구현",
      "마지막 검 획득 시 박쥐 소멸과 클리어 연출 추가",
      "보스를 직접 만들지 않고도 긴장감 있는 후반부 구성",
    ], GREEN);
    addBulletCard(slide, slideNo, 474, 350, 330, 228, "추가 기능 2", [
      "이동형 발판 스크립트 구현",
      "정방향과 역방향으로 움직이는 발판 모두 지원",
      "스테이지 흐름 속에 기다림과 타이밍 요소 추가",
    ], BLUE);
    addBulletCard(slide, slideNo, 844, 350, 330, 228, "추가 기능 3", [
      "Stage 5 전용 BGM 추가",
      "점프, 아이템 획득, 메뉴 선택 효과음 적용",
      "메인 메뉴 종료 버튼과 클리어 UI 보강",
    ], ORANGE);
    addPanel(slide, 104, 610, 1070, 48, BG, LINE, 1);
    addText(
      slide,
      slideNo,
      "추가 구현 요소를 넣은 이유: 기본 과제 요구사항만 채우는 것이 아니라 후반부 긴장감과 몰입감을 높여 게임이 한 단계 더 완성된 느낌이 나도록 만들기 위해서입니다.",
      124,
      624,
      1020,
      18,
      { size: 15, color: SUB, role: "extra feature reason" },
    );
    addNotes(slide, "최소 요구사항 외에 직접 추가한 기능 정리");
  }

  {
    const slideNo = 10;
    const slide = p.slides.add();
    addBase(slide);
    addTitle(slide, slideNo, "사용 리소스 정리", "직접 제작, 에셋 스토어, AI 활용 범위를 구분해서 정리");
    addPanel(slide, 876, 170, 286, 140, "#F8FAFC", LINE, 1);
    addText(slide, slideNo, "많이 사용한 타일 / 오브젝트 예시", 904, 190, 220, 18, {
      size: 13,
      color: ORANGE,
      bold: true,
      role: "asset sample title",
    });
    await addImage(slide, slideNo, IMG.tile29, 898, 222, 42, 42, "contain", "tile sample");
    await addImage(slide, slideNo, IMG.tile30, 946, 222, 42, 42, "contain", "tile sample");
    await addImage(slide, slideNo, IMG.tile31, 994, 222, 42, 42, "contain", "tile sample");
    await addImage(slide, slideNo, IMG.tile32, 1042, 222, 42, 42, "contain", "tile sample");
    await addImage(slide, slideNo, IMG.woodPlatform, 1090, 228, 48, 28, "contain", "object sample");
    addBulletCard(slide, slideNo, 104, 340, 326, 230, "직접 제작 / 직접 구성", [
      "스테이지 1~5 타일맵 배치와 전체 진행 흐름 구성",
      "메인 메뉴 UI, 클리어 문구, 도움말, 종료 버튼 직접 구성",
      "박쥐 추격, 이동형 발판, 아이템 연동, 사운드 연결 직접 구현",
    ], GREEN);
    addBulletCard(slide, slideNo, 476, 340, 326, 230, "에셋 스토어 / 외부 리소스", [
      "Unity Asset Store 기반 픽셀 플랫폼 에셋 사용",
      "사용 패키지 예시: MR-Platformer-PixelAssets-v1",
      "타일, 캐릭터, 적, 오브젝트, 일부 배경 리소스에 활용",
    ], ORANGE);
    addBulletCard(slide, slideNo, 848, 340, 326, 230, "AI 활용 여부", [
      "코드 구현 방향 정리와 오류 원인 분석 보조",
      "메인 UI 구성 아이디어와 발표 자료 정리에 참고",
      "최종 게임 구현과 수정, 구조 이해와 설명은 직접 진행",
    ], BLUE);
    addText(
      slide,
      slideNo,
      "발표 시에는 'AI가 대신 만들어준 게임'이 아니라, 구현 과정에서 코드 정리와 문제 해결 방향을 보조받고 실제 적용과 수정은 직접 진행했다는 점을 설명하면 됩니다.",
      104,
      612,
      1040,
      34,
      { size: 15, color: SUB, role: "resource note" },
    );
    addNotes(slide, "사용 리소스와 AI 활용 범위 설명");
  }

  {
    const slideNo = 11;
    const slide = p.slides.add();
    addBase(slide);
    addTitle(slide, slideNo, "마무리");
    addText(
      slide,
      slideNo,
      "Sword Escape는 검 수집, 점프 액션, 추격 탈출 연출을 결합한 2D 플랫포머 게임입니다.\n\nStage 1부터 Stage 5까지 난이도를 점차 올리면서,\n마지막에는 박쥐떼 추격과 최종 검 연출로 게임의 마무리를 완성했습니다.",
      104,
      344,
      620,
      180,
      { size: 22, role: "closing body" },
    );
    addText(slide, slideNo, "감사합니다", 820, 360, 300, 60, {
      size: 40,
      bold: true,
      align: "center",
      role: "thanks",
    });
    addNotes(slide, "발표 마무리 슬라이드");
  }

  return p;
}

async function saveBlobToFile(blob, filePath) {
  const bytes = new Uint8Array(await blob.arrayBuffer());
  await fs.writeFile(filePath, bytes);
}

async function exportDeck(presentation) {
  await ensureDirs();

  const previewPaths = [];
  for (let i = 0; i < presentation.slides.items.length; i += 1) {
    const preview = await presentation.export({
      slide: presentation.slides.items[i],
      format: "png",
      scale: 1,
    });
    const previewPath = path.join(PREVIEW_DIR, `slide-${String(i + 1).padStart(2, "0")}.png`);
    await saveBlobToFile(preview, previewPath);
    previewPaths.push(previewPath);
  }

  const pptxBlob = await PresentationFile.exportPptx(presentation);
  const pptxPath = path.join(OUT_DIR, "output.pptx");
  await pptxBlob.save(pptxPath);

  const inspect = [
    JSON.stringify({ kind: "deck", slideCount: presentation.slides.count }),
    ...inspectRecords.map((record) => JSON.stringify(record)),
  ].join("\n");
  await fs.writeFile(INSPECT_PATH, `${inspect}\n`, "utf8");
  await fs.writeFile(
    path.join(VERIFICATION_DIR, "render_verify_loops.ndjson"),
    `${JSON.stringify({ kind: "render_verify_loop", loop: 1, previewCount: previewPaths.length, pptxPath })}\n`,
    "utf8",
  );

  return pptxPath;
}

const presentation = await buildSlides();
const outputPath = await exportDeck(presentation);
console.log(outputPath);
