INSERT INTO Artworks (Title, Description, CategoryId, ArtistId, Price, ImageUrl, Year, Dimensions, IsAvailable, IsFeatured, CreatedDate) VALUES 
-- PAINTINGS (CategoryId = 1)
('Crimson Dreams', 'A vibrant exploration of color and emotion, where bold strokes of red and crimson dance across the canvas.', 1, 1, 2500.00, '/images/artworks/artwork1.jpg', 2024, '36" × 48"', 1, 1, DATEADD(day, -15, GETDATE())),
('Sunset Dreams', 'Golden hues blend seamlessly with warm oranges in this stunning abstract piece.', 1, 1, 1200.00, '/images/artworks/artwork2.jpg', 2024, '24" × 36"', 1, 1, DATEADD(day, -14, GETDATE())),
('Urban Nights', 'A captivating city landscape that brings urban energy to life with neon lights.', 1, 1, 1800.00, '/images/artworks/artwork5.jpg', 2024, '30" × 40"', 1, 0, DATEADD(day, -13, GETDATE())),
('Abstract Dreams', 'Mixed media artwork combining acrylics, charcoal, and gold leaf.', 1, 1, 2200.00, '/images/artworks/artwork7.jpg', 2024, '36" × 48"', 1, 1, DATEADD(day, -12, GETDATE())),
('Crimson Sunset', 'Bold reds and oranges dominate this expressive oil painting.', 1, 1, 1500.00, '/images/artworks/artwork10.jpg', 2024, '24" × 36"', 1, 0, DATEADD(day, -11, GETDATE())),
('Golden Hour', 'Warm, luminous tones fill this acrylic painting inspired by sunset.', 1, 1, 1900.00, '/images/artworks/artwork12.png', 2024, '30" × 40"', 1, 0, DATEADD(day, -10, GETDATE())),
('Blue Horizons', 'Expansive abstract painting dominated by various shades of blue.', 1, 1, 2100.00, '/images/artworks/artwork15.jpg', 2024, '36" × 48"', 1, 0, DATEADD(day, -9, GETDATE())),

-- PHOTOGRAPHY (CategoryId = 2)
('Mountain Vista', 'A breathtaking landscape photograph capturing majestic peaks at golden hour.', 2, 2, 450.00, '/images/artworks/artwork3.jpg', 2023, '18" × 24"', 1, 1, DATEADD(day, -8, GETDATE())),
('Ocean Waves', 'Powerful waves crash against rocky shores in this stunning seascape.', 2, 2, 650.00, '/images/artworks/artwork6.png', 2023, '20" × 30"', 1, 0, DATEADD(day, -7, GETDATE())),
('Forest Path', 'A serene nature photograph capturing sunlight filtering through trees.', 2, 2, 550.00, '/images/artworks/artwork8.jpg', 2023, '18" × 24"', 1, 0, DATEADD(day, -6, GETDATE())),
('Desert Storm', 'Dramatic landscape photography capturing desert dunes under stormy skies.', 2, 2, 750.00, '/images/artworks/artwork11.png', 2023, '20" × 30"', 1, 0, DATEADD(day, -5, GETDATE())),
('Mountain Peak', 'Alpine photography with crisp details of snow-covered peaks.', 2, 2, 850.00, '/images/artworks/artwork13.jpg', 2023, '24" × 36"', 1, 0, DATEADD(day, -4, GETDATE())),

-- SCULPTURE (CategoryId = 3)
('Modern Form', 'A contemporary metal sculpture challenging traditional perceptions of space.', 3, 3, 2500.00, '/images/artworks/artwork4.jpg', 2024, '12" × 8" × 6"', 1, 0, DATEADD(day, -3, GETDATE())),
('Steel Harmony', 'An elegant metal sculpture exploring balance between strength and grace.', 3, 3, 3200.00, '/images/artworks/artwork9.jpg', 2024, '15" × 10" × 8"', 1, 0, DATEADD(day, -2, GETDATE())),
('Urban Sculpture', 'A bold bronze sculpture inspired by city architecture.', 3, 3, 4500.00, '/images/artworks/artwork14.jpg', 2024, '20" × 12" × 10"', 1, 0, DATEADD(day, -1, GETDATE()));

--