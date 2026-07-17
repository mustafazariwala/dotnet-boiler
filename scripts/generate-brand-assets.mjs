import sharp from "sharp";

const assets = [
  {
    input: "frontend/public/assets/og-image.svg",
    output: "frontend/public/assets/og-image.png",
    width: 1200,
    height: 630
  },
  {
    input: "frontend/public/assets/tag-image.svg",
    output: "frontend/public/assets/tag-image.png",
    width: 600,
    height: 315
  },
  {
    input: "frontend/public/assets/logo.svg",
    output: "frontend/public/apple-touch-icon.png",
    width: 180,
    height: 180
  }
];

await Promise.all(
  assets.map((asset) =>
    sharp(asset.input)
      .resize(asset.width, asset.height)
      .png()
      .toFile(asset.output)
  )
);
